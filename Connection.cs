using System;
using System.IO.Ports;

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
            StopBits stopBits = StopBits.None,
            int dataBits = 8,
            Handshake handshake = Handshake.None)
        {
            Port = new SerialPort
            {
                Site = null,
                BaudRate = baudRate,
                BreakState = false,
                DataBits = dataBits,
                DiscardNull = false,
                DtrEnable = false,
                Encoding = null,
                Handshake = handshake,
                NewLine = null,
                Parity = parity,
                ParityReplace = 0,
                PortName = portName,
                ReadBufferSize = 0,
                ReadTimeout = timeout,
                ReceivedBytesThreshold = 0,
                RtsEnable = false,
                StopBits = stopBits,
                WriteBufferSize = 0,
                WriteTimeout = timeout
            };
        }

        /// <summary>Begin connection</summary>
        /// <returns>Whether or not the controller accepted the connection</returns>
        public bool Init(int Count, int Width, int Height)
        {
            byte[] initMessage = { 0xAA, 0x55, 0xAA, 0x55, VEController, (byte)Count, (byte)Width, (byte)Height};
            Port.Write(initMessage,0,8);

            var buff = new byte[8];
            Port.Read(buff, 0, 10);

            // +----+----+----+----+----+----+---------+
            // | VE | CN | Wd | He | AC | CD | CONTROL |
            // +----+----+----+----+----+----+----+----+
            // | 00 | 07 | 18 | 07 | FF | AF | AF | 00 |  - Accepted connection
            // +----+----+----+----+----+----+----+----+
            //
            // We ignore the last 2 data bytes (LENDATA)

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
        public void Destroy() => Port.Write(new byte[] { CD, 0x0F, 0x00, 0x00 }, 0, 4);

        /// <summary>Re-write the whole screen</summary>
        /// <param name="data">Data to write</param>
        public void FullScreenWrite(byte[] data)
        {
            byte[] msg = { CD, 0x11, (byte)data.Length};
            Port.Write(msg,0, 4);
        }
    }
}