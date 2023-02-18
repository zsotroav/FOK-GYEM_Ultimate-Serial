
namespace SerialCommPlugin
{
    partial class FormConfig
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.timeoutNumeric = new System.Windows.Forms.NumericUpDown();
            this.COMCombo = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label6 = new System.Windows.Forms.Label();
            this.baudCombo = new System.Windows.Forms.ComboBox();
            this.parityCombo = new System.Windows.Forms.ComboBox();
            this.stopCombo = new System.Windows.Forms.ComboBox();
            this.configText = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(63, 15);
            this.label1.TabIndex = 0;
            this.label1.Text = "COM Port:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 15);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud rate:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 72);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(81, 15);
            this.label3.TabIndex = 3;
            this.label3.Text = "Timeout (ms):";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 101);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(40, 15);
            this.label4.TabIndex = 4;
            this.label4.Text = "Parity:";
            // 
            // timeoutNumeric
            // 
            this.timeoutNumeric.Location = new System.Drawing.Point(112, 70);
            this.timeoutNumeric.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.timeoutNumeric.Name = "timeoutNumeric";
            this.timeoutNumeric.Size = new System.Drawing.Size(130, 23);
            this.timeoutNumeric.TabIndex = 6;
            this.timeoutNumeric.Value = new decimal(new int[] {
            500,
            0,
            0,
            0});
            // 
            // COMCombo
            // 
            this.COMCombo.FormattingEnabled = true;
            this.COMCombo.Location = new System.Drawing.Point(112, 12);
            this.COMCombo.Name = "COMCombo";
            this.COMCombo.Size = new System.Drawing.Size(130, 23);
            this.COMCombo.TabIndex = 8;
            this.COMCombo.Text = "COM1";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 198);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(230, 23);
            this.button1.TabIndex = 10;
            this.button1.Text = "Start connection";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 130);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(56, 15);
            this.label6.TabIndex = 12;
            this.label6.Text = "Stop bits:";
            // 
            // baudCombo
            // 
            this.baudCombo.FormattingEnabled = true;
            this.baudCombo.Items.AddRange(new object[] {
            "110",
            "300",
            "1200",
            "2400",
            "4800",
            "9600",
            "19200",
            "38400",
            "57600",
            "115200"});
            this.baudCombo.Location = new System.Drawing.Point(112, 41);
            this.baudCombo.Name = "baudCombo";
            this.baudCombo.Size = new System.Drawing.Size(130, 23);
            this.baudCombo.TabIndex = 2;
            this.baudCombo.Text = "9600";
            // 
            // parityCombo
            // 
            this.parityCombo.FormattingEnabled = true;
            this.parityCombo.Items.AddRange(new object[] {
            "None",
            "Even",
            "Odd",
            "Mark",
            "Space"});
            this.parityCombo.Location = new System.Drawing.Point(112, 98);
            this.parityCombo.Name = "parityCombo";
            this.parityCombo.Size = new System.Drawing.Size(130, 23);
            this.parityCombo.TabIndex = 15;
            this.parityCombo.Text = "None";
            // 
            // stopCombo
            // 
            this.stopCombo.FormattingEnabled = true;
            this.stopCombo.Items.AddRange(new object[] {
            "One",
            "OnePointFive",
            "Two"});
            this.stopCombo.Location = new System.Drawing.Point(112, 127);
            this.stopCombo.Name = "stopCombo";
            this.stopCombo.Size = new System.Drawing.Size(130, 23);
            this.stopCombo.TabIndex = 16;
            this.stopCombo.Text = "One";
            // 
            // configText
            // 
            this.configText.BackColor = System.Drawing.SystemColors.Control;
            this.configText.Enabled = false;
            this.configText.Location = new System.Drawing.Point(12, 169);
            this.configText.Name = "configText";
            this.configText.Size = new System.Drawing.Size(230, 23);
            this.configText.TabIndex = 17;
            this.configText.Text = "config";
            this.configText.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // FormConfig
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(254, 233);
            this.Controls.Add(this.configText);
            this.Controls.Add(this.stopCombo);
            this.Controls.Add(this.parityCombo);
            this.Controls.Add(this.baudCombo);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.COMCombo);
            this.Controls.Add(this.timeoutNumeric);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "FormConfig";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Serial Communication Config";
            ((System.ComponentModel.ISupportInitialize)(this.timeoutNumeric)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown timeoutNumeric;
        private System.Windows.Forms.ComboBox COMCombo;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox baudCombo;
        private System.Windows.Forms.ComboBox parityCombo;
        private System.Windows.Forms.ComboBox stopCombo;
        private System.Windows.Forms.TextBox configText;
    }
}