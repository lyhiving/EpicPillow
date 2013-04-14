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
	public partial class MainForm : Form
	{
		public Size size = new Size(1366,768);
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
        public bool mjpgvideo = true;
        public bool jpgimg = true;
        public int jpgport = 1263;
        
        public void optsetPorts()
        {
            if (File.Exists("config.txt"))
            {
                try
                {
                    StreamReader sr = new StreamReader("config.txt");
                    string configRead = sr.ReadToEnd();
                    sr.Close();
                    string[] configLines = configRead.Split('\n');
                    foreach (string line in configLines)
                    {
                        if (line.StartsWith("udpport="))
                        {
                            udpport = Int32.Parse(line.Substring(8));
                        }
                        else if (line.StartsWith("mjpgport="))
                        {
                            mjpgport = Int32.Parse(line.Substring(9));
                        }
                        else if (line.StartsWith("mjpgvideo="))
                        {
                            string mjpgVal = line.Substring(10);
                            if (mjpgVal == "true")
                            {
                                mjpgvideo = true;
                            }
                            else if (mjpgVal == "false")
                            {
                                mjpgvideo = false;
                            }
                            else if (line.StartsWith("jpgimg="))
                            {
                                string jpgVal = line.Substring(7);
                                if (jpgVal == "true")
                                {
                                    jpgimg = true; 
                                }
                                else if (jpgVal == "false")
                                {
                                    jpgimg = false; 
                                }
                            }
                            else if (line.StartsWith("jpgport="))
                            {
                                jpgport = Int32.Parse(line.Substring(8)); 
                            }
                            else if (line.StartsWith("picBoxShow="))
                            {
                                string picShowVal = line.Substring(11);
                                if (picShowVal == "true")
                                {
                                    checkBox1.Checked = true;
                                    pictBoxShow = true;
                                }
                                else if (picShowVal == "false")
                                {
                                    checkBox1.Checked = false;
                                    pictBoxShow = false; 
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString()); 
                }
            }
        }
        private void navigateLink()
        {
            try
            {
                pubBrowse.delegateNav(new Uri(urlTextBox.Text));
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString()); 
            }
            finally
            {
                urlTextBox.Text = pubBrowse.pubbrowser.Url.ToString(); 
            }
        }
        public int udpport = 1261;
        public int mjpgport = 1262;
        HtmlToImage.Windows.View.PillowServer pillyServer;
        ImageStreamingServer server = new ImageStreamingServer();
        void startup()
        {
            try
            {
                optsetPorts();
                GlobalPillow.currentFrame = pubBrowse.Render(new Uri(urlTextBox.Text), size);
                pictureBox.Image = GlobalPillow.currentFrame; 
                pubBrowse.startConverter();
                pictureBox.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(picture_keyPress);
                startUpdate();
                startudpListen();
                if (mjpgvideo)
                {
                    server.ImagesSource = pictureNumerator();
                    server.Start(mjpgport);
                }
                if (jpgimg)
                {
                    startpillowServer(); 
                }
                timer1.Start(); 
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString()); 
            }
            
        }
        void startpillowServer()
        {
            pillyServer = new HtmlToImage.Windows.View.PillowServer(); 
            pillyServer.imageSrc = GlobalPillow.currentFrame;
            pillyServer.serverPort = jpgport;
            pillyServer.startPillow(); 
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
            pubBrowse.keyboardSend((int)e.KeyCode); 
        }

        public void UpdateThings()
        {
            try
            {
                continuousUpdate();
                
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString()); 
            }
        }
        
        public bool isConnected = false;
        List<Image> picList = new List<Image>();
        public void startUpdate()
        {
            Thread t = new Thread(updateThread);
            t.Priority = ThreadPriority.AboveNormal; 
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
            GlobalPillow.currentFrame = pubBrowse.delegateScreenshot();
            if (pictBoxShow)
            {
                SetPicture((Image)GlobalPillow.currentFrame.Clone()); 
            }
            GC.Collect(); 
        }
        bool pictBoxShow = false; 
        private void timer1_Tick(object sender, EventArgs e)
        {
            pictBoxShow = checkBox1.Checked; 
        }
        public void startudpListen()
        {
            Thread t = new Thread(udpListen);
            t.SetApartmentState(ApartmentState.STA); 
            t.Start();
        }
        private void SetPicture(Image img)
        {
            if (pictureBox.InvokeRequired)
            {
                pictureBox.Invoke(new MethodInvoker(
                delegate()
                {
                    pictureBox.Image = img;
                }));
            }
            else
            {
                pictureBox.Image = img;
            }
        }
        private void SetURL(string url)
        {
            if (urlTextBox.InvokeRequired)
            {
                urlTextBox.Invoke(new MethodInvoker(
                delegate()
                {
                    urlTextBox.Text = url;
                }));
            }
            else
            {
                urlTextBox.Text = url; 
            }
        }
        void udpListen()
        {
            byte[] data = new byte[1024];
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, udpport);
            UdpClient newsock = new UdpClient(ipep);
            IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
            while (true)
            {
                //try
                //{
                    data = newsock.Receive(ref sender);

                    //Console.WriteLine(Encoding.ASCII.GetString(data, 0, data.Length));

                    mail = Encoding.ASCII.GetString(data, 0, data.Length).Replace("\0", "");
                    //MessageBox.Show(mail); 
                    try
                    {
                        exec_Cmd(mail);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine(ex.ToString()); 
                    }
                    newsock.Send(data, data.Length, sender);
                //}
                //catch (Exception ex)
                //{

                //}
            }
        }

        KeysConverter kc = new KeysConverter(); 
        public void exec_Cmd(string recv)
        {
            System.Diagnostics.Debug.WriteLine(recv); 
            if (recv.StartsWith("lclick"))
            {
                string[] coordString = recv.Split('(')[1].Split(')')[0].Split(',');
                Point clickPoint = new Point(Int32.Parse(coordString[0]), Int32.Parse(coordString[1]));
                pubBrowse.DoMouseLeftClick(clickPoint); 
            }
            else if (recv.StartsWith("nav"))
            {
                string navurl = recv.Split('(')[1].Split(')')[0];
                SetURL(navurl); 
                pubBrowse.delegateNav(new Uri(urlTextBox.Text));
            }
            else if (recv.StartsWith("type"))
            {
                string typestring = recv.Split('(')[1].Split(')')[0];
                for (int i = 0; i < typestring.Length; i++)
                {
                    pubBrowse.SendStrokes((char)typestring[i]); 
                }
            }
            else if (recv.StartsWith("2lclick"))
            {
                string[] coordString = recv.Split('(')[1].Split(')')[0].Split(',');
                //Point clickPoint = new Point(Int32.Parse(coordString[0]), Int32.Parse(coordString[1]));
                //Size viewSize = new Size(Int32.Parse(coordString[2]), Int32.Parse(coordString[3]));
                //pubBrowse.DoMouseLeftClick(clickPoint);
                float newX = (((float)(Int32.Parse(coordString[0]))) * ((float)pubBrowse.pubbrowser.Size.Width)) / ((float)(Int32.Parse(coordString[2])));
                float newY = (((float)(Int32.Parse(coordString[1]))) * ((float)pubBrowse.pubbrowser.Size.Height)) / ((float)(Int32.Parse(coordString[3])));
                System.Diagnostics.Debug.WriteLine(newX.ToString());
                System.Diagnostics.Debug.WriteLine(newY.ToString());
                Point newPoint = new Point((int)Math.Round(newX), (int)Math.Round(newY)); 
                //Point newPoint = getProportion(clickPoint, viewSize, pubBrowse.pubbrowser.Size);
                
                //System.Diagnostics.Debug.Write(newPoint.ToString()); 
                pubBrowse.DoMouseLeftClick(newPoint); 
            }
            else if (recv.StartsWith("back"))
            {
                pubBrowse.goBack(); 
            }
            else if (recv.StartsWith("forward"))
            {
                pubBrowse.goForward(); 
            }
            else if (recv.StartsWith("home"))
            {
                pubBrowse.goHome(); 
            }
        }
        
        string mail = ""; 
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
                yield return (Image)GlobalPillow.currentFrame.Clone(); 
            }
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            Environment.Exit(0); 
        }
	}
}