﻿namespace HtmlToImage.Windows
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
    using System.Net;
    using System.Net.Sockets;
    using System.IO;
    using rtaNetworking.Streaming;
    using System.Threading;
	public partial class MainForm : Form
	{
		
		public Size size = new Size(1920, 1080);
        private HtmlToBitmapConverter pubBrowse = new HtmlToBitmapConverter();
        public MainForm()
		{
			InitializeComponent();

            
		}
		public void setpictBoxSize(Size s)
		{
			pictureBox.Size = s; 
		}
		private void RenderHtmlToBitmapLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			
			pictureBox.Image = new HtmlToBitmapConverter().Render(htmlTextBox.Text, size);
		}
        public void updateAddress()
        {
            urlTextBox.Text = pubBrowse.pubbrowser.Url.ToString(); 
        }
		private void NavigateLinkLabelLinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
            navigateLink(); 
		}
        private void navigateLink()
        {
            try
            {
                pubBrowse.delegateNav(new Uri(urlTextBox.Text));
            }
            catch (Exception ex)
            {

            }
            finally
            {
                urlTextBox.Text = pubBrowse.pubbrowser.Url.ToString(); 
            }
        }
        public int port = 1261;
        public int outport = 8080;
        ImageStreamingServer server = new ImageStreamingServer();
        void startup()
        {
            try
            {
                pictureBox.Image = pubBrowse.Render(new Uri(urlTextBox.Text), size);
                pubBrowse.startConverter();
                pictureBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(picture_keyPress);
                startUpdate(); 
                server.ImagesSource = pictureNumerator();
                server.Start(outport);
                timer1.Start(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
            }
            
        }
        public string LocalIPAddress()
        {
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                }
            }
            return localIP;
        }
        public static byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        private void picture_keyPress(object sender, PreviewKeyDownEventArgs e)
        {
            pubBrowse.keyboardSend(e.KeyCode); 
        }

        public void UpdateThings()
        {
            try
            {
                continuousUpdate();
                
            }
            catch (Exception ex)
            {
            }
        }
        public Bitmap bmp;
        public bool isConnected = false;
        List<Image> picList = new List<Image>();
        public void startUpdate()
        {
            Thread t = new Thread(updateThread);
            t.Start(); 
            
        }
        public void updateThread()
        {
            while (true)
            {
                continuousUpdate(); 
            }
        }
        public void continuousUpdate()
        {
            bmp = pubBrowse.delegateScreenshot();
            GC.Collect(); 
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox.Image = bmp; 
        }
		
		void MainFormLoad(object sender, EventArgs e)
		{
			startup(); 			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
		}
		void PictureBoxClick(object sender, EventArgs e)
		{
		}
		void PictureBoxMouseDown(object sender, MouseEventArgs e)
		{
			Point mouseDownLocation = new Point(e.X, e.Y); 
            Point newPoint = getProportion(mouseDownLocation, pictureBox.Size, pubBrowse.pubbrowser.Size);
            pubBrowse.DoMouseLeftClick(newPoint); 
		}
		public Point getProportion(Point clickPoint, Size pictureBox, Size webBrowser)
		{
			float newX = (clickPoint.X * webBrowser.Width)/pictureBox.Width;
			float newY = (clickPoint.Y * webBrowser.Height)/pictureBox.Height;
            return new Point((int)Math.Round(newX), (int)Math.Round(newY)); 
		}
		
		void Button2Click(object sender, EventArgs e)
		{
            Object o = pubBrowse.pubbrowser.ActiveXInstance; 
		}

        private void urlTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                navigateLink(); 
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            pictureBox.Image.Save("test.bmp");

        }
        public IEnumerable<Image> pictureNumerator()
        {
            while (true)
            {
                yield return bmp; 
            }

            yield break;
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0); 
        }
	}
}