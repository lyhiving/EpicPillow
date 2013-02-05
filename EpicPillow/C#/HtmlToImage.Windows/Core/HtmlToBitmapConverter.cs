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
		private const int SleepTimeMiliseconds = 69;
		public Uri navURL;
        public Size defSize = new Size(800,600); 
		public Size newSize = new Size(800,600); 
		public Size minSize = new Size(640, 480);
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
			pubbrowser.NewWindow += new CancelEventHandler(cancelWindow);
			minPix = minSize.Width * minSize.Height;
		}
		public void docClicked(object sender, HtmlElementEventArgs e)
		{
		}
		private void doneLoading(object sender, WebBrowserDocumentCompletedEventArgs e)
		{
		}
		string url = ""; 
		private void LinkClicked(object sender, EventArgs e)
		{
		}
		private void cancelWindow(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
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
			
            while (browser.ReadyState != WebBrowserReadyState.Complete)
			{
				Application.DoEvents();
			}
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
				width = Math.Max(body2.scrollWidth, root2.scrollWidth);
    			height = Math.Max(root2.scrollHeight, body2.scrollHeight);
    			browser.SetBounds(0, 0, width, height); 
			}
			newSize = new Size(width, height); 
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
            try
            {
                screenie = new Bitmap(size.Width, size.Height);

                NativeMethods.GetImage(GetDomDocument(), screenie, Color.White);
                return screenie;
            }
            catch (Exception ex)
            {
                screenie = new Bitmap(defSize.Width, defSize.Height);
                NativeMethods.GetImage(GetDomDocument(), screenie, Color.White);
                return screenie; 
            }
		}
        public bool newResize = false;
        public int resizeCount = 0;
        public int maxResize = 69;
        public Bitmap delegateScreenshot()
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
                    width = Math.Max(body2.scrollWidth, root2.scrollWidth);
                    height = Math.Max(root2.scrollHeight, body2.scrollHeight);
                    pubbrowser.SetBounds(0, 0, width, height);
                    newResize = true;
                }
                catch (Exception ex)
                {
                }
            return GetBitmapFromControl(pubbrowser, pubbrowser.Size); 
        }
        
        public void btnMouseClick_Click(int xMouse, int yMouse)
        {
            int x = xMouse;
            int y = yMouse; 
            IntPtr handle = pubbrowser.Handle;
            StringBuilder className = new StringBuilder(100);
            while (className.ToString() != "Internet Explorer_Server") 
            {
                handle = GetWindow(handle, 5); 
                GetClassName(handle, className, className.Capacity);
            }
            IntPtr lParam = (IntPtr)((y << 16) | x); 
            IntPtr wParam = IntPtr.Zero;
            const uint downCode = 0x201; 
            const uint upCode = 0x202; 
            SendMessage(Flash(), downCode, wParam, lParam);
            SendMessage(Flash(), upCode, wParam, lParam);
            SendMessage(handle, downCode, wParam, lParam); 
            SendMessage(handle, upCode, wParam, lParam); 
            
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
        }
        [DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
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
        public delegate object GetDomHandler(); 
        public object GetDomDocument()
        {
            if (pubbrowser.InvokeRequired)
            {
                return pubbrowser.Invoke(new GetDomHandler(GetDomDocument)) as object;

            }
            else
            {
                return pubbrowser.Document.DomDocument; 
            }
        }
	}
}