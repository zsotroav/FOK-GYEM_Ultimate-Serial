using System;
using System.IO.Ports;
using PluginBase;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace SerialCommPlugin
{
    public class Connection
    {
        ///<summary>Controller protocol version number</summary>
        public static readonly Byte VEController = 0x00;
        
        ///<summary>Display protocol version number</summary>
        public Byte VEDisplay;

        ///<summary>Connection reference number</summary>
        public Byte CD;

        public SerialPort Port; 

        public Connection(string portName, 
            int baudRate,
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
            Port.Open();
        }

        /// <summary>Begin connection</summary>
        /// <returns>Whether or not the controller accepted the connection</returns>
        public bool Init(int Count, int Width, int Height)
        {
            byte[] initMessage = { 0xAA, 0x55, 0xAA, 0x55, VEController, (byte)Count, (byte)Width, (byte)Height};
            Port.Write(initMessage,0,8);

            var buff = new byte[6]; 
            ReadExactly(buff, Port);
            
            // +----+----+----+----+----+----+
            // | VE | CN | Wd | He | AC | CD |
            // +----+----+----+----+----+----+
            // | 00 | 07 | 18 | 07 | FF | AF |
            // +----+----+----+----+----+----+

            VEDisplay = buff[0];

            // If we agree on all the parameters, consider the connection established
            if (buff[1] == (byte)Count && buff[2] == (byte)Width && buff[3] == (byte)Height && buff[4] == 0xFF)
            {
                CD = buff[5];
                return true;
            }

            return false;
        }

        /// <summary>Close connection</summary>
        public void Destroy()
        {
            Port.Write(new byte[] { CD, 0x0F, 0x00, 0x00 }, 0, 4);
            Port.Close();
        }

        /// <summary>Re-write the whole screen</summary>
        /// <param name="data">Data to write</param>
        public void FullScreenWrite(byte[] data)
        {
            byte[] msg = { CD, 0x11, (byte)(data.Length>>8), (byte)data.Length};
            Port.Write(msg,0, 4);
            Port.Write(data, 0, data.Length);
            GetResponse();
        }

        public void PixelWrite(pixelData pixel)
        {
            byte[] msg = { CD, 0x12, 0x00, 0x03, (byte)pixel.loc.x, (byte)pixel.loc.y, (byte)(pixel.state ? 0x01 : 0x00) };
            Port.Write(msg, 0, 7);
            GetResponse();
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
    }
}