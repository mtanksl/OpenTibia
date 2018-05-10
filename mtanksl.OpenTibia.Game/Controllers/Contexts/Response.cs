using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class Response
    {
        private Context context;

        public Context Context
        {
            get
            {
                return context;
            }
            set
            {
                context = value;

                if (context != null && context.Response != this)
                {
                    context.Response = this;
                }
            }
        }

        private Dictionary<IConnection, Message> messages = new Dictionary<IConnection, Message>();

        public void Write(IOutgoingPacket packet)
        {
            Write(Context.Request.Connection, packet);
        }

        public void Write(IConnection connection, IOutgoingPacket packet)
        {
            Message message;

            if ( !messages.TryGetValue(connection, out message) )
            {
                message = new Message();

                messages.Add(connection, message);
            }

            message.Add(packet);
        }

        public void Flush()
        {
            foreach (var pair in messages)
            {
                IConnection connection = pair.Key;

                Message message = pair.Value;

                connection.Send( message.GetBytes(connection.Keys) );
            }
        }
    }
}