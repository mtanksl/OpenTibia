using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OpenTibia
{
    public class Listener : IDisposable
    {
        private readonly object sync = new object();

            private bool stop = false;

            private List<IClient> clients = new List<IClient>();

            private AutoResetEvent syncStop = new AutoResetEvent(false);
        
        private int port;

        private IClientFactory clientFactory;

        public Listener(int port, IClientFactory clientFactory)
        {
            this.port = port;

            this.clientFactory = clientFactory;
        }

        ~Listener()
        {
            Dispose(false);
        }

        private Socket socket;

        public void Start()
        {
            lock (sync)
            {
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp); socket.Bind(new IPEndPoint(IPAddress.Any, port) ); socket.Listen(0);

                socket.BeginAccept(Accept, null);
            }
        }

        private void Accept(IAsyncResult result)
        {
            lock (sync)
            {
                if (stop)
                {
                    syncStop.Set();
                }
                else
                {
                    try
                    {
                        IClient client = clientFactory.Create( socket.EndAccept(result) );

                        client.Disconnect += (sender, e) =>
                        {
                            lock (sync)
                            {
                                if ( !stop )
                                {
                                    clients.Remove(client);

                                    client.Dispose();
                                }
                            }
                        };

                        clients.Add(client);

                        client.Start();
                        
                        socket.BeginAccept(Accept, null);
                    }
                    catch (SocketException)
                    {
                        //Empty
                    }
                }
            }
        }

        public void Stop(bool wait = true)
        {
            lock (sync)
            {
                if (stop)
                {
                    wait = false;
                }
                else
                {
                    stop = true;

                    foreach (var client in clients)
                    {
                        client.Stop();
                    }

                    socket.Close();
                }
            }

            if (wait)
            {
                syncStop.WaitOne();
            }
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
                    foreach (var client in clients)
                    {
                        client.Dispose();
                    }

                    socket.Dispose();
                }
            }
        }
    }
}