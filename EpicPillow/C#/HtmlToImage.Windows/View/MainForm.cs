namespace HtmlToImage.Windows
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using EyeOpen.Imaging;
	using System.Data;
	using System.ComponentModel;
	using System.Collections.Generic;
	using System.Text;
	using System.Runtime.InteropServices; 
	
	public partial class MainForm : Form
	{
		
		public Size size = new Size(1920, 1080);
        private HtmlToBitmapConverter pubBrowse = new HtmlToBitmapConverter(); 
		public MainForm()
		{
			InitializeComponent();

			//SetHtml();
            
		}
		public void setpictBoxSize(Size s)
		{
			pictureBox.Size = s; 
		}
		private void RenderHtmlToBitmapLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			
			pictureBox.Image =
				new HtmlToBitmapConverter()
					.Render(htmlTextBox.Text, size);
		}

		private void NavigateLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
            pubBrowse.delegateNav(new Uri(urlTextBox.Text)); 
		}
        void startup()
        {
            pictureBox.Image = pubBrowse.Render(new Uri(urlTextBox.Text), size);
            pubBrowse.startConverter(); 
            //updatetmr.Start(); 
            //pictureBox.Image.Save("test.bmp");
            //System.Diagnostics.Process.Start("test.bmp"); 
            timer1.Start(); 
            SetHtml(); 
        }
        public void UpdateThings()
        {
            continuousUpdate(); 
        }
        Bitmap bmp; 
        public void continuousUpdate()
        {
            bmp = null; 
            bmp = pubBrowse.delegateScreenshot();
            pictureBox.Image = bmp;
            //pubBrowse.btnMouseClick_Click(100, 80); 
        }
		private void SetHtml()
		{
			const string Html = 
				"<html>" + "\r\n" +
				"\t<body>" + "\r\n" +
				"\t\tleft" + "\r\n" +
				"\t\t<div style=\"text-align: right\">" + "\r\n" +
				"\t\t\tright" + "\r\n" +
				"\t\t</div>" + "\r\n" +
				"\t</body>" + "\r\n" +
			    "</html>";

			htmlTextBox.Text = Html;
		}

        private void timer1_Tick(object sender, EventArgs e)
        {
            continuousUpdate(); 
        }
		
		void MainFormLoad(object sender, EventArgs e)
		{
			startup(); 			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			pubBrowse.btnMouseClick_Click(Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text)); 
		}
		
	}
}