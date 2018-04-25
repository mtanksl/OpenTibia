using OpenTibia.IO;
using OpenTibia.Threading;
using System;
using System.Net.Sockets;
using System.Threading;

namespace OpenTibia
{
    public abstract class TibiaClient : IClient
    {
        private readonly object sync = new object();

            private bool stop = false;

            private bool first = true;

            private AutoResetEvent syncStop = new AutoResetEvent(false);
        
        private Socket socket;

        public TibiaClient(Socket socket)
        {
            this.socket = socket;
        }

        ~TibiaClient()
        {
            Dispose(false);
        }

        public uint[] Keys { get; set; }

        public Message Response
        {
            get
            {
                DispatcherContext context = DispatcherContext.Current;

                if (context != null)
                {
                    return context.GetItem<MessageCollection>("Messages", () => new MessageCollection() ).GetMessage(this);
                }

                return null;
            }
        }
        
        public void Start()
        {
            lock (sync)
            {
                OnConnect();

                byte[] header = new byte[2]; socket.BeginReceive(header, 0, header.Length, SocketFlags.None, ReceiveHeader, header);
            }
        }
        
        private void ReceiveHeader(IAsyncResult result)
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
                        byte[] header = result.AsyncState as byte[];
                
                        if (header.Length == socket.EndReceive(result) )
                        {
                            byte[] body = new byte[ header[1] << 8 | header[0] ]; socket.BeginReceive(body, 0, body.Length, SocketFlags.None, ReceiveBody, body);
                        }
                        else
                        {
                            OnDisconnect(new DisconnectEventArgs(DisconnetionType.SocketClosed) );
                        }
                    }
                    catch (SocketException)
                    {
                        OnDisconnect(new DisconnectEventArgs(DisconnetionType.SocketException) );
                    }
                }
            }
        }
        
        private void ReceiveBody(IAsyncResult result)
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
                        byte[] body = result.AsyncState as byte[];
                 
                        if (body.Length == socket.EndReceive(result) )
                        {
                            OnReceive(body, first);

                            if (first)
                            {
                                first = false;   
                            }

                            byte[] header = new byte[2]; socket.BeginReceive(header, 0, header.Length, SocketFlags.None, ReceiveHeader, header);
                        }
                        else
                        {
                            OnDisconnect(new DisconnectEventArgs(DisconnetionType.SocketClosed) );
                        }
                    }
                    catch (SocketException)
                    {
                        OnDisconnect(new DisconnectEventArgs(DisconnetionType.SocketException) );
                    }
                }
            }
        }
        
        public void Send(byte[] bytes)
        {
            lock (sync)
            {
                if ( !stop )
                {
                    socket.BeginSend(bytes, 0, bytes.Length, SocketFlags.None, Send, bytes);
                }
            }
        }

        private void Send(IAsyncResult result)
        {
            lock (sync)
            {
                if (stop)
                {
                    //Empty
                }
                else
                {
                    try
                    {
                        byte[] bytes = result.AsyncState as byte[];

                        if (bytes.Length == socket.EndSend(result) )
                        {
                            //Empty
                        }
                        else
                        {
                            //Empty
                        }
                    }
                    catch (SocketException)
                    {
                        //Empty
                    }
                }
            }
        }

        protected virtual void OnConnect() { }

        protected virtual void OnReceive(byte[] bytes, bool first) { }

        protected virtual void OnDisconnect(DisconnectEventArgs e)
        {
            switch (e.Type)
            {
                case DisconnetionType.Logout:

                    Stop();

                    break;

                case DisconnetionType.SocketClosed:

                case DisconnetionType.SocketException:

                    Stop(false);

                    break;
            }

            if (Disconnect != null)
            {
                Disconnect(this, e);
            }
        }

        public event EventHandler<DisconnectEventArgs> Disconnect;
          
        public void ExecuteInContext(Action callback)
        {
            DispatcherContext context = DispatcherContext.Current;

            if (context == null)
            {
                Game.Current.Dispatcher.QueueForExecution( () => 
                {
                    callback();
                } );
            }
            else
            {
                callback();
            }
        }

        public void ExecuteInContext<T>(ByteArrayStreamReader reader, Action<T> callback) where T : IIncomingPacket
        {
            T packet = Game.Current.IncomingPacketFactory.Create<T>(reader);

            DispatcherContext context = DispatcherContext.Current;

            if (context == null)
            {
                Game.Current.Dispatcher.QueueForExecution( () => 
                {
                    callback(packet);
                } );
            }
            else
            {
                callback(packet);
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
                    socket.Dispose();
                }               
            }
        }
    }
}