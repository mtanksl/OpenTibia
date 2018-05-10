using OpenTibia.Game;
using System;

namespace mtanksl.OpenTibia.Host
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Server() )
            {
                game.Start();

                Console.ReadKey();

                game.Stop();
            }
        }
    }
}