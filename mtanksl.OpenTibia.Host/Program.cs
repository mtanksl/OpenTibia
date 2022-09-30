using OpenTibia.Game;
using System;

namespace mtanksl.OpenTibia.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new Server("127.0.0.1", 7171, 7172) )
            {
                server.Start();

                Console.ReadKey();

                server.Stop();
            }

            Console.ReadKey();
        }
    }
}