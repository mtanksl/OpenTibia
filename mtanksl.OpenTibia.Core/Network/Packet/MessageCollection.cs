using System.Collections.Generic;

namespace OpenTibia
{
    public class MessageCollection
    {
        private Dictionary<TibiaClient, Message> clients = new Dictionary<TibiaClient, Message>();

        public Message GetMessage(TibiaClient client)
        {
            Message message;

            if ( !clients.TryGetValue(client, out message) )
            {
                message = new Message();

                clients.Add(client, message);
            }

            return message;
        }

        public void Flush()
        {
            foreach (var pair in clients)
            {
                TibiaClient client = pair.Key; 
                
                Message message = pair.Value;

                client.Send( message.GetBytes(client.Keys) );
            }

            clients.Clear();
        }

        public void Clear()
        {
            clients.Clear();
        }
    }
}