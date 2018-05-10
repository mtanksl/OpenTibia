using OpenTibia.Threading;
using System;

namespace OpenTibia.Game
{
    public class Server : IDisposable
    {
        public Server()
        {
            Dispatcher = new Dispatcher();

            Scheduler = new Scheduler(Dispatcher);

            LoginListener = new Listener(7171, socket => new LoginConnection(this, socket) );

            GameListener = new Listener(7172, socket => new GameConnection(this, socket) );
        }

        ~Server()
        {
            Dispose(false);
        }

        public Dispatcher Dispatcher { get; set; }

        public Scheduler Scheduler { get; set; }

        public Listener LoginListener { get; set; }

        public Listener GameListener { get; set; }

        public void Start()
        {
            Dispatcher.Start();

            Scheduler.Start();

            LoginListener.Start();

            GameListener.Start();
        }

        public void Stop()
        {
            Dispatcher.Stop();

            Scheduler.Stop();

            LoginListener.Stop();

            GameListener.Stop();
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    LoginListener.Dispose();

                    GameListener.Dispose();
                }
            }
        }
    }
}