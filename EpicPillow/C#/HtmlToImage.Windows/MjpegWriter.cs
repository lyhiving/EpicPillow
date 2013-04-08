using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Net.Sockets;
using System.Drawing.Imaging;

// -------------------------------------------------
// Developed By : Ragheed Al-Tayeb
// e-Mail       : ragheedemail@gmail.com
// Date         : April 2012
// -------------------------------------------------

namespace rtaNetworking.Streaming
{

    /// <summary>
    /// Provides a stream writer that can be used to write images as MJPEG 
    /// or (Motion JPEG) to any stream.
    /// </summary>
    public class MjpegWriter:IDisposable 
    {
        
        private static byte[] CRLF = new byte[] { 13, 10 };
        private static byte[] EmptyLine = new byte[] { 13, 10, 13, 10};

        private string _Boundary;

        public MjpegWriter(Socket stream)
            : this(stream, "--boundary")
        {

        }

        public MjpegWriter(Socket stream,string boundary)
        {

            this.Stream = stream;
            this.Boundary = boundary;
        }

        public string Boundary { get; private set; }
        public Socket Stream { get; private set; }

        public void WriteHeader()
        {

            Write( 
                    "HTTP/1.0 200 OK\r\n" +
                    "Cache-Control: no-cache\r\n" +
                    "Pragma: no-cache\r\n" + 
                    "Expires: Thu, 01 Dec 1994 16:00:00 GMT\r\n" + 
                    "Connection: close\r\n" + 
                    "Content-Type: multipart/x-mixed-replace; boundary=" +
                    this.Boundary +
                    "\r\n"
                   //""
                 );

            //this.Stream.Flush();
       }
        public void minWriteHeader()
        {

            Write(
                    "HTTP/1.1 200 OK\r\n" + 
                    "Content-Type: multipart/x-mixed-replace\r\n"
                //""
                 );

            //this.Stream.Flush();
        }
        public void Write(Image image)
        {
            MemoryStream ms = BytesOf(image);
            this.Write(ms);
        }

        public void Write(MemoryStream imageStream, bool boundary = false)
        {

            StringBuilder sb = new StringBuilder();

            //sb.AppendLine(); 
            if (boundary)
            {
                sb.AppendLine();
                sb.AppendLine(this.Boundary);
            }
            if (boundary == false)
            {
                sb.AppendLine("Date: " + DateTime.Now.ToUniversalTime().ToString("r"));
            }
            sb.AppendLine("Content-Type: image/jpeg"); 
            sb.AppendLine("Content-Length: " + imageStream.Length.ToString());
            
            sb.AppendLine();
            Write(sb.ToString());
            imageStream.WriteTo(new NetworkStream(this.Stream));
            //Write(ImageToByte((Image)HtmlToImage.Windows.GlobalPillow.currentFrame.Clone())); 
            Write("\r\n");
            
            //this.Stream.Flush();

        }
        public void EndSock()
        {
            this.Stream.Close(); 
        }
        public void writeImg(Image image)
        {
            MemoryStream ms = BytesOf(image, 10);
            Write(ms); 
        }
        byte[] ImageToByte(Image img)
        {
            ImageConverter converter = new ImageConverter();
            return (byte[])converter.ConvertTo(img, typeof(byte[]));
        }
        private void Write(byte[] data)
        {
            //this.Stream.Send(data, data.Length);
            this.Stream.Send(data); 
            System.Diagnostics.Debug.Write(data.Length.ToString()); 
        }

        private void Write(string text)
        {
            byte[] data = BytesOf(text);
            //this.Stream.Write(data, 0, data.Length);
            this.Stream.Send(data); 
            System.Diagnostics.Debug.Write(text); 
        }

        private static byte[] BytesOf(string text)
        {
            return Encoding.ASCII.GetBytes(text);
        }
        //from MrPolite in VBForums
        public static MemoryStream SaveJpeg(MemoryStream path, Image img, int quality)
        {
            if (quality < 0 || quality > 100)
                throw new ArgumentOutOfRangeException("quality must be between 0 and 100.");


            // Encoder parameter for image quality 
            EncoderParameter qualityParam =
                new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);
            // Jpeg image codec 
            ImageCodecInfo jpegCodec = GetEncoderInfo("image/jpeg");

            EncoderParameters encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = qualityParam;

            img.Save(path, jpegCodec, encoderParams);
            return path; 
        }

        /// <summary> 
        /// Returns the image codec with the given mime type 
        /// </summary> 
        private static ImageCodecInfo GetEncoderInfo(string mimeType)
        {
            // Get image codecs for all image formats 
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

            // Find the correct image codec 
            for (int i = 0; i < codecs.Length; i++)
                if (codecs[i].MimeType == mimeType)
                    return codecs[i];
            return null;
        }  
        private static MemoryStream BytesOf(Image image, int compress = 0)
        {
            if (compress != 0)
            {
                MemoryStream ms = new MemoryStream();
                SaveJpeg(ms, image, compress);
                return ms; 
            }
            else
            {
                MemoryStream ms = new MemoryStream();
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
                return ms;
            }
        }
        public string ReadRequest(int length)
        {

            byte[] data = new byte[length];
            int count = this.Stream.Receive(data);
            
            if (count != 0)
                return Encoding.ASCII.GetString(data, 0, count);

            return null;
        }

        #region IDisposable Members

        public void Dispose()
        {

            try
            {

                if (this.Stream != null)
                    this.Stream.Dispose();

            }
            finally
            {
                this.Stream = null;
            }
        }

        #endregion
    }
}
