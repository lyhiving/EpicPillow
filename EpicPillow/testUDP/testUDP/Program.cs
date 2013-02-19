using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading; 
namespace UdpClient
{
    class Program
    {
        static void Main(string[] args)
        {
            System.Net.Sockets.UdpClient sock = new System.Net.Sockets.UdpClient();
            IPEndPoint iep = new IPEndPoint(IPAddress.Parse("192.168.0.2"), 1261);
            while (true)
            {
                byte[] data = Encoding.ASCII.GetBytes(Console.ReadLine());
                sock.Send(data, data.Length, iep);
                //sock.Close();
                int[] bytesAsInts = Array.ConvertAll(data, c => (int)c);
                string test = "";
                foreach (int i in bytesAsInts)
                {
                    Console.WriteLine(i.ToString()); 
                    test += ((char)i).ToString(); 
                }
                Console.WriteLine(test); 
                Console.WriteLine("Message sent.");
            }
        }
    }
}