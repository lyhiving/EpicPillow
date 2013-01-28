namespace EyeOpen.Imaging
{
	using System;
	using System.Drawing;
	using System.Threading;
	using System.Windows.Forms;
	using System.Text;
	using System.ComponentModel;
	using System.Runtime.InteropServices; 
	using MSHTML;
    using System.Collections.Generic; 

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
        public Size defSize = new Size(1366,768); 
		public Size newSize = new Size(1366,768); 
		public Size minSize = new Size(640, 480);
        public List<IntPtr> Handlez = new List<IntPtr>(); 
		public int minPix; 
		public Bitmap Render(string html, Size size)
		{
			pubbrowser = CreateBrowser(size);

			pubbrowser.Navigate("about:blank");
			pubbrowser.Document.Write(html);

			return GetBitmapFromControl(pubbrowser, newSize);
		}
		public void startConverter()
		{
            //defSize = Screen.PrimaryScreen.Bounds.Size;
            //newSize = Screen.PrimaryScreen.Bounds.Size; 
            
			//pubbrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(doneLoading);
			pubbrowser.NewWindow += new CancelEventHandler(cancelWindow);
			//pubbrowser.Document.Click += new HtmlElementEventHandler(docClicked);
			minPix = minSize.Width * minSize.Height;
            //Handlez.Add(Flash());
            //Handlez.Add(pubbrowser.Handle); 
		}
		public void docClicked(object sender, HtmlElementEventArgs e)
		{
			
		}
		private void doneLoading(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
            /*
			//mainFrm.urlTextBox.Text = pubbrowser.Url.ToString(); 
			HtmlElementCollection links = pubbrowser.Document.Links;
			foreach (HtmlElement var in links)
			{
				var.AttachEventHandler("onclick", LinkClicked);
			}
            */
		}
		string url = ""; 
		private void LinkClicked(object sender, EventArgs e)
		{
		/*
		HtmlElement link = pubbrowser.Document.ActiveElement;
		url = link.GetAttribute("href");
		*/
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
		public void NavigateAndWaitForLoad(WebBrowser browser, Uri uri, int waitTime, bool resize = true)
		{
            newResize = false; 
            browser.Size = defSize; 
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
			//browser.Document.Body.ClientRectangle.Size = defSize; 
			//browser.Size = defSize; 
			//while (browser.Document.Body == null)
            while (browser.ReadyState != WebBrowserReadyState.Complete)
			{
				Application.DoEvents();
			}
			//browser.Size = new Size(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Height); 
			int width = defSize.Width;
			int height = defSize.Height; 
			if (resize)
			{
				var doc2 = (IHTMLDocument2)browser.Document.DomDocument;
				var doc3 = (IHTMLDocument3)browser.Document.DomDocument; 
				var body2 = (IHTMLElement2)doc2.body;
				var root2 = (IHTMLElement2)doc3.documentElement; 
    			width = Math.Max(body2.scrollWidth, root2.scrollWidth);
    			height = Math.Max(root2.scrollHeight, body2.scrollHeight);
				browser.SetBounds(0, 0, width, height); 
				//browser.SetBounds(0, 0, browser.Document.Body.ClientRectangle.Size.Width, browser.Document.Body.ClientRectangle.Size.Height); 
				width = Math.Max(body2.scrollWidth, root2.scrollWidth);
    			height = Math.Max(root2.scrollHeight, body2.scrollHeight);
    			browser.SetBounds(0, 0, width, height); 
			}
			//MessageBox.Show(browser.Document.Body.ScrollRectangle.Width.ToString());
			//MessageBox.Show(browser.Document.Body.ScrollRectangle.Height.ToString());
			//newSize = new Size(browser.Document.Body.ScrollRectangle.Width, browser.Document.Body.ScrollRectangle.Height); 
			newSize = new Size(width, height); 
			//HideScrollBars(browser);
            resizeCount = 0; 
		}

		private void HideScrollBars(WebBrowser browser)
		{
			const string Hidden = "hidden";
			var document = (IHTMLDocument2)browser.Document.DomDocument;
			var style = (IHTMLStyle2)document.body.style;
			style.overflowX = Hidden;
			style.overflowY = Hidden;
            int width;
            int height; 
            var doc2 = (IHTMLDocument2)browser.Document.DomDocument;
            var doc3 = (IHTMLDocument3)browser.Document.DomDocument;
            var body2 = (IHTMLElement2)doc2.body;
            var root2 = (IHTMLElement2)doc3.documentElement;
            width = Math.Max(body2.scrollWidth, root2.scrollWidth);
            height = Math.Max(root2.scrollHeight, body2.scrollHeight);
            browser.SetBounds(0, 0, width, height);
            //browser.SetBounds(0, 0, browser.Document.Body.ClientRectangle.Size.Width, browser.Document.Body.ClientRectangle.Size.Height); 
            width = Math.Max(body2.scrollWidth, root2.scrollWidth);
            height = Math.Max(root2.scrollHeight, body2.scrollHeight);
            browser.SetBounds(0, 0, width, height); 
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
        public bool newResize = false;
        public int resizeCount = 0;
        public int maxResize = 14;
        public Bitmap delegateScreenshot()
        {
            //if (newResize == false)
            //{
            if (resizeCount < maxResize)
            {
                try
                {
                    resizeCount++;
                    int width;
                    int height;
                    var doc2 = (IHTMLDocument2)pubbrowser.Document.DomDocument;
                    var doc3 = (IHTMLDocument3)pubbrowser.Document.DomDocument;
                    var body2 = (IHTMLElement2)doc2.body;
                    var root2 = (IHTMLElement2)doc3.documentElement;
                    width = Math.Max(body2.scrollWidth, root2.scrollWidth);
                    height = Math.Max(root2.scrollHeight, body2.scrollHeight);
                    pubbrowser.SetBounds(0, 0, width, height);
                    //browser.SetBounds(0, 0, browser.Document.Body.ClientRectangle.Size.Width, browser.Document.Body.ClientRectangle.Size.Height); 
                    width = Math.Max(body2.scrollWidth, root2.scrollWidth);
                    height = Math.Max(root2.scrollHeight, body2.scrollHeight);
                    pubbrowser.SetBounds(0, 0, width, height);
                    newResize = true;
                }
                catch (Exception ex)
                {

                }
            }
            //}
            return GetBitmapFromControl(pubbrowser, pubbrowser.Size); 
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
            //while (className.ToString() != "TabWindowClass") 
            {
                
                handle = GetWindow(handle, 5); // Get a handle to the child window
                GetClassName(handle, className, className.Capacity);
            }
            IntPtr lParam = (IntPtr)((y << 16) | x); // The coordinates
            IntPtr wParam = IntPtr.Zero; // Additional parameters for the click (e.g. Ctrl)
            const uint downCode = 0x201; // Left click down code
            const uint upCode = 0x202; // Left click up code
            SendMessage(Flash(), downCode, wParam, lParam);
            SendMessage(Flash(), upCode, wParam, lParam);
            SendMessage(handle, downCode, wParam, lParam); // Mouse button down
            SendMessage(handle, upCode, wParam, lParam); // Mouse button up
            
        }
        
        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr FindWindowEx(IntPtr parentHandle, IntPtr childAfter, string className, IntPtr windowTitle);
        public IntPtr Flash()
        {
            IntPtr pControl;
            pControl = FindWindowEx(pubbrowser.Handle, IntPtr.Zero, "Shell Embedding", IntPtr.Zero);
            pControl = FindWindowEx(pControl, IntPtr.Zero, "Shell DocObject View", IntPtr.Zero);
            pControl = FindWindowEx(pControl, IntPtr.Zero, "Internet Explorer_Server", IntPtr.Zero);
            pControl = FindWindowEx(pControl, IntPtr.Zero, "MacromediaFlashPlayerActiveX", IntPtr.Zero);
            return pControl;
        }
        [DllImport("user32.dll", SetLastError = true)]
        static extern bool PostMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
        public enum WMessages : int
        {
            WM_LBUTTONDOWN = 0x201,
            WM_LBUTTONUP = 0x202
        }
        private int MAKELPARAM(int p, int p_2)
        {
            return ((p_2 << 16) | (p & 0xFFFF));
        }
        public void DoMouseLeftClick(Point x)
        {
            
            
            btnMouseClick_Click(x.X, x.Y);
            //PostMessage(Flash(), (uint)WMessages.WM_LBUTTONDOWN, 0, MAKELPARAM(x.X, x.Y));
            //PostMessage(Flash(), (uint)WMessages.WM_LBUTTONUP, 0, MAKELPARAM(x.X, x.Y));
            //SetForegroundWindow(pubbrowser.Handle); 
        }
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName,
            string lpWindowName);

        // Activate an application window.
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        public void keyboardSend(Keys stroke)
        {
            KeyDown(stroke);
            KeyUp(stroke); 
        }
        public static void KeyDown(Keys key)
        {
            keybd_event((byte)key, 0, 0, 0); 
        }
        public static void KeyUp(Keys key)
        {
            keybd_event((byte)key, 0, 0x7F, 0); 
        }
	}
}