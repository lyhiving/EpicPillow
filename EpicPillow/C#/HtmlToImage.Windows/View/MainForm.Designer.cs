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
			this.label1 = new System.Windows.Forms.Label();
			this.urlTextBox = new System.Windows.Forms.TextBox();
			this.navigateLinkLabel = new System.Windows.Forms.LinkLabel();
			this.htmlTextBox = new System.Windows.Forms.TextBox();
			this.renderLinkLabel = new System.Windows.Forms.LinkLabel();
			this.pictureBox = new System.Windows.Forms.PictureBox();
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
			this.urlTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.urlTextBox.Name = "urlTextBox";
			this.urlTextBox.Size = new System.Drawing.Size(496, 22);
			this.urlTextBox.TabIndex = 1;
			this.urlTextBox.Text = "http://www.google.com";
			// 
			// navigateLinkLabel
			// 
			this.navigateLinkLabel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.navigateLinkLabel.AutoSize = true;
			this.navigateLinkLabel.Location = new System.Drawing.Point(521, 34);
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
			this.htmlTextBox.Location = new System.Drawing.Point(16, 74);
			this.htmlTextBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.htmlTextBox.Multiline = true;
			this.htmlTextBox.Name = "htmlTextBox";
			this.htmlTextBox.Size = new System.Drawing.Size(496, 138);
			this.htmlTextBox.TabIndex = 3;
			// 
			// renderLinkLabel
			// 
			this.renderLinkLabel.AutoSize = true;
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
			this.pictureBox.Location = new System.Drawing.Point(16, 255);
			this.pictureBox.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.pictureBox.Name = "pictureBox";
			this.pictureBox.Size = new System.Drawing.Size(497, 248);
			this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox.TabIndex = 5;
			this.pictureBox.TabStop = false;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(565, 518);
			this.Controls.Add(this.pictureBox);
			this.Controls.Add(this.renderLinkLabel);
			this.Controls.Add(this.htmlTextBox);
			this.Controls.Add(this.navigateLinkLabel);
			this.Controls.Add(this.urlTextBox);
			this.Controls.Add(this.label1);
			this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
			this.Name = "MainForm";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "Eye.Open - HtmlToImage converter";
			((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();
		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.TextBox urlTextBox;
		private System.Windows.Forms.LinkLabel navigateLinkLabel;
		private System.Windows.Forms.TextBox htmlTextBox;
		private System.Windows.Forms.LinkLabel renderLinkLabel;
		public System.Windows.Forms.PictureBox pictureBox;
	}
}

