using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using System;
using System.Net.Sockets;
using System.Threading;

namespace OpenTibia.Network.Sockets
{
    public abstract class Connection : IConnection, IDisposable
    {
        private readonly object sync = new object();

            private bool stopped = false;
        
            private AutoResetEvent syncStop = new AutoResetEvent(false);

        private Socket socket;

        public Connection(Socket socket)
        {
            this.socket = socket;
        }

        ~Connection()
        {
            Dispose(false);
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
                OnConnected();

                if ( !stopped )
                {
                    byte[] header = new byte[2];

                    socket.BeginReceive(header, 0, header.Length, SocketFlags.None, ReceiveHeader, header);
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
                    try
                    {
                        byte[] header = result.AsyncState as byte[];
                
                        if (header.Length == socket.EndReceive(result) )
                        {
                            byte[] body = new byte[ header[1] << 8 | header[0] ];

                            socket.BeginReceive(body, 0, body.Length, SocketFlags.None, ReceiveBody, body);
                        }
                        else
                        {
                            OnDisconnected(new DisconnectedEventArgs(DisconnetionType.SocketClosed) );
                        }
                    }
                    catch (SocketException)
                    {
                        OnDisconnected(new DisconnectedEventArgs(DisconnetionType.SocketException) );
                    }
                }
            }
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
                    try
                    {
                        byte[] body = result.AsyncState as byte[];
                 
                        if (body.Length == socket.EndReceive(result) )
                        {
                            OnReceived(body);

                            if ( !stopped )
                            {
                                byte[] header = new byte[2];

                                socket.BeginReceive(header, 0, header.Length, SocketFlags.None, ReceiveHeader, header);
                            }
                        }
                        else
                        {
                            OnDisconnected(new DisconnectedEventArgs(DisconnetionType.SocketClosed) );
                        }
                    }
                    catch (SocketException)
                    {
                        OnDisconnected(new DisconnectedEventArgs(DisconnetionType.SocketException) );
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
                    socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, Send, bytes);
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

                        if (bytes.Length == socket.EndSend(result) )
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
            if (!stopped)
            {
                OnDisconnected(new DisconnectedEventArgs(DisconnetionType.Requested) );
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

                    socket.Shutdown(SocketShutdown.Both);
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
                    socket.Dispose();
                }
            }
        }        
    }
}