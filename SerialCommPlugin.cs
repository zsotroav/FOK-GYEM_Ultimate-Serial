using System.Collections;
using System.Collections.Generic;
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
            },
            new Action{
                Menu = Menu.Plugin,
                ActionName = "Refresh full screen",
                ActionID = 8
            },
            new Action{
                Menu = Menu.Plugin,
                ActionName = "Force refresh screen",
                ActionID = 9
            }
        };

        private int _moduleCount, _width, _height;

        public int Init(IContext context)
        {
            // AllocConsole();
            // Console.OpenStandardOutput();

            (_moduleCount, _width, _height) = 
                (context.ModCnt, context.ModH, context.ModV);

            SDK.PixelUpdatedEvent += ActionPD;
            SDK.ScreenUpdatedFull += ActionBA;
            SDK.ScreenSizeChangedEvent += ScreenUpdated;
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
                case 8:
                    ManualWrite(context.ScreenState);
                    break;
                case 9:
                    ManualWrite(context.ScreenState, true);
                    break;
            }
            return 0;
        }

        public Connection Connection = new();

        public void Start()
        {
            if (Connection is { IsConnected: true })
            {
                SDK.Communicate("Serial Connection", "Connection already established!\nOnly one serial controller is supported at once.", "warning");
                return;
            }
            var form = new FormConfig(_moduleCount, _width, _height);
            form.ShowDialog();

            if (form.Success)
            {
                Connection = form.Connection;
                if (Connection.IsConnected) SDK.Communicate("Serial Connection", "Connection established");
                return;
            }

            SDK.Communicate("Serial Connection", "Connection failed", "error");
        }

        public void Disconnect()
        {
            if (Connection.IsConnected)
            {
                Connection.Destroy();
                SDK.Communicate("Serial COM disconnect", "Disconnected serial screen.");
                return;
            }
            SDK.Communicate("Serial COM disconnect", "Not connected, no connection to destroy!", "warning");
        }

        public void ActionPD(PixelData data)
        {
            if (!Connection.IsConnected) return;
            Connection.PixelWrite(data);
        }

        public void ActionBA(BitArray data)
        {
            if (!Connection.IsConnected) return;
            Connection.FullScreenWrite(Utils.ToByteArray(data));
        }

        public void ManualWrite(BitArray data, bool force = false)
        {
            if (!Connection.IsConnected)
            {
                SDK.Communicate("Serial COM Disconnected", "Controller not connected!", "warning");
                return;
            }

            if (force) Connection.ForceScreenWrite(Utils.ToByteArray(data));
            else Connection.FullScreenWrite(Utils.ToByteArray(data));
        }

        public void ScreenUpdated(int cnt, int _, int w, int h)
        {
            if (cnt == _moduleCount && w == _width && h == _height) return;
            (_moduleCount, _width, _height) = (cnt, w, h);

            if (!Connection.IsConnected) return;
            SDK.Communicate("Serial COM Warning", "Screen size changed, disconnecting from serial connected controller...", "warning");

            Disconnect();
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool AllocConsole();
    }
}