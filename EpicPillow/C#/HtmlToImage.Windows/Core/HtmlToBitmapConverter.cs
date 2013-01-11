namespace EyeOpen.Imaging
{
	using System;
	using System.Drawing;
	using System.Threading;
	using System.Windows.Forms;
	using System.Text;
	using System.ComponentModel;
	using System.Runtime.InteropServices; 
	using mshtml;

	public class HtmlToBitmapConverter
	{

		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
 
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
 
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
 
		public WebBrowser pubbrowser; 
		//public HtmlToImage.Windows.MainForm mainFrm = new HtmlToImage.Windows.MainForm(); 
		private const int SleepTimeMiliseconds = 69;
		public Uri navURL; 
		public Size newSize = new Size(1920, 1080); 
		public Bitmap Render(string html, Size size)
		{
			pubbrowser = CreateBrowser(size);

			pubbrowser.Navigate("about:blank");
			pubbrowser.Document.Write(html);

			return GetBitmapFromControl(pubbrowser, newSize);
		}
		public void startConverter()
		{
			pubbrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(doneLoading);
			pubbrowser.NewWindow += new CancelEventHandler(cancelWindow);
			pubbrowser.Document.Click += new HtmlElementEventHandler(docClicked);
		}
		public void docClicked(object sender, HtmlElementEventArgs e)
		{
			
		}
		private void doneLoading(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
			//mainFrm.urlTextBox.Text = pubbrowser.Url.ToString(); 
			HtmlElementCollection links = pubbrowser.Document.Links;
			foreach (HtmlElement var in links)
			{
				var.AttachEventHandler("onclick", LinkClicked);
			}
		}
		string url = ""; 
		private void LinkClicked(object sender, EventArgs e)
		{
		
		HtmlElement link = pubbrowser.Document.ActiveElement;
		url = link.GetAttribute("href");
		
		}
		private void cancelWindow(object sender, CancelEventArgs e)
		{
			//MessageBox.Show(pubbrowser.StatusText); 
			
			e.Cancel = true;
			//delegateNav(new Uri(url)); 
			//delegateNav(navURL);
			//MessageBox.Show(e.ToString()); 
			//delegateNav(new Uri(e.ToString()));
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
			navURL = uri; 
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
        
        public void btnMouseClick_Click(int xMouse, int yMouse)
        {
            //int x = 100; // X coordinate of the click
            //int y = 80; // Y coordinate of the click
            int x = xMouse;
            int y = yMouse; 
            IntPtr handle = pubbrowser.Handle;
            StringBuilder className = new StringBuilder(100);
            while (className.ToString() != "Internet Explorer_Server") // The class control for the browser
            {
                handle = GetWindow(handle, 5); // Get a handle to the child window
                GetClassName(handle, className, className.Capacity);
            }
 
            IntPtr lParam = (IntPtr)((y << 16) | x); // The coordinates
            IntPtr wParam = IntPtr.Zero; // Additional parameters for the click (e.g. Ctrl)
            const uint downCode = 0x201; // Left click down code
            const uint upCode = 0x202; // Left click up code
            SendMessage(handle, downCode, wParam, lParam); // Mouse button down
            SendMessage(handle, upCode, wParam, lParam); // Mouse button up
        }
        
	}
}