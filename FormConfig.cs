using System;
using System.IO.Ports;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using PluginBase;

namespace SerialCommPlugin
{
    public partial class FormConfig : Form
    {
        public Connection Connection;
        public bool Success;
        
        public FormConfig()
        {
            InitializeComponent();

            COMCombo.Items.AddRange(SerialPort.GetPortNames());
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var parity = parityCombo.Text switch
            {
                "Even" => Parity.Even,
                "Odd" => Parity.Odd,
                "Mark" => Parity.Mark,
                "Space" => Parity.Space,
                _ => Parity.None
            };
            var stop = stopCombo.Text switch
            {
                "OnePointFive" => StopBits.OnePointFive,
                "Two" => StopBits.Two,
                _ => StopBits.One
            };

            try
            {
                Connection = new Connection(
                    COMCombo.Text,
                    int.Parse(baudCombo.Text),
                    (int)timeoutNumeric.Value,
                    parity,
                    stop);
                if (!Connection.Init((int)modNumeric.Value, (int)widthNumeric.Value, (int)heightNumeric.Value))
                    throw new ExternalException("Initializing connection failed");

                Success = true;
            }
            catch (Exception ex)
            {
                SDK.Communicate("Serial COM Exception", ex.Message, "error");
            }

            Close();
        }
    }
}
