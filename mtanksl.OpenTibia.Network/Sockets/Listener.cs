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

            private bool stopped = false;
        
            private AutoResetEvent syncStop = new AutoResetEvent(false);

        private Func<Socket, Connection> factory;

        public Listener(Func<Socket, Connection> factory)
        {
            this.factory = factory;
        }

        ~Listener()
        {
            Dispose(false);
        }

        private int maxConnections;

        private Socket socket;
        
        public void Start(int maxConnections, int port)
        {
            lock (sync)
            {
                this.maxConnections = maxConnections;

                this.socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                    this.socket.Bind(new IPEndPoint(IPAddress.Any, port) );

                    this.socket.Listen(0);

                this.socket.BeginAccept(Accept, null);
            }
        }

        private HashSet<Connection> connections = new HashSet<Connection>();

        private void Accept(IAsyncResult result)
        {
            lock (sync)
            {
                if (stopped)
                {
                    syncStop.Set();
                }
                else
                {
                    try
                    {
                        Connection connection = factory(socket.EndAccept(result) );

                        connection.Disconnected += (sender, e) =>
                        {
                            lock (sync)
                            {
                                if ( !stopped )
                                {
                                    connections.Remove(connection);

                                    connection.Dispose();

                                    if (connections.Count == maxConnections - 1)
                                    {
                                        socket.BeginAccept(Accept, null);
                                    }
                                }
                            }
                        };

                        connections.Add(connection);

                        connection.Start();

                        if (connections.Count < maxConnections)
                        {
                            socket.BeginAccept(Accept, null);
                        }
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
                if (stopped)
                {
                    wait = false;
                }
                else
                {
                    stopped = true;

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
                    if (connections != null)
                    {
                        foreach (var connection in connections)
                        {
                            connection.Dispose();
                        }
                    }

                    if (socket != null)
                    {
                        socket.Dispose();
                    }
                }
            }
        }
    }
}