using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using System.Runtime.InteropServices;
using PluginBase;
using Action = PluginBase.Action;

namespace SerialCommPlugin
{
    public class SerialComm : IPlugin
    {
        public string Name => "Serial Communications";
        public string Description => "A plugin to enable serial communication with the Arduino controller (built-in)";
        public string Author => "zsotroav";
        public string Link => "https://github.com/zsotroav/FOK-GYEM_Ultimate";

        public List<Action> Actions => new()
        {
            new Action
            {
                Menu = Menu.Export,
                ActionName = "Serial Comm",
                ActionID = 0,
                SubActions = new List<SubAction>()
                {
                    new() {ActionID = 1, ActionName = "Connect"},
                    new() {ActionID = 2, ActionName = "Disconnect"}
                }
            }
        };

        public int Init(IContext context)
        {
            // AllocConsole();
            // Console.OpenStandardOutput();

            SDK.PixelUpdatedEvent += ActionPD;
            SDK.ScreenUpdatedFull += ActionBA;
            return 0;
        }

        public int Run(IContext context, int runID)
        {
            switch (runID)
            {
                case 1:
                    Start();
                    break;
                case 2:
                    Disconnect();
                    break;
            }
            return 0;
        }

        public Connection connection;
        public bool active;

        public void Start()
        {
            connection = new Connection("COM3", 9600, Parity.None, 0, StopBits.None);
            //active = connection.Init();
            if (active) SDK.Communicate("Serial Connection", "Connection established");
            else SDK.Communicate("Serial Connection",  "Connection failed", "error");
        }

        public void Disconnect()
        {
            if (active) connection.Destroy();
            active = false;
        }

        public void ActionPD(pixelData data)
        {

        }

        public void ActionBA(BitArray data)
        {
          /*if (active)*/ connection.FullScreenWrite(Utils.ToByteArray(data));
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}