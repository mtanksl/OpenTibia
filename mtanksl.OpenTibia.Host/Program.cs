using OpenTibia.Game;
using System;
using System.IO;

namespace mtanksl.OpenTibia.Host2
{
    internal class Program
    {
        static void Main(string[] args)
        {
            if ( !File.Exists("data\\database.db") )
            {
                File.Copy("data\\template.db", "data\\database.db");
            }

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