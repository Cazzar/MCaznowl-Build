using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.CodeDom.Compiler;
using System.Reflection;
using System.Reflection.Emit;

namespace MCForge.GUI
{
    public partial class CustomLogout : Form
    {
        public CustomLogout()
        {
            InitializeComponent();
        }

        private void btnLoad_Click(object sender, EventArgs e)
        {
            if (txtPlayer.Text != null)
            {
                if (!File.Exists("text/logout/" + txtPlayer.Text + ".txt"))
                {
                    MessageBox.Show("Sorry, " + txtPlayer.Text + ".txt does not exist!");
                }
                else
                {
                    txtLoginMessage.Text = null;
                    txtLoginMessage.Text = File.ReadAllText("text/logout/" + txtPlayer.Text + ".txt");
                }
            }
            else
            {
                MessageBox.Show("You didn't specify a player!");
            }
        }

        private void btnDone_Click(object sender, EventArgs e)
        {
            if (txtPlayer.Text != null)
            {
                File.WriteAllText("text/logout/" + txtPlayer.Text + ".txt", null);
                File.WriteAllText("text/logout/" + txtPlayer.Text + ".txt", txtLoginMessage.Text);
                txtLoginMessage.Text = null;
                MessageBox.Show("The logout message has been saved!");
                Close();
            }
            else
            {
                MessageBox.Show("You didn't specify a player!");
            }
        }
    }
}
