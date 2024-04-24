using System;
using System.Net.Sockets;
using System.Text;

namespace mtanksl.OpenTibia.InfoProtocol
{
    class Program
    {
        static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            socket.Connect("127.0.0.1", 7173);

            byte[] request = new byte[]
            {
                0x06, 0x00,             // Length

                0xFF,                   // Identifier

                0xFF,                   // Body

                0x69, 0x6E, 0x66, 0x6F
            };

            socket.Send(request);

            byte[] response = new byte[1024];

            int received = socket.Receive(response);

            Console.WriteLine(Encoding.Latin1.GetString(response, 0, received) );

            socket.Close();
        }
    }
}