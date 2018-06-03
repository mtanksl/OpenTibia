using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Collections.Generic;

namespace OpenTibia.Web
{
    public class Response
    {
        public Response(Context context)
        {
            this.context = context;
        }

        private Context context;

        public Context Context
        {
            get
            {
                return context;
            }
        }

        private Dictionary<IConnection, Message> messages = new Dictionary<IConnection, Message>();
        
        public Response Write(IConnection connection, IOutgoingPacket packet)
        {
            Message message;

            if ( !messages.TryGetValue(connection, out message) )
            {
                message = new Message();

                messages.Add(connection, message);
            }

            message.Add(packet);

            return this;
        }

        public void Flush()
        {
            foreach (var pair in messages)
            {
                IConnection connection = pair.Key;

                Message message = pair.Value;

                connection.Send( message.GetBytes(connection.Keys) );
            }

            messages.Clear();
        }
    }
}