namespace MCForge.GUI
{
    partial class EditText
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
            this.EdittxtCombo = new System.Windows.Forms.ComboBox();
            this.LoadTxt = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.SaveEditTxtBt = new System.Windows.Forms.Button();
            this.DiscardEdittxtBt = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // EdittxtCombo
            // 
            this.EdittxtCombo.FormattingEnabled = true;
            this.EdittxtCombo.Location = new System.Drawing.Point(94, 13);
            this.EdittxtCombo.Name = "EdittxtCombo";
            this.EdittxtCombo.Size = new System.Drawing.Size(178, 21);
            this.EdittxtCombo.TabIndex = 0;
            // 
            // LoadTxt
            // 
            this.LoadTxt.Location = new System.Drawing.Point(13, 13);
            this.LoadTxt.Name = "LoadTxt";
            this.LoadTxt.Size = new System.Drawing.Size(75, 23);
            this.LoadTxt.TabIndex = 1;
            this.LoadTxt.Text = "Load:";
            this.LoadTxt.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(13, 43);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(259, 244);
            this.textBox1.TabIndex = 2;
            // 
            // SaveEditTxtBt
            // 
            this.SaveEditTxtBt.Location = new System.Drawing.Point(13, 293);
            this.SaveEditTxtBt.Name = "SaveEditTxtBt";
            this.SaveEditTxtBt.Size = new System.Drawing.Size(126, 23);
            this.SaveEditTxtBt.TabIndex = 3;
            this.SaveEditTxtBt.Text = "Save";
            this.SaveEditTxtBt.UseVisualStyleBackColor = true;
            // 
            // DiscardEdittxtBt
            // 
            this.DiscardEdittxtBt.Location = new System.Drawing.Point(145, 293);
            this.DiscardEdittxtBt.Name = "DiscardEdittxtBt";
            this.DiscardEdittxtBt.Size = new System.Drawing.Size(127, 23);
            this.DiscardEdittxtBt.TabIndex = 4;
            this.DiscardEdittxtBt.Text = "Discard";
            this.DiscardEdittxtBt.UseVisualStyleBackColor = true;
            // 
            // EditText
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 328);
            this.Controls.Add(this.DiscardEdittxtBt);
            this.Controls.Add(this.SaveEditTxtBt);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.LoadTxt);
            this.Controls.Add(this.EdittxtCombo);
            this.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "EditText";
            this.Text = "EditText";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox EdittxtCombo;
        private System.Windows.Forms.Button LoadTxt;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Button SaveEditTxtBt;
        private System.Windows.Forms.Button DiscardEdittxtBt;
    }
}