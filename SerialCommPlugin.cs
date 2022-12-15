using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using PluginBase;
using Action = PluginBase.Action;

namespace SerialCommPlugin
{
    public class Test : IPlugin
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
                    new SubAction() {ActionID = 10, ActionName = "Connect"},
                    new SubAction() {ActionID = 11, ActionName = "Disconnect"}
                }
            }
        };

        public int Init(IContext context)
        {
            AllocConsole();
            Console.OpenStandardOutput();

            SDK.PixelUpdatedEvent += ActionPD;
            SDK.ScreenUpdatedFull += ActionBA;
            return 0;
        }

        public int Run(IContext context, int runID)
        {
            switch (runID)
            {
                case 10:
                    Connect();
                    break;
                case 11:
                    Disconnect();
                    break;
            }
            return 0;
        }

        public void Connect()
        {
            Console.WriteLine("AS");
        }

        public void Disconnect()
        {

        }

        public void ActionPD(pixelData data)
        {
        }

        public void ActionBA(System.Collections.BitArray i){
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}