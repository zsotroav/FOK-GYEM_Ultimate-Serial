using System;
using System.IO.Ports;

namespace SerialCommPlugin
{
    public class Connection
    {

        public SerialPort Port;

        public Connection(string portName, 
            int baudRate,
            Parity parity,
            int dataBits,
            StopBits stopBits,
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
                ReadTimeout = 500,
                ReceivedBytesThreshold = 0,
                RtsEnable = false,
                StopBits = stopBits,
                WriteBufferSize = 0,
                WriteTimeout = 500
            };
        }
    }
}