using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
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

        public bool IsConnected = false;

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

        public Connection Connection;

        public void Start()
        {
            if (IsConnected)
            {
                SDK.Communicate("Serial Connection", "Connection already established!\nOnly one serial controller is supported at once.", "warning");
                return;
            }
            var form = new FormConfig();
            form.ShowDialog();

            if (form.Success)
            {
                Connection = form.Connection;
                IsConnected = true;
            }

            if (IsConnected) SDK.Communicate("Serial Connection", "Connection established");
            else SDK.Communicate("Serial Connection", "Connection failed", "error");
        }

        public void Disconnect()
        {
            if (IsConnected)
            {
                Connection.Destroy();
                IsConnected = false;
                SDK.Communicate("Serial COM disconnect", "Disconnected serial screen.");
                return;
            }
            SDK.Communicate("Serial COM disconnect", "Not connected, no connection to destroy!", "warning");
        }

        public void ActionPD(pixelData data)
        {
            if (!IsConnected) return;
            Connection.PixelWrite(data);
        }

        public void ActionBA(BitArray data)
        {
            if (!IsConnected) return;
            Connection.FullScreenWrite(Utils.ToByteArray(data));
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}