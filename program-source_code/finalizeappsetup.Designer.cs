namespace CD_Player
{
    partial class finalizeappsetup
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(finalizeappsetup));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.button3 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel2 = new System.Windows.Forms.Panel();
            this.doneLabel = new System.Windows.Forms.Label();
            this.installLabel = new System.Windows.Forms.Label();
            this.PiLLabel = new System.Windows.Forms.Label();
            this.greenArrow = new System.Windows.Forms.PictureBox();
            this.welcomeLabel = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.formname = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenArrow)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button1.Location = new System.Drawing.Point(270, 165);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(319, 51);
            this.button1.TabIndex = 2;
            this.button1.Text = "Light Mode";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Font = new System.Drawing.Font("Microsoft YaHei", 15.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button2.Location = new System.Drawing.Point(270, 222);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(319, 51);
            this.button2.TabIndex = 3;
            this.button2.Text = "Dark Mode";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(267, 309);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(127, 21);
            this.label3.TabIndex = 4;
            this.label3.Text = "Selected mode:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(400, 309);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(55, 21);
            this.label4.TabIndex = 5;
            this.label4.Text = "label4";
            // 
            // button3
            // 
            this.button3.Font = new System.Drawing.Font("Microsoft YaHei", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.button3.Location = new System.Drawing.Point(593, 434);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(118, 30);
            this.button3.TabIndex = 6;
            this.button3.Text = "Finish";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(161, 362);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(440, 57);
            this.label5.TabIndex = 7;
            this.label5.Text = "NOTE: \r\nClicking \'Finish\' will save the chosen options and close this application" +
    ",\r\nreopen it to see the changes take effect.";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel2);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(152, 476);
            this.panel1.TabIndex = 8;
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(194)))), ((int)(((byte)(194)))), ((int)(((byte)(194)))));
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.doneLabel);
            this.panel2.Controls.Add(this.installLabel);
            this.panel2.Controls.Add(this.PiLLabel);
            this.panel2.Controls.Add(this.greenArrow);
            this.panel2.Controls.Add(this.welcomeLabel);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 152);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(152, 324);
            this.panel2.TabIndex = 3;
            // 
            // doneLabel
            // 
            this.doneLabel.AutoSize = true;
            this.doneLabel.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.doneLabel.Location = new System.Drawing.Point(41, 160);
            this.doneLabel.Name = "doneLabel";
            this.doneLabel.Size = new System.Drawing.Size(62, 40);
            this.doneLabel.TabIndex = 6;
            this.doneLabel.Text = "Finalize\r\nSetup";
            // 
            // installLabel
            // 
            this.installLabel.AutoSize = true;
            this.installLabel.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.installLabel.Location = new System.Drawing.Point(41, 129);
            this.installLabel.Name = "installLabel";
            this.installLabel.Size = new System.Drawing.Size(51, 20);
            this.installLabel.TabIndex = 4;
            this.installLabel.Text = "Install";
            // 
            // PiLLabel
            // 
            this.PiLLabel.AutoSize = true;
            this.PiLLabel.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.PiLLabel.Location = new System.Drawing.Point(41, 59);
            this.PiLLabel.Name = "PiLLabel";
            this.PiLLabel.Size = new System.Drawing.Size(72, 60);
            this.PiLLabel.TabIndex = 2;
            this.PiLLabel.Text = "Program\r\ninstall\r\nlocation";
            // 
            // greenArrow
            // 
            this.greenArrow.Image = global::CD_Player.Properties.Resources.whistler_xp_icons_redesigned_see_captions_v0_0u05lpyyffnc1;
            this.greenArrow.Location = new System.Drawing.Point(11, 29);
            this.greenArrow.Name = "greenArrow";
            this.greenArrow.Size = new System.Drawing.Size(24, 20);
            this.greenArrow.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.greenArrow.TabIndex = 1;
            this.greenArrow.TabStop = false;
            // 
            // welcomeLabel
            // 
            this.welcomeLabel.AutoSize = true;
            this.welcomeLabel.Font = new System.Drawing.Font("Microsoft YaHei", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.welcomeLabel.Location = new System.Drawing.Point(41, 29);
            this.welcomeLabel.Name = "welcomeLabel";
            this.welcomeLabel.Size = new System.Drawing.Size(78, 20);
            this.welcomeLabel.TabIndex = 0;
            this.welcomeLabel.Text = "Welcome";
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(174)))), ((int)(((byte)(174)))), ((int)(((byte)(174)))));
            this.pictureBox1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Top;
            this.pictureBox1.Image = global::CD_Player.Properties.Resources.CD_icon_test;
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(152, 152);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft YaHei", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(187, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(214, 19);
            this.label7.TabIndex = 11;
            this.label7.Text = "What display mode do you want?";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft YaHei", 21.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(158, 22);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(358, 38);
            this.label8.TabIndex = 10;
            this.label8.Text = "Now to finish the setup..";
            // 
            // formname
            // 
            this.formname.AutoSize = true;
            this.formname.Location = new System.Drawing.Point(158, 9);
            this.formname.Name = "formname";
            this.formname.Size = new System.Drawing.Size(91, 13);
            this.formname.TabIndex = 9;
            this.formname.Text = "CD Ripper & Player";
            this.formname.Visible = false;
            // 
            // finalizeappsetup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(723, 476);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.formname);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "finalizeappsetup";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CD Ripper & Player";
            this.Load += new System.EventHandler(this.finalizeappsetup_Load);
            this.panel1.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.greenArrow)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label doneLabel;
        private System.Windows.Forms.Label installLabel;
        private System.Windows.Forms.Label PiLLabel;
        private System.Windows.Forms.PictureBox greenArrow;
        private System.Windows.Forms.Label welcomeLabel;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label formname;
    }
}