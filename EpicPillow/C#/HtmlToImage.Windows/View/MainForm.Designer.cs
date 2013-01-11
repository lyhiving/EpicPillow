namespace HtmlToImage.Windows
{
	partial class MainForm
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
			this.components = new System.ComponentModel.Container();
			this.label1 = new System.Windows.Forms.Label();
			this.urlTextBox = new System.Windows.Forms.TextBox();
			this.navigateLinkLabel = new System.Windows.Forms.LinkLabel();
			this.htmlTextBox = new System.Windows.Forms.TextBox();
			this.renderLinkLabel = new System.Windows.Forms.LinkLabel();
			this.pictureBox = new System.Windows.Forms.PictureBox();
			this.timer1 = new System.Windows.Forms.Timer(this.components);
			this.textBox1 = new System.Windows.Forms.TextBox();
			this.textBox2 = new System.Windows.Forms.TextBox();
			this.button1 = new System.Windows.Forms.Button();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(16, 11);
			this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(26, 17);
			this.label1.TabIndex = 0;
			this.label1.Text = "Url";
			// 
			// urlTextBox
			// 
			this.urlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.urlTextBox.Location = new System.Drawing.Point(16, 31);
			this.urlTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.urlTextBox.Name = "urlTextBox";
			this.urlTextBox.Size = new System.Drawing.Size(480, 22);
			this.urlTextBox.TabIndex = 1;
			this.urlTextBox.Text = "http://youtube.com";
			// 
			// navigateLinkLabel
			// 
			this.navigateLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.navigateLinkLabel.AutoSize = true;
			this.navigateLinkLabel.Location = new System.Drawing.Point(504, 34);
			this.navigateLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.navigateLinkLabel.Name = "navigateLinkLabel";
			this.navigateLinkLabel.Size = new System.Drawing.Size(27, 17);
			this.navigateLinkLabel.TabIndex = 2;
			this.navigateLinkLabel.TabStop = true;
			this.navigateLinkLabel.Text = "Go";
			this.navigateLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.NavigateLinkLabelLinkClicked);
			// 
			// htmlTextBox
			// 
			this.htmlTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.htmlTextBox.Enabled = false;
			this.htmlTextBox.Location = new System.Drawing.Point(16, 74);
			this.htmlTextBox.Margin = new System.Windows.Forms.Padding(4);
			this.htmlTextBox.Multiline = true;
			this.htmlTextBox.Name = "htmlTextBox";
			this.htmlTextBox.Size = new System.Drawing.Size(745, 138);
			this.htmlTextBox.TabIndex = 3;
			// 
			// renderLinkLabel
			// 
			this.renderLinkLabel.AutoSize = true;
			this.renderLinkLabel.Enabled = false;
			this.renderLinkLabel.Location = new System.Drawing.Point(16, 217);
			this.renderLinkLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
			this.renderLinkLabel.Name = "renderLinkLabel";
			this.renderLinkLabel.Size = new System.Drawing.Size(55, 17);
			this.renderLinkLabel.TabIndex = 4;
			this.renderLinkLabel.TabStop = true;
			this.renderLinkLabel.Text = "Render";
			this.renderLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.RenderHtmlToBitmapLinkClicked);
			// 
			// pictureBox
			// 
			this.pictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
									| System.Windows.Forms.AnchorStyles.Left) 
									| System.Windows.Forms.AnchorStyles.Right)));
			this.pictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
			this.pictureBox.Location = new System.Drawing.Point(13, 74);
			this.pictureBox.Margin = new System.Windows.Forms.Padding(4);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(788, 393);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox.TabIndex = 5;
			this.pictureBox.TabStop = false;
			// 
			// timer1
			// 
			this.timer1.Interval = 200;
			this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
			// 
			// textBox1
			// 
			this.textBox1.Location = new System.Drawing.Point(538, 31);
			this.textBox1.Name = "textBox1";
			this.textBox1.Size = new System.Drawing.Size(76, 22);
			this.textBox1.TabIndex = 6;
			// 
			// textBox2
			// 
			this.textBox2.Location = new System.Drawing.Point(620, 29);
			this.textBox2.Name = "textBox2";
			this.textBox2.Size = new System.Drawing.Size(81, 22);
			this.textBox2.TabIndex = 7;
			// 
			// button1
			// 
			this.button1.Location = new System.Drawing.Point(707, 28);
			this.button1.Name = "button1";
			this.button1.Size = new System.Drawing.Size(75, 23);
			this.button1.TabIndex = 8;
			this.button1.Text = "button1";
			this.button1.UseVisualStyleBackColor = true;
			this.button1.Click += new System.EventHandler(this.Button1Click);
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(814, 480);
			this.Controls.Add(this.button1);
			this.Controls.Add(this.textBox2);
			this.Controls.Add(this.textBox1);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.renderLinkLabel);
			this.Controls.Add(this.htmlTextBox);
			this.Controls.Add(this.navigateLinkLabel);
			this.Controls.Add(this.urlTextBox);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(4);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "EpicPillow Server - Eye.Open - HtmlToImage converter";
			this.Load += new System.EventHandler(this.MainFormLoad);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}
		private System.Windows.Forms.Button button1;
		private System.Windows.Forms.TextBox textBox2;
		private System.Windows.Forms.TextBox textBox1;

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox urlTextBox;
		private System.Windows.Forms.LinkLabel navigateLinkLabel;
		private System.Windows.Forms.TextBox htmlTextBox;
		private System.Windows.Forms.LinkLabel renderLinkLabel;
		public System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.Timer timer1;
	}
}

