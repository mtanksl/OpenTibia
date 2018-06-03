using OpenTibia.Game;
using System;

namespace mtanksl.OpenTibia.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var server = new Server() )
            {
                server.Start();

                Console.ReadKey();
                
                server.Stop();
            }

            Console.ReadKey();
        }
    }
}