using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
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

        private Socket socket;

        private int receiveTimeout;

        private int sendTimeout;

        public Connection(Socket socket, int receiveTimeout, int sendTimeout)
        {
            this.socket = socket;

            this.receiveTimeout = receiveTimeout;

            this.sendTimeout = sendTimeout;

            this.ipAddress = ( (IPEndPoint)socket.RemoteEndPoint).Address.ToString();
        }

        ~Connection()
        {
            Dispose(false);
        }

        private string ipAddress;

        public string IpAddress
        {
            get
            {
                return ipAddress;
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

        public MessageProtocol MessageProtocol { get; set; }

        public uint[] Keys { get; set; }

        public void Start()
        {
            lock (sync)
            {
                if ( !stopped )
                {
                    if (IncreaseActiveConnection() && IsConnectionCountOk() )
                    {
                        OnConnected();

                        headerLength = 2;

                        IAsyncResult slowSender = socket.BeginReceive(header, 0, headerLength, SocketFlags.None, ReceiveHeader, null);

                        if ( !slowSender.IsCompleted)
                        {
                            ThreadPool.RegisterWaitForSingleObject(slowSender.AsyncWaitHandle, (state, timeout) => 
                            {
                                lock (sync)
                                {
                                    if (timeout)
                                    {
                                        IncreaseSlowSocket();
                                    }
                                }

                            }, null, receiveTimeout, true);
                        }
                    }
                }
            }
        }

        private byte[] header = new byte[2];

        private int headerLength;

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
                    if (IsPacketCountOk() )
                    {
                        try
                        {
                            if (headerLength == socket.EndReceive(result) )
                            {
                                bodyLength = header[1] << 8 | header[0];

                                IAsyncResult slowSender = socket.BeginReceive(body, 0, bodyLength, SocketFlags.None, ReceiveBody, null);

                                if ( !slowSender.IsCompleted)
                                {
                                    ThreadPool.RegisterWaitForSingleObject(slowSender.AsyncWaitHandle, (state, timeout) => 
                                    {
                                        lock (sync)
                                        {
                                            if (timeout)
                                            {
                                                IncreaseSlowSocket();
                                            }
                                        }

                                    }, null, receiveTimeout, true);
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

        private byte[] body = new byte[65535];

        private int bodyLength;

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
                    if (IsPacketCountOk() )
                    {
                        try
                        {
                            if (bodyLength == socket.EndReceive(result) )
                            {
                                OnReceived(body, bodyLength);

                                if ( !stopped )
                                {
                                    headerLength = 2;

                                    socket.BeginReceive(header, 0, headerLength, SocketFlags.None, ReceiveHeader, null);
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
        
        public void Send(byte[] bytes)
        {
            lock (sync)
            {
                if ( !stopped )
                {
                    IAsyncResult slowReceiver = socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, Send, bytes);

                    if ( !slowReceiver.IsCompleted)
                    {
                        ThreadPool.RegisterWaitForSingleObject(slowReceiver.AsyncWaitHandle, (state, timeout) => 
                        {
                            lock (sync)
                            {
                                if (timeout)
                                {
                                    IncreaseSlowSocket();
                                }
                            }

                        }, null, sendTimeout, true);
                    }
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
                        byte[] bytes = ( byte[] )result.AsyncState;

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
            lock (sync)
            {
                if (!stopped)
                {
                    OnDisconnected(new DisconnectedEventArgs(DisconnectionType.Requested) );
                }
            }
        }

        protected virtual bool IncreaseActiveConnection()
        {
            return true;
        }

        protected virtual void DecreaseActiveConnection()
        {

        }

        protected virtual bool IsConnectionCountOk()
        {
            return true;
        }

        protected virtual bool IsPacketCountOk()
        {
            return true;
        }

        protected virtual void IncreaseSlowSocket()
        {

        }

        protected virtual void IncreaseInvalidMessage()
        {
            
        }

        protected virtual void IncreaseUnknownPacket()
        {
            
        }

        protected virtual void OnConnected()
        {

        }

        protected virtual void OnReceived(byte[] body, int length)
        {

        }

        public event EventHandler<DisconnectedEventArgs> Disconnected;

        protected virtual void OnDisconnected(DisconnectedEventArgs e)
        {
            DecreaseActiveConnection();

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
                    if (socket != null)
                    {
                        socket.Dispose();
                    }
                }
            }
        }        
    }
}