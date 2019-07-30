using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class CommandContext : IDisposable
    {
        public CommandContext()
        {
            
        }

        ~CommandContext()
        {
            Dispose(false);
        }

        private Dictionary<IConnection, Message> messages = new Dictionary<IConnection, Message>();

        public CommandContext Write(IConnection connection, IOutgoingPacket packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException( nameof(CommandContext) );
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

        public CommandContext Write(IConnection connection, params IOutgoingPacket[] packet)
        {
            if (disposed)
            {
                throw new ObjectDisposedException( nameof(CommandContext) );
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

        private HashSet<IConnection> connections = new HashSet<IConnection>();

        public CommandContext Disconnect(IConnection connection)
        {
            if (disposed)
            {
                throw new ObjectDisposedException( nameof(CommandContext) );
            }

            connections.Add(connection);

            return this;
        }

        public void Flush()
        {
            if (disposed)
            {
                throw new ObjectDisposedException( nameof(CommandContext) );
            }

            foreach (var pair in messages)
            {
                IConnection connection = pair.Key;

                Message message = pair.Value;

                connection.Send( message.GetBytes(connection.Keys) );
            }

            foreach (var connection in connections)
            {
                connection.Disconnect();
            }

            messages.Clear();

            connections.Clear();
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