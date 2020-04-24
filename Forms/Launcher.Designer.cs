namespace OmicronMain
{
    partial class Launcher
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Launcher));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.selectOmicron = new System.Windows.Forms.Button();
            this.omicronGithub = new System.Windows.Forms.Button();
            this.selectOmicronPi = new System.Windows.Forms.Button();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 146);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(299, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Omicron is designed for creating config_gameplay files.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(446, 146);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(395, 15);
            this.label2.TabIndex = 3;
            this.label2.Text = "Omicron Pi is designed for editing and creating config_remoteadmin files.";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Noto Sans", 27.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(236, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(343, 51);
            this.label3.TabIndex = 4;
            this.label3.Text = "Program Selection";
            // 
            // selectOmicron
            // 
            this.selectOmicron.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.selectOmicron.Location = new System.Drawing.Point(111, 164);
            this.selectOmicron.Name = "selectOmicron";
            this.selectOmicron.Size = new System.Drawing.Size(147, 23);
            this.selectOmicron.TabIndex = 5;
            this.selectOmicron.Text = "Select";
            this.selectOmicron.UseVisualStyleBackColor = true;
            this.selectOmicron.Click += new System.EventHandler(this.selectOmicron_Click);
            // 
            // omicronGithub
            // 
            this.omicronGithub.ForeColor = System.Drawing.SystemColors.ControlText;
            this.omicronGithub.Location = new System.Drawing.Point(744, 12);
            this.omicronGithub.Name = "omicronGithub";
            this.omicronGithub.Size = new System.Drawing.Size(92, 23);
            this.omicronGithub.TabIndex = 6;
            this.omicronGithub.Text = "GitHub";
            this.omicronGithub.UseVisualStyleBackColor = true;
            this.omicronGithub.Click += new System.EventHandler(this.omicronGithub_Click);
            // 
            // selectOmicronPi
            // 
            this.selectOmicronPi.BackColor = System.Drawing.Color.AliceBlue;
            this.selectOmicronPi.ForeColor = System.Drawing.SystemColors.ControlText;
            this.selectOmicronPi.Location = new System.Drawing.Point(571, 164);
            this.selectOmicronPi.Name = "selectOmicronPi";
            this.selectOmicronPi.Size = new System.Drawing.Size(147, 23);
            this.selectOmicronPi.TabIndex = 7;
            this.selectOmicronPi.Text = "Select";
            this.selectOmicronPi.UseVisualStyleBackColor = false;
            this.selectOmicronPi.Click += new System.EventHandler(this.selectOmicronPi_Click);
            // 
            // pictureBox2
            // 
            this.pictureBox2.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox2.Image = global::OmicronMain.Properties.Resources.omicronpifull;
            this.pictureBox2.Location = new System.Drawing.Point(530, 85);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(236, 58);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
            this.pictureBox1.Image = global::OmicronMain.Properties.Resources.omicrontype;
            this.pictureBox1.Location = new System.Drawing.Point(68, 85);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(235, 58);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // Launcher
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.BackgroundImage = global::OmicronMain.Properties.Resources.omicronsplashscreenBg;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ClientSize = new System.Drawing.Size(848, 256);
            this.Controls.Add(this.selectOmicronPi);
            this.Controls.Add(this.omicronGithub);
            this.Controls.Add(this.selectOmicron);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBox2);
            this.Controls.Add(this.pictureBox1);
            this.DoubleBuffered = true;
            this.Font = new System.Drawing.Font("Noto Sans", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ForeColor = System.Drawing.SystemColors.Control;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Launcher";
            this.Text = "Launcher";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button selectOmicron;
        private System.Windows.Forms.Button omicronGithub;
        private System.Windows.Forms.Button selectOmicronPi;
    }
}