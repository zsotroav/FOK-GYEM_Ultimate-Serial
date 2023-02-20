using System;
using System.IO.Ports;
using PluginBase;

namespace SerialCommPlugin
{
    public class Connection
    {
        ///<summary>Controller protocol version number</summary>
        public static readonly byte VEController = 0x01;
        
        ///<summary>Display protocol version number</summary>
        public byte VEDisplay;

        ///<summary>Connection reference number</summary>
        public byte CD;

        public SerialPort Port;

        public bool IsConnected;

        public Connection(string portName = "COM1", 
            int baudRate = 9600,
            int timeout = 500,
            Parity parity = Parity.None, 
            StopBits stopBits = StopBits.One,
            int dataBits = 8)
        {
            Port = new SerialPort
            {
                PortName = portName,
                BaudRate = baudRate,
                Parity = parity,
                DataBits = dataBits,
                Handshake = Handshake.None,
                ReadTimeout = timeout,
                StopBits = stopBits,
                WriteTimeout = timeout
            };
        }

        /// <summary>Begin connection</summary>
        /// <returns>Whether or not the controller accepted the connection</returns>
        public bool Init(int count, int width, int height)
        {
            Port.Open();
            byte[] initMessage = { 0xAA, 0x55, 0xAA, 0x55, VEController, (byte)count, (byte)width, (byte)height};
            Port.Write(initMessage,0,8);

            var buff = new byte[6];
            try
            {
                ReadExactly(buff, Port);
            }
            catch (Exception ex)
            {
                SDK.Communicate("Serial COM Exception", ex.Message, "error");
                Port.Close();
                IsConnected = false;
                return false;
            }

            // +----+----+----+----+----+----+
            // | VE | CN | Wd | He | AC | CD |
            // +----+----+----+----+----+----+
            // | 01 | 07 | 18 | 07 | FF | AF |
            // +----+----+----+----+----+----+

            VEDisplay = buff[0];

            // If we agree on all the parameters, consider the connection established
            if (buff[1] == (byte)count && buff[2] == (byte)width && buff[3] == (byte)height && buff[4] == 0xFF)
            {
                CD = buff[5];
                IsConnected = true;
                FullScreenWrite(Utils.ToByteArray(SDK.GetScreenState()));
                return true;
            }

            Port.Close();
            IsConnected = false;
            return false;
        }

        /// <summary>Close connection</summary>
        public void Destroy()
        {
            Port.Write(new byte[] { CD, 0x0F, 0x00, 0x00 }, 0, 4);
            IsConnected = false;
            Port.Close();
        }

        /// <summary>Re-write the whole screen with the driver's optimizations in place</summary>
        /// <param name="data">Data to write</param>
        /// <param name="toReverse">If the bytes need to be reversed (little endian &lt;--&gt; big endian)</param>
        public void FullScreenWrite(byte[] data, bool toReverse = true)
        {
            if (toReverse) data = Utils.ReverseBytes(data);
            try
            {
                byte[] msg = { CD, 0x11, (byte)(data.Length >> 8), (byte)data.Length };
                Port.Write(msg, 0, 4);
                Port.Write(data, 0, data.Length);
                GetResponse();
            } catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Re-write the whole screen with driver_forceScreenWrite (SLOW!)
        /// This bypasses any checks the driver arduino may make to
        /// save on resources, and as such should only be used when
        /// regular full screen writing doesn't work as expected.
        /// </summary>
        /// <param name="data">Data to write</param>
        /// <param name="toReverse"></param>
        public void ForceScreenWrite(byte[] data, bool toReverse = true)
        {
            if (toReverse) data = Utils.ReverseBytes(data);

            // Force writing is slow and a timeout error is very likely at usual values.
            Port.ReadTimeout += 2000;
            try
            {
                byte[] msg = { CD, 0x19, (byte)(data.Length >> 8), (byte)data.Length };
                Port.Write(msg, 0, 4);
                Port.Write(data, 0, data.Length);
                GetResponse();
            }
            catch (Exception ex) { HandleException(ex); }

            Port.ReadTimeout -= 2000;
        }

        public void PixelWrite(PixelData pixel)
        {
            try
            {
                byte[] msg =
                    { CD, 0x12, 0x00, 0x03, (byte)pixel.Loc.X, (byte)pixel.Loc.Y, (byte)(pixel.State ? 0x01 : 0x00) };
                Port.Write(msg, 0, 7);
                GetResponse();
            } catch (Exception ex) { HandleException(ex); }
        }

        /// <summary>
        /// Reads exactly as much data as the buffer can hold
        /// </summary>
        /// <param name="buffer">The buffer to read data to</param>
        /// <param name="port">The Serial Port to read data from</param>
        public static void ReadExactly(byte[] buffer, SerialPort port)
        {
            for (int offset = 0; offset < buffer.Length;)
            {
                int n = port.Read(buffer, offset, buffer.Length - offset);
                offset += n;
            }
        }

        /// <summary>
        /// Get the serial connected controller's response and deal with errors
        /// </summary>
        public void GetResponse()
        {
            var buffRe = new byte[2];
            ReadExactly(buffRe, Port);
            if (buffRe[0] == CD && buffRe[1] == 0x01) return;
            Error(buffRe[1]);
        }

        /// <summary>
        /// Handle the known error codes with alerts towards the user.
        /// </summary>
        /// <param name="errNumber">Error code</param>
        public void Error(byte errNumber)
        {
            var message = errNumber switch
            {
                0x02 => "An internal exception occurred in the serial connected controller.\nRestarting the controller and the connection is recommended.",
                0x03 => "There was an error with the screen\nCheck all connections; restarting the controller and the connection is recommended.",
                0x04 => "Malfunction in the serial connected controller. Make sure protocol versions are identical.\nDisconnected from controller.\n(Unknown command)",
                0x05 => "Malfunction in the serial connected controller or communicator plugin. Make sure protocol versions are identical.\nDisconnected from controller.\n(Bad data)",
                _ => "An unknown error occurred during serial communication"
            };

            SDK.Communicate("Serial Communication error", message, "error");

            if (errNumber == 0x04 || errNumber == 0x05) Destroy();
        }

        public void HandleException(Exception ex)
        {
            if (ex is InvalidOperationException)
            {
                SDK.Communicate("Serial COM Exception", "The serial connected controller appears to have been disconnected.", "error");
                IsConnected = false;
            } else
                SDK.Communicate("Serial COM Exception", ex.Message, "error");
        }
    }
}