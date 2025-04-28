using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace WatsUpServer
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(Dns.GetHostAddresses(Dns.GetHostName())[0], 45989);
            listener.Start();

            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                new Thread(new Server(client).Start).Start();
            }
        }
    }
}
