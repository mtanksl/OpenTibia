using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class Context : IDisposable
    {
        public Context()
        {
            
        }

        ~Context()
        {
            Dispose(false);
        }

        private Dictionary<IConnection, Message> messages = null;

        public Context Write(IConnection connection, IOutgoingPacket packet)
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

        public Context Write(IConnection connection, params IOutgoingPacket[] packet)
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