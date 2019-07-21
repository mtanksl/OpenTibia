using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OpenTibia.Network.Sockets
{
    public class Listener : IDisposable
    {
        private readonly object sync = new object();

            private bool stop = false;
        
            private AutoResetEvent syncStop = new AutoResetEvent(false);

        private int port;

        private Func<Socket, Connection> factory;

        public Listener(int port, Func<Socket, Connection> factory)
        {
            this.port = port;

            this.factory = factory;
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
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    socket.Bind(new IPEndPoint(IPAddress.Any, port) );

                    socket.Listen(0);


                socket.BeginAccept(Accept, null);
            }
        }

        private List<Connection> connections = new List<Connection>();

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
                        Connection connection = factory( socket.EndAccept(result) );

                        connection.Disconnected += (sender, e) =>
                        {
                            lock (sync)
                            {
                                if ( !stop )
                                {
                                    connections.Remove(connection);

                                    connection.Dispose();
                                }
                            }
                        };

                        connections.Add(connection);

                        connection.Start();
                        

                        socket.BeginAccept(Accept, null);
                    }
                    catch (SocketException)
                    {
                        //
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

                    foreach (var connection in connections)
                    {
                        connection.Stop();
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
                    foreach (var connection in connections)
                    {
                        connection.Dispose();
                    }

                    socket.Dispose();
                }
            }
        }
    }
}