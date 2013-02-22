using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Threading;
using System.Drawing;
using System.Net.Sockets; 
namespace HtmlToImage.Windows.View
{
    class PillowServer
    {
        HttpListener listener = new HttpListener();
        public int serverPort = 1263;
        public Bitmap imageSrc = null; 
        public PillowServer()
        {
            
        }
        public void startPillow()
        {
            Thread t = new Thread(Listen);
            t.Start(); 
        }
        void Listen()
        {
            Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Server.Bind(new IPEndPoint(IPAddress.Any, serverPort));
            Server.Listen(10);

            System.Diagnostics.Debug.WriteLine(string.Format("Server started on port {0}.", serverPort));
            /*
            while (true)
            {
                Socket s = Server.Accept();
                ParameterizedThreadStart start = new ParameterizedThreadStart(Handle);
                Thread t = new Thread(start);
                t.Start(s);
            }
             * */
            foreach (Socket client in Connections(Server))
            {
                //ThreadPool.QueueUserWorkItem(new WaitCallback(ClientThread), client);
                
                Handle(client); 
            }
        }
        public static IEnumerable<Socket> Connections(Socket server)
        {
            while (true)
                yield return server.Accept();
        }
        void Handle(object clientSock)
        {
            /*
            byte[] buffer = ImageToByte(imageSrc);
            HttpListenerContext c = (HttpListenerContext)context; 
            c.Response.OutputStream.Write(buffer, 0, buffer.Length);
            c.Response.OutputStream.Close(); 
             * */
            Socket client = (Socket)clientSock;
            NetworkStream ns = new NetworkStream(client, true); 
            using (rtaNetworking.Streaming.MjpegWriter wr = new rtaNetworking.Streaming.MjpegWriter(ns))
            {
                //try
                //{
                    wr.minWriteHeader();
                    wr.writeImg((Image)GlobalPillow.currentFrame.Clone());
                //}
                //catch (Exception ex)
                //{

                //}
            }
            ns.Close();
            client.Close(); 
            //Thread.CurrentThread.Join(); 
        }
        byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
    }
}
