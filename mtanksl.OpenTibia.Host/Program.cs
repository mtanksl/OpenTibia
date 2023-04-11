using OpenTibia.Game;
using System;

namespace mtanksl.OpenTibia.Host2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            using (var server = new Server(7171, 7172) )
            {
                server.Start();

                Console.ReadKey();

                server.KickAll();

                server.Stop();
            }

            Console.ReadKey();
        }
    }
}