namespace HtmlToImage.Windows
{
	using System;
	using System.Drawing;
	using System.Windows.Forms;
	using EyeOpen.Imaging;

	public partial class MainForm : Form
	{
		public Size size = new Size(1920, 1080);

		public MainForm()
		{
			InitializeComponent();

			SetHtml();
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
			
			pictureBox.Image =
				new HtmlToBitmapConverter()
					.Render(new Uri(urlTextBox.Text), size);
			pictureBox.Image.Save("test.bmp");
			System.Diagnostics.Process.Start("test.bmp"); 
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
	}
}