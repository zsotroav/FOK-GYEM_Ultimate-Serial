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
        public bool Success = true;
        
        public FormConfig()
        {
            InitializeComponent();
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
                "One" => StopBits.One,
                "OnePointFive" => StopBits.OnePointFive,
                "Two" => StopBits.Two,
                _ => StopBits.None
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
            }
            catch (Exception ex)
            {
                Success = false;
                SDK.Communicate("Serial COM Exception", ex.Message, "error");
                Close();
            }
            
            Close();
        }
    }
}
