namespace EyeOpen.Imaging
{
	using System;
	using System.Drawing;
	using System.Threading;
	using System.Windows.Forms;
	using mshtml;

	public class HtmlToBitmapConverter
	{
		public WebBrowser pubbrowser; 
		private const int SleepTimeMiliseconds = 69;
		public Size newSize = new Size(1920, 1080); 
		public Bitmap Render(string html, Size size)
		{
			pubbrowser = CreateBrowser(size);

			pubbrowser.Navigate("about:blank");
			pubbrowser.Document.Write(html);

			return GetBitmapFromControl(pubbrowser, newSize);
		}

		public Bitmap Render(Uri uri, Size size)
		{
			pubbrowser = CreateBrowser(size);

			NavigateAndWaitForLoad(pubbrowser, uri, 0);

			return GetBitmapFromControl(pubbrowser, newSize);
		}
        public void delegateNav(Uri link)
        {
            NavigateAndWaitForLoad(pubbrowser, link, SleepTimeMiliseconds); 
        }
		public void NavigateAndWaitForLoad(WebBrowser browser, Uri uri, int waitTime)
		{
			browser.Navigate(uri);
			var count = 0;

			while (browser.ReadyState != WebBrowserReadyState.Complete)
			{
				Thread.Sleep(SleepTimeMiliseconds);
				
				Application.DoEvents();
				count++;
				
				if (count > waitTime / SleepTimeMiliseconds)
				{
					break;
				}
			}
			
			//while (browser.Document.Body == null)
            while (browser.ReadyState != WebBrowserReadyState.Complete)
			{
				Application.DoEvents();
			}
			//browser.Size = new Size(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Height); 
			browser.SetBounds(0, 0, browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Height); 
			//MessageBox.Show(browser.Document.Body.ScrollRectangle.Width.ToString());
			//MessageBox.Show(browser.Document.Body.ScrollRectangle.Height.ToString());
			newSize = new Size(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Height); 
			HideScrollBars(browser);
		}

		private void HideScrollBars(WebBrowser browser)
		{
			const string Hidden = "hidden";
			var document = (IHTMLDocument2)browser.Document.DomDocument;
			var style = (IHTMLStyle2)document.body.style;
			style.overflowX = Hidden;
			style.overflowY = Hidden;
		}

		private WebBrowser CreateBrowser(Size size)
		{
			var 
				newBrowser =
					new WebBrowser
					{
						ScrollBarsEnabled = false,
						ScriptErrorsSuppressed = true,
						Size = size
					};

			newBrowser.BringToFront();

			return newBrowser;
		}
        Bitmap screenie; 
		private Bitmap GetBitmapFromControl(WebBrowser browser, Size size)
		{
            screenie = null; 
			screenie = new Bitmap(size.Width, size.Height);

			NativeMethods.GetImage(browser.Document.DomDocument, screenie, Color.White);
			return screenie;
		}
        public Bitmap delegateScreenshot()
        {
            return GetBitmapFromControl(pubbrowser, newSize); 
        }
	}
}