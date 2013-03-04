
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using MjpegProcessor; 

namespace EpicCow_Client
{
	/// <summary>
	/// Description of MainForm.
	/// </summary>
	public partial class MainForm : Form
	{
        MjpegDecoder _mjpeg; 
		public MainForm()
		{
			//
			// The InitializeComponent() call is required for Windows Forms designer support.
			//
			InitializeComponent();
			
			//
			// TODO: Add constructor code after the InitializeComponent() call.
			//
            _mjpeg = new MjpegDecoder();
            _mjpeg.FrameReady += mjpeg_FrameReady; 
		}

        private void MainForm_Load(object sender, EventArgs e)
        {
            
        }
        private void mjpeg_FrameReady(object sender, FrameReadyEventArgs e)
        {
            pictureBox1.Image = e.Bitmap; 
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _mjpeg.ParseStream(new Uri("http://192.168.0.2:8080")); 
        }
	}
}
