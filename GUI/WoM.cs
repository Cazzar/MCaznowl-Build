using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace MCForge.GUI
{
    public partial class WoM : Form
    {
        public WoM()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {
            if (textBox3.Text.ToCharArray().Length > 15)
            {
                MessageBox.Show("Only 15 characters allowed!", "Warning");
                return;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            WOMBeat.SetSettings(Server.IP, "" + Server.port, textBox1.Text, textBox2.Text, textBox3.Text);
            MessageBox.Show("Done!", "Results");
            this.Close();
        }
    }
}
