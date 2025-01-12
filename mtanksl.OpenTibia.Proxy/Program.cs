using System;

namespace mtanksl.OpenTibia.Proxy
{
    class Program
    {
        static void Main(string[] args)
        {
            var logger = new Logger();

            using (var loginProxy = new Listener(7173, socket => new LoginProxyConnection(logger, socket, "127.0.0.1", 7171) ) )
            {
                using (var gameProxy = new Listener(7174, socket => new GameProxyConnection(logger, socket, "127.0.0.1", 7172) ) )
                {
                    loginProxy.Start();

                    gameProxy.Start();

                    logger.WriteLine("Proxy online");

                    bool exit = false;

                    while (!exit)
                    {
                        switch (Console.ReadLine() )
                        {
                            case "cls":

                                Console.Clear();

                                break;

                            case "":

                                loginProxy.Stop();

                                gameProxy.Stop();

                                logger.WriteLine("Proxy offline");

                                exit = true;

                                break;
                        }
                    }
                }
            }      
            
            Console.ReadKey();
        }
    }
}