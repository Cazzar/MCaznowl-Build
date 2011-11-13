namespace MCForge.GUI
{
    partial class Textures
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
            this.button1 = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.button2 = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.custom = new System.Windows.Forms.TextBox();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.cloud = new System.Windows.Forms.TextBox();
            this.fog = new System.Windows.Forms.TextBox();
            this.sky = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.panel3 = new System.Windows.Forms.Panel();
            this.button4 = new System.Windows.Forms.Button();
            this.label7 = new System.Windows.Forms.Label();
            this.terr = new System.Windows.Forms.TextBox();
            this.panel4 = new System.Windows.Forms.Panel();
            this.button5 = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.custom_side = new System.Windows.Forms.TextBox();
            this.side = new System.Windows.Forms.ComboBox();
            this.label10 = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(228, 204);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(116, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Save Textures";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.button2);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Controls.Add(this.custom);
            this.panel1.Controls.Add(this.comboBox1);
            this.panel1.Controls.Add(this.label1);
            this.panel1.Location = new System.Drawing.Point(12, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(260, 82);
            this.panel1.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(184, 59);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(73, 21);
            this.button2.TabIndex = 4;
            this.button2.Text = "More Info";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(145, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Custom";
            // 
            // custom
            // 
            this.custom.Enabled = false;
            this.custom.Location = new System.Drawing.Point(148, 25);
            this.custom.Name = "custom";
            this.custom.Size = new System.Drawing.Size(100, 20);
            this.custom.TabIndex = 2;
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Items.AddRange(new object[] {
            "Bedrock (Default)",
            "Custom (Texture File ID)",
            "Water",
            "Lava",
            "Blue",
            "Brick",
            "Coal",
            "Rock",
            "Cyan",
            "Dirt",
            "Gold Ore",
            "Gold",
            "Glass",
            "Light Grey",
            "Black",
            "Gravel",
            "Green",
            "Iron",
            "Iron Ore",
            "Light Green",
            "Log",
            "Sponge",
            "Staircase",
            "Stone",
            "TNT",
            "Mossy Cobblestone",
            "Obsidian",
            "Orange",
            "Pink",
            "Red",
            "Sand",
            "Purple",
            "White",
            "Wood",
            "Yellow"});
            this.comboBox1.Location = new System.Drawing.Point(6, 25);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(121, 21);
            this.comboBox1.TabIndex = 1;
            this.comboBox1.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(62, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Edge Block";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.cloud);
            this.panel2.Controls.Add(this.fog);
            this.panel2.Controls.Add(this.sky);
            this.panel2.Controls.Add(this.label6);
            this.panel2.Controls.Add(this.label5);
            this.panel2.Controls.Add(this.label4);
            this.panel2.Controls.Add(this.label3);
            this.panel2.Controls.Add(this.button3);
            this.panel2.Location = new System.Drawing.Point(12, 100);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(260, 98);
            this.panel2.TabIndex = 5;
            // 
            // cloud
            // 
            this.cloud.Location = new System.Drawing.Point(6, 73);
            this.cloud.Name = "cloud";
            this.cloud.Size = new System.Drawing.Size(100, 20);
            this.cloud.TabIndex = 14;
            // 
            // fog
            // 
            this.fog.Location = new System.Drawing.Point(6, 47);
            this.fog.Name = "fog";
            this.fog.Size = new System.Drawing.Size(100, 20);
            this.fog.TabIndex = 13;
            // 
            // sky
            // 
            this.sky.Location = new System.Drawing.Point(6, 20);
            this.sky.Name = "sky";
            this.sky.Size = new System.Drawing.Size(100, 20);
            this.sky.TabIndex = 12;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(112, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Cloud Color";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(112, 49);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Fog Color";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(112, 23);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Sky Color";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 4);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(36, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Colors";
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(182, 72);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(73, 21);
            this.button3.TabIndex = 4;
            this.button3.Text = "More Info";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.button4);
            this.panel3.Controls.Add(this.label7);
            this.panel3.Controls.Add(this.terr);
            this.panel3.Location = new System.Drawing.Point(278, 12);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(260, 82);
            this.panel3.TabIndex = 5;
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(182, 56);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(73, 21);
            this.button4.TabIndex = 4;
            this.button4.Text = "More Info";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 9);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(84, 13);
            this.label7.TabIndex = 3;
            this.label7.Text = "Terrain Textures";
            // 
            // terr
            // 
            this.terr.Location = new System.Drawing.Point(6, 25);
            this.terr.Name = "terr";
            this.terr.Size = new System.Drawing.Size(242, 20);
            this.terr.TabIndex = 2;
            this.terr.Text = "bc4acee575474f5266105430c3cc628b8b3948a2";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.button5);
            this.panel4.Controls.Add(this.label9);
            this.panel4.Controls.Add(this.custom_side);
            this.panel4.Controls.Add(this.side);
            this.panel4.Controls.Add(this.label10);
            this.panel4.Location = new System.Drawing.Point(278, 100);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(260, 98);
            this.panel4.TabIndex = 6;
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(182, 72);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(73, 21);
            this.button5.TabIndex = 4;
            this.button5.Text = "More Info";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(145, 9);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(42, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Custom";
            // 
            // custom_side
            // 
            this.custom_side.Location = new System.Drawing.Point(148, 25);
            this.custom_side.Name = "custom_side";
            this.custom_side.Size = new System.Drawing.Size(100, 20);
            this.custom_side.TabIndex = 2;
            // 
            // side
            // 
            this.side.FormattingEnabled = true;
            this.side.Items.AddRange(new object[] {
            "Water (Default)",
            "Custom (Texture File ID)",
            "Bedrock",
            "Lava",
            "Blue",
            "Brick",
            "Coal",
            "Rock",
            "Cyan",
            "Dirt",
            "Gold Ore",
            "Gold",
            "Glass",
            "Light Grey",
            "Black",
            "Gravel",
            "Green",
            "Iron",
            "Iron Ore",
            "Light Green",
            "Log",
            "Sponge",
            "Staircase",
            "Stone",
            "TNT",
            "Mossy Cobblestone",
            "Obsidian",
            "Orange",
            "Pink",
            "Red",
            "Sand",
            "Purple",
            "White",
            "Wood",
            "Yellow"});
            this.side.Location = new System.Drawing.Point(6, 25);
            this.side.Name = "side";
            this.side.Size = new System.Drawing.Size(121, 21);
            this.side.TabIndex = 1;
            this.side.SelectedIndexChanged += new System.EventHandler(this.side_SelectedIndexChanged);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 9);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(58, 13);
            this.label10.TabIndex = 0;
            this.label10.Text = "Side Block";
            // 
            // Textures
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(546, 233);
            this.Controls.Add(this.panel4);
            this.Controls.Add(this.panel3);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.button1);
            this.Name = "Textures";
            this.Text = "Textures";
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox custom;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox terr;
        private System.Windows.Forms.Panel panel4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox custom_side;
        private System.Windows.Forms.ComboBox side;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox cloud;
        private System.Windows.Forms.TextBox fog;
        private System.Windows.Forms.TextBox sky;
    }
}