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
    using System.IO; 

	public class HtmlToBitmapConverter
	{
		[DllImport("user32.dll", CharSet = CharSet.Auto, SetLastError = false)]
        static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, IntPtr wParam, IntPtr lParam);
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr GetWindow(IntPtr hWnd, uint uCmd);
        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        static extern int GetClassName(IntPtr hWnd, StringBuilder lpClassName, int nMaxCount);
        //[DllImport("user32.dll", EntryPoint = "FindWindowEx")]
        //public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);
		public WebBrowser pubbrowser; 
		public Uri navURL;
        public Size defSize = new Size(1366,768); 
		public Size newSize = new Size(1366,768); 
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

			NavigateAndWaitForLoad(pubbrowser, uri);

			return GetBitmapFromControl(pubbrowser, newSize);
		}
        public void delegateNav(Uri link)
        {
            NavigateAndWaitForLoad(pubbrowser, link); 
        }
        
		public void NavigateAndWaitForLoad(WebBrowser browser, Uri uri)
		{

            threadbrowseSize(pubbrowser, defSize);
			navURL = uri;
            threadbrowseNav(pubbrowser, navURL);
            browserReady = false; 
            while (browserReady != true)
			{
                threadbrowseReady(browser);
                //MessageBox.Show(browserReady.ToString()); 
                //Thread.Sleep(100); 
				Application.DoEvents();
			}
            browseResize();  
            resizeCount = 0; 
		}
        void Send(string message, IntPtr winHandle)
        {
            SendMessage(winHandle, 0x000C, 0, message);
        }
        [DllImport("user32.dll", SetLastError = true)]  
	    static extern IntPtr FindWindow(string lpClassName, string lpWindowName); 
	    [DllImport("user32.dll", SetLastError = true)]  
	    static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);  
	 
	    //[DllImport("user32.dll")]  
	    //private static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, Int32 wParam, Int32 lParam); 
        bool browserReady = false;
        const int WM_KEYDOWN = 0x100;
        void SendStrokes(int key)
        {
            PostMessage(Flash(), WM_KEYDOWN, key, 0);
            PostMessage(IEHandle(), WM_KEYDOWN, key, 0); 
        }
        public void threadbrowseReady(WebBrowser tBrowser)
        {
            if (tBrowser.InvokeRequired)
            {
                tBrowser.Invoke(new MethodInvoker(delegate()
                {
                    if (tBrowser.ReadyState != WebBrowserReadyState.Complete)
                    {
                        browserReady = false; 
                    }
                    else
                    {
                        browserReady = true; 
                    }
                    //MessageBox.Show(browserReady.ToString()); 
                }));
            }
            else
            {
                if (tBrowser.ReadyState != WebBrowserReadyState.Complete)
                {
                    browserReady = false;
                }
                else
                {
                    browserReady = true;
                }
            }
        }
        public void threadbrowseSize(WebBrowser tBrowser, Size defSize)
        {
            if (tBrowser.InvokeRequired)
            {
                tBrowser.Invoke(new MethodInvoker(delegate()
                {
                    tBrowser.Size = defSize; 
                }));
            }
            else
            {
                tBrowser.Size = defSize; 
            }
        }
        public void threadbrowseNav(WebBrowser tBrowser, Uri uri)
        {
            if (tBrowser.InvokeRequired)
            {
                tBrowser.Invoke(new MethodInvoker(delegate()
                {
                    tBrowser.Navigate(uri); 
                }));
            }
            else
            {
                tBrowser.Navigate(uri); 
            }
        }
		private void HideScrollBars(WebBrowser browser)
		{
			const string Hidden = "hidden";
			var document = (IHTMLDocument2)browser.Document.DomDocument;
			var style = (IHTMLStyle2)document.body.style;
			style.overflowX = Hidden;
			style.overflowY = Hidden;
            browseResize(); 
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
                    //threadbrowseReady(pubbrowser);
                    if (browserReady)
                    {
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
                        //browseResize();
                    }
                }
                catch (Exception ex)
                {
                }
            return GetBitmapFromControl(pubbrowser, pubbrowser.Size); 
        }
        int width = 0;
        int height = 0;
        
        public void browseResize()
        {
            try
            {
                if (pubbrowser.InvokeRequired)
                {
                    pubbrowser.Invoke(new MethodInvoker( delegate()
                    {
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
                        
                    }));
                }
                else
                {
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

                }
            }
            catch (Exception ex)
            {

            }
        }
        IntPtr browseHandle;
        public void threadbrowseHandle(Control tBrowser)
        {
            if (tBrowser.InvokeRequired)
            {
                tBrowser.Invoke(new MethodInvoker(delegate()
                {
                    browseHandle = tBrowser.Handle; 
                }));
            }
            else
            {
                browseHandle = tBrowser.Handle; 
            }
        }
        public void btnMouseClick_Click(int xMouse, int yMouse)
        {
            int x = xMouse;
            int y = yMouse; 
            //IntPtr handle = pubbrowser.Handle;
            threadbrowseHandle(pubbrowser);
            IntPtr handle = browseHandle; 
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
            threadbrowseHandle(pubbrowser); 
            IntPtr pControl;
            pControl = FindWindowEx(browseHandle, IntPtr.Zero, "Shell Embedding", IntPtr.Zero);
            pControl = FindWindowEx(pControl, IntPtr.Zero, "Shell DocObject View", IntPtr.Zero);
            pControl = FindWindowEx(pControl, IntPtr.Zero, "Internet Explorer_Server", IntPtr.Zero);
            pControl = FindWindowEx(pControl, IntPtr.Zero, "MacromediaFlashPlayerActiveX", IntPtr.Zero);
            return pControl;
        }
        public IntPtr IEHandle()
        {
            threadbrowseHandle(pubbrowser);
            IntPtr handle = browseHandle;
            StringBuilder className = new StringBuilder(100);
            while (className.ToString() != "Internet Explorer_Server")
            {
                handle = GetWindow(handle, 5);
                GetClassName(handle, className, className.Capacity);
            }
            return handle; 
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
        //[DllImport("USER32.DLL", CharSet = CharSet.Unicode)]
        //public static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("USER32.DLL")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);
        [DllImport("user32.dll")]
        static extern uint keybd_event(byte bVk, byte bScan, int dwFlags, int dwExtraInfo);
        [DllImport("user32.dll")]
        static extern IntPtr GetForegroundWindow();
        [DllImport("user32.dll")]
        static extern int GetWindowText(IntPtr hWnd, StringBuilder text, int count);
        KeysConverter kc = new KeysConverter(); 
        public void keyboardSend(int stroke)
        {
            //threadbrowseHandle(pubbrowser);
            //IntPtr ptrFF = browseHandle;
            //SetForegroundWindow(ptrFF);
            //SendKeys.SendWait(stroke); 
            //KeyDown(stroke);
            //KeyUp(stroke); 
            /*
            threadbrowseHandle(pubbrowser);
            IntPtr handle = browseHandle;
            StringBuilder className = new StringBuilder(100);
            while (className.ToString() != "Internet Explorer_Server")
            {
                handle = GetWindow(handle, 5);
                GetClassName(handle, className, className.Capacity);
            }
            */
            /*
            PostMessage(Flash(), (byte)stroke, 0, 0);
            PostMessage(Flash(), (byte)stroke, 0x7F, 0);
            PostMessage(handle, (byte)stroke, 0, 0);
            PostMessage(handle, (byte)stroke, 0x7F, 0);
             * 
             * */
            
            SendStrokes(stroke);
        }
        private string GetActiveWindowTitle()
        {
            const int nChars = 256;
            IntPtr handle = IntPtr.Zero;
            StringBuilder Buff = new StringBuilder(nChars);
            handle = GetForegroundWindow();

            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
        }
        private string HandleToString(IntPtr handle)
        {
            const int nChars = 256;
            StringBuilder Buff = new StringBuilder(nChars);
            if (GetWindowText(handle, Buff, nChars) > 0)
            {
                return Buff.ToString();
            }
            return null;
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