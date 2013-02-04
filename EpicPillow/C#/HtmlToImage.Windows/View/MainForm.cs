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
    using System.Net;
    using System.Net.Sockets;
    using System.IO;
    using rtaNetworking.Streaming;
    using System.Threading;
    using MjpegProcessor; 
	public partial class MainForm : Form
	{
		
		public Size size = new Size(1920, 1080);
        private HtmlToBitmapConverter pubBrowse = new HtmlToBitmapConverter();
        MjpegDecoder _mjpeg; 
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
                //urlTextBox.Text = pubBrowse.pubbrowser.Url.ToString();
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
        void startup()
        {
            try
            {
                pictureBox.Image = pubBrowse.Render(new Uri(urlTextBox.Text), size);
                pubBrowse.startConverter();
                pictureBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(picture_keyPress);
                _mjpeg = new MjpegDecoder();
                _mjpeg.FrameReady += mjpeg_FrameReady;
                
                //updatetmr.Start(); 
                //pictureBox.Image.Save("test.bmp");
                //System.Diagnostics.Process.Start("test.bmp"); 
                //timer1.Start();
                startUpdate(); 

                //startContinuousThread(); 
                //SetHtml();
                ImageStreamingServer server = new ImageStreamingServer();
                server.ImagesSource = pictureNumerator();
                server.Start(outport);
                //MessageBox.Show("http://" + LocalIPAddress() + ":" + outport.ToString()); 
                _mjpeg.ParseStream(new Uri("http://" + LocalIPAddress() + ":" + outport.ToString())); 
                //_mjpeg.ParseStream(new Uri("http://192.168.0.2:8080")); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
            }
            
        }
        public void startupdatePicture()
        {
            Thread t = new Thread(updatePicture);
            t.Start(); 
        }
        public void updatePicture()
        {
            while (true)
            {
                pictureBox.Image = _mjpeg.Bitmap; 
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
        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            pictureBox.Image = e.Bitmap;
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
            //t.Priority = ThreadPriority.AboveNormal; 
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
            //bmp = null; 
            bmp = pubBrowse.delegateScreenshot();
            //pictureBox.Image = pubBrowse.delegateScreenshot();
            //bmp.Dispose();
            GC.Collect(); 
            //pubBrowse.btnMouseClick_Click(100, 80); 
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox.Image = _mjpeg.Bitmap; 
        }
		
		void MainFormLoad(object sender, EventArgs e)
		{
			startup(); 			
		}
		
		void Button1Click(object sender, EventArgs e)
		{
			//pubBrowse.btnMouseClick_Click(Int32.Parse(textBox1.Text), Int32.Parse(textBox2.Text)); 
		}
		
		
		void PictureBoxClick(object sender, EventArgs e)
		{
		}
		
		void PictureBoxMouseDown(object sender, MouseEventArgs e)
		{
			Point mouseDownLocation = new Point(e.X, e.Y); 
			//MessageBox.Show((e.X).ToString() + "," + (e.Y).ToString()); 
            Point newPoint = getProportion(mouseDownLocation, pictureBox.Size, pubBrowse.pubbrowser.Size);
            pubBrowse.DoMouseLeftClick(newPoint); 
			//pubBrowse.btnMouseClick_Click(newPoint.X, newPoint.Y);
		}
		public Point getProportion(Point clickPoint, Size pictureBox, Size webBrowser)
		{
			float newX = (clickPoint.X * webBrowser.Width)/pictureBox.Width;
			float newY = (clickPoint.Y * webBrowser.Height)/pictureBox.Height;
			
			//return new Point((int)Math.Floor(newX), (int)Math.Floor(newY));
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
            /*
            Bitmap dstImage; 
            while (true)
            {
                dstImage = pubBrowse.delegateScreenshot();
                dstImage.Save("test.bmp"); 
                yield return dstImage;

            }
            dstImage.Dispose(); 
            yield break;
            */
            //Image test = Image.FromFile("test.bmp"); 
            while (true)
            {
                //bmp = null; 
                //bmp = pubBrowse.delegateScreenshot();
                //pictureBox.Image = pubBrowse.delegateScreenshot(); 
                //GC.Collect();
                //MessageBox.Show("hello"); 
                //yield return dstImage;
                //yield return test; 
                yield return bmp; 
                //yield return pubBrowse.delegateScreenshot(); 

            }

            yield break;
        }
	}
}