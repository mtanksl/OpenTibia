using OpenTibia.Common.Objects;
using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;

namespace OpenTibia.Network.Sockets
{
    public abstract class Connection : IConnection, IDisposable
    {
        private readonly object sync = new object();

            private bool stopped = false;
        
            private AutoResetEvent syncStop = new AutoResetEvent(false);

        private Listener listener;

        private Socket clientSocket;

        public Connection(Listener listener, Socket clientSocket)
        {
            this.listener = listener;

            this.clientSocket = clientSocket;
        }

        ~Connection()
        {
            Dispose(false);
        }

        public string IpAddress
        {
            get
            {
                return ( (IPEndPoint)clientSocket.RemoteEndPoint ).Address.ToString();
            }
        }

        private IClient client;

        public IClient Client
        {
            get
            {
                return client;
            }
            set
            {
                if (value != client)
                {
                    var current = client;
                    
                                  client = value;

                    if (value == null)
                    {
                        current.Connection = null;
                    }
                    else
                    {
                        client.Connection = this;
                    }
                }
            }
        }
        
        public uint[] Keys { get; set; }

        public void Start()
        {
            lock (sync)
            {
                if ( !stopped )
                {
                    if (listener.IsBanned(IpAddress) )
                    {
                        Disconnect();
                    }
                    else
                    {
                        OnConnected();

                        byte[] header = new byte[2];

                        clientSocket.BeginReceive(header, 0, header.Length, SocketFlags.None, ReceiveHeader, header);
                    }
                }
            }
        }

        private void ReceiveHeader(IAsyncResult result)
        {
            lock (sync)
            {
                if (stopped)
                {
                    syncStop.Set();
                }
                else
                {
                    if (listener.IsBanned(IpAddress) )
                    {
                        Disconnect();
                    }
                    else
                    {
                        try
                        {
                            byte[] header = result.AsyncState as byte[];
                
                            if (header.Length == clientSocket.EndReceive(result) )
                            {
                                byte[] body = new byte[ header[1] << 8 | header[0] ];

                                clientSocket.BeginReceive(body, 0, body.Length, SocketFlags.None, ReceiveBody, body);
                            }
                            else
                            {
                                OnDisconnected(new DisconnectedEventArgs(DisconnectionType.SocketClosed) );
                            }
                        }
                        catch (SocketException)
                        {
                            OnDisconnected(new DisconnectedEventArgs(DisconnectionType.SocketException) );
                        }
                    }
                }
            }
        }

        private DateTime previous = DateTime.MinValue;

        private int count = 0;

        private bool IsRateLimited()
        {
            if ( (DateTime.UtcNow - previous).TotalSeconds > 1)
            {
                previous = DateTime.UtcNow;

                count = 0;
            }
            else
            {
                count++;

                if (count > 10)
                {
                    return true;
                }
            }

            return false;
        }

        private void ReceiveBody(IAsyncResult result)
        {
            lock (sync)
            {
                if (stopped)
                {
                    syncStop.Set();
                }
                else
                {
                    if (listener.IsBanned(IpAddress) )
                    {
                        Disconnect();
                    }
                    else
                    {
                        if (IsRateLimited() )
                        {
                            listener.AddBan(IpAddress);

                            Disconnect();
                        }
                        else
                        {
                            try
                            {
                                byte[] body = result.AsyncState as byte[];
                 
                                if (body.Length == clientSocket.EndReceive(result) )
                                {
                                    OnReceived(body);

                                    if ( !stopped )
                                    {
                                        byte[] header = new byte[2];

                                        clientSocket.BeginReceive(header, 0, header.Length, SocketFlags.None, ReceiveHeader, header);
                                    }
                                }
                                else
                                {
                                    OnDisconnected(new DisconnectedEventArgs(DisconnectionType.SocketClosed) );
                                }
                            }
                            catch (SocketException)
                            {
                                OnDisconnected(new DisconnectedEventArgs(DisconnectionType.SocketException) );
                            }
                        }
                    }
                }
            }
        }
        
        public void Send(byte[] bytes)
        {
            lock (sync)
            {
                if ( !stopped )
                {
                    clientSocket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, Send, bytes);
                }
            }
        }

        private void Send(IAsyncResult result)
        {
            lock (sync)
            {
                if (stopped)
                {
                    //
                }
                else
                {
                    try
                    {
                        byte[] bytes = result.AsyncState as byte[];

                        if (bytes.Length == clientSocket.EndSend(result) )
                        {
                            //
                        }
                        else
                        {
                            //
                        }
                    }
                    catch (SocketException)
                    {
                        //
                    }
                }
            }
        }

        public void Disconnect()
        {
            if ( !stopped )
            {
                OnDisconnected(new DisconnectedEventArgs(DisconnectionType.Requested) );
            }
        }

        protected virtual void OnConnected()
        {

        }

        protected virtual void OnReceived(byte[] body)
        {

        }

        public event EventHandler<DisconnectedEventArgs> Disconnected;

        protected virtual void OnDisconnected(DisconnectedEventArgs e)
        {
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

                    clientSocket.Shutdown(SocketShutdown.Both);
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
                    if (clientSocket != null)
                    {
                        clientSocket.Dispose();
                    }
                }
            }
        }        
    }
}