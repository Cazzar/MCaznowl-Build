using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
//using MCForge.Levels.Textures;

namespace MCForge.GUI
{
    public partial class Textures : Form
    {
        public Level l;
        public Textures()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (custom.Enabled)
                l.textures.ChangeEdge(custom.Text);
            else if (comboBox1.SelectedIndex != -1)
            {
                byte block = Block.Byte(comboBox1.Items[comboBox1.SelectedIndex].ToString().Replace(' ', '_'));
                try
                {
                    //To prevent something like this:
                    //Water (Default)
                    //We just want "Water"
                    if (comboBox1.Items[comboBox1.SelectedIndex].ToString().Split(' ')[1].StartsWith("("))
                    {
                        block = Block.Byte(comboBox1.Items[comboBox1.SelectedIndex].ToString().Split(' ')[0]);
                    }
                }
                catch { }
                l.textures.ChangeEdge(block);
            }
            if (cloud.Text != "")
                l.textures.ChangeCloud(cloud.Text);
            if (fog.Text != "")
                l.textures.ChangeFog(fog.Text);
            if (sky.Text != "")
                l.textures.ChangeSky(sky.Text);
            if (terr.Text != "")
                l.textures.terrainid = terr.Text;
            if (custom_side.Enabled)
                l.textures.side = custom_side.Text;
            else if (side.SelectedIndex != -1)
            {
                byte block = Block.Byte(side.Items[side.SelectedIndex].ToString().Replace(' ', '_'));
                try
                {
                    //To prevent something like this:
                    //Water (Default)
                    //We just want "Water"
                    if (side.Items[side.SelectedIndex].ToString().Split(' ')[1].StartsWith("("))
                    {
                        block = Block.Byte(side.Items[side.SelectedIndex].ToString().Split(' ')[0]);
                    }
                }
                catch { }
                l.textures.side = LevelTextures.GetBlockTexture(block);
            }
            l.textures.CreateCFG();
            this.Hide();
            this.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            MessageBox.Show("When using custom, please put in the texture file ID that will download from files.worldofminecraft.net. EX: f3dac271d7bce9954baad46e183a6a910a30d13b", "Help");
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Items[comboBox1.SelectedIndex].ToString().Split(' ')[0].ToLower() == "custom")
                custom.Enabled = true;
            else
                custom.Enabled = false;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Here you can change the colors of the sky, clouds, and even the fog! You must input hex colors ONLY! (Do not include the #)");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Here you input the terrian texture file ID that you uploaded to files.worldofminecraft.net. By default, it will use the default textures.", "Help");
        }

        private void side_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (side.Items[side.SelectedIndex].ToString().Split(' ')[0].ToLower() == "custom")
                custom_side.Enabled = true;
            else
                custom_side.Enabled = false;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Here you can change side block that the players see at the edge of the level (The default is bedrock)");
        }

        private void terr_TextChanged(object sender, EventArgs e)
        {

        }
    }
}
