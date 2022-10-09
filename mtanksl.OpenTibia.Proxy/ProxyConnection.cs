using System;
using System.Net.Sockets;
using System.Threading;

namespace OpenTibia.Proxy
{
    public abstract class ProxyConnection : IDisposable
    {
        private readonly object sync = new object();

            private bool stopped = false;
        
            private AutoResetEvent syncStop = new AutoResetEvent(false);

        private Socket client;

        private Socket server;

        private string host;

        private int port;

        public ProxyConnection(Socket socket, string host, int port)
        {
            this.client = socket;

            this.server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            this.host = host;

            this.port = port;
        }

        ~ProxyConnection()
        {
            Dispose(false);
        }

        public uint[] Keys { get; set; }

        public void Start()
        {
            OnConnectedFromClient();

            lock (sync)
            {
                if (!stopped)
                {
                    byte[] header = new byte[2];

                    client.BeginReceive(header, 0, header.Length, SocketFlags.None, ReceiveHeaderFromClient, header);

                    byte[] header2 = new byte[2];

                    server.BeginReceive(header2, 0, header2.Length, SocketFlags.None, ReceiveHeaderFromServer, header2);
                }
            }
        }

        private void ReceiveHeaderFromClient(IAsyncResult result)
        {
            lock (sync)
            {
                if (!stopped)
                {
                    try
                    {
                        byte[] header = result.AsyncState as byte[];
                
                        if (header.Length == client.EndReceive(result) )
                        {
                            OnReceivedHeaderFromClient(header);

                            byte[] body = new byte[ header[1] << 8 | header[0] ];

                            client.BeginReceive(body, 0, body.Length, SocketFlags.None, ReceiveBodyFromClient, body);
                        }
                        else
                        {
                            OnDisconnectedFromClient(new DisconnectedEventArgs(DisconnetionType.SocketClosed) );
                        }
                    }
                    catch (SocketException)
                    {
                        OnDisconnectedFromClient(new DisconnectedEventArgs(DisconnetionType.SocketException) );
                    }
                }
            }
        }

        private void ReceiveBodyFromClient(IAsyncResult result)
        {
            lock (sync)
            {
                if (!stopped)
                {
                    try
                    {
                        byte[] body = result.AsyncState as byte[];
                 
                        if (body.Length == client.EndReceive(result) )
                        {
                            OnReceivedBodyFromClient(body);

                            byte[] header = new byte[2];

                            client.BeginReceive(header, 0, header.Length, SocketFlags.None, ReceiveHeaderFromClient, header);
                        }
                        else
                        {
                            OnDisconnectedFromClient(new DisconnectedEventArgs(DisconnetionType.SocketClosed) );
                        }
                    }
                    catch (SocketException)
                    {
                        OnDisconnectedFromClient(new DisconnectedEventArgs(DisconnetionType.SocketException) );
                    }
                }
            }
        }
        
        private void ReceiveHeaderFromServer(IAsyncResult result)
        {
            lock (sync)
            {
                if (!stopped)
                {
                    try
                    {
                        byte[] header = result.AsyncState as byte[];

                        if (header.Length == server.EndReceive(result) )
                        {
                            OnReceivedHeaderFromServer(header);

                            byte[] body = new byte[ header[1] << 8 | header[0] ];

                            server.BeginReceive(body, 0, body.Length, SocketFlags.None, ReceiveBodyFromServer, body);
                        }
                        else
                        {
                            OnDisconnectedFromServer(new DisconnectedEventArgs(DisconnetionType.SocketClosed) );
                        }
                    }
                    catch (SocketException)
                    {
                        OnDisconnectedFromServer(new DisconnectedEventArgs(DisconnetionType.SocketException) );
                    }
                }
            }
        }

        private void ReceiveBodyFromServer(IAsyncResult result)
        {
            lock (sync)
            {
                if (!stopped)
                {
                    try
                    {
                        byte[] body = result.AsyncState as byte[];
                 
                        if (body.Length == server.EndReceive(result) )
                        {
                            OnReceivedBodyFromServer(body);

                            byte[] header = new byte[2];

                            server.BeginReceive(header, 0, header.Length, SocketFlags.None, ReceiveHeaderFromServer, header);
                        }
                        else
                        {
                            OnDisconnectedFromServer(new DisconnectedEventArgs(DisconnetionType.SocketClosed) );
                        }
                    }
                    catch (SocketException)
                    {
                        OnDisconnectedFromServer(new DisconnectedEventArgs(DisconnetionType.SocketException) );
                    }
                }
            }
        }

        protected virtual void OnConnectedFromClient() 
        {
            server.Connect(host, port);
        }

        protected virtual void OnReceivedHeaderFromClient(byte[] header) 
        {
            server.Send(header);
        }

        protected virtual void OnReceivedBodyFromClient(byte[] body) 
        {
            server.Send(body);
        }

        protected virtual void OnDisconnectedFromClient(DisconnectedEventArgs e)
        { 
            server.Disconnect(false);
        }

        protected virtual void OnReceivedHeaderFromServer(byte[] header) 
        {
            client.Send(header);
        }

        protected virtual void OnReceivedBodyFromServer(byte[] body) 
        {
            client.Send(body);
        }

        public event EventHandler<DisconnectedEventArgs> Disconnected;

        protected virtual void OnDisconnectedFromServer(DisconnectedEventArgs e)
        {
            client.Disconnect(false);

            Stop(false);

            if (Disconnected != null)
            {
                Disconnected(this, e);
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

                    client.Shutdown(SocketShutdown.Both);

                    server.Shutdown(SocketShutdown.Both);
                }
            }

            if (wait)
            {
                //TODO
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
                    client.Dispose();

                    server.Dispose();
                }
            }
        }        
    }
}
