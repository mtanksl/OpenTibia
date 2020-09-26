using OpenTibia.Common.Events;
using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class Context : IDisposable
    {
        public Context(Server server)
        {
            this.server = server;
        }

        ~Context()
        {
            Dispose(false);
        }

        private Server server;

        public Server Server
        {
            get
            {
                return server;
            }
        }

        private Dictionary<string, object> data;

        public Dictionary<string, object> Data
        {
            get
            {
                return data ?? (data = new Dictionary<string, object>() );
            }
        }

        private List<GameEventArgs> events;

        public Context AddEvent(GameEventArgs e)
        {
            if (disposed)
            {
                throw new ObjectDisposedException( nameof(Context) );
            }

            if (events == null)
            {
                events = new List<GameEventArgs>();
            }

            events.Add(e);

            return this;
        }

        private Dictionary<IConnection, Message> messages = null;

        public Context AddPacket(IConnection connection, IOutgoingPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException( nameof(Context) );
            }

            if (messages == null)
            {
                messages = new Dictionary<IConnection, Message>();
            }

            Message message;

            if ( !messages.TryGetValue(connection, out message) )
            {
                message = new Message();

                messages.Add(connection, message);
            }

            message.Add(packet);

            return this;
        }

        public Context AddPacket(IConnection connection, params IOutgoingPacket[] packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException( nameof(Context) );
            }

            if (messages == null)
            {
                messages = new Dictionary<IConnection, Message>();
            }

            Message message;

            if ( !messages.TryGetValue(connection, out message) )
            {
                message = new Message();

                messages.Add(connection, message);
            }

            message.Add(packet);

            return this;
        }

        private HashSet<IConnection> connections = null;

        public Context Disconnect(IConnection connection)
        {
            if (disposed)
            {
                throw new ObjectDisposedException( nameof(Context) );
            }

            if (connections == null)
            {
                connections = new HashSet<IConnection>();
            }

            connections.Add(connection);

            return this;
        }

        public void Flush()
        {
            if (disposed)
            {
                throw new ObjectDisposedException( nameof(Context) );
            }

            if (events != null)
            {
                foreach (var e in events)
                {
                    server.Events.Publish(this, e);
                }

                events.Clear();
            }

            if (messages != null)
            {
                foreach (var pair in messages)
                {
                    IConnection connection = pair.Key;

                    Message message = pair.Value;

                    connection.Send( message.GetBytes(connection.Keys) );
                }

                messages.Clear();
            }

            if (connections != null)
            {
                foreach (var connection in connections)
                {
                    connection.Disconnect();
                }

                connections.Clear();
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
            if ( !disposed )
            {
                disposed = true;

                if (disposing)
                {
                    
                }
            }
        }
    }
}