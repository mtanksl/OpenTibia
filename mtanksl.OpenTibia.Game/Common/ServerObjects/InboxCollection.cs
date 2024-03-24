using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class InboxCollection
    {
        private class Inbox
        {
            private Dictionary<ushort, Queue<Item> > queues = new Dictionary<ushort, Queue<Item> >();

            public void AddItem(ushort townId, Item item)
            {
                Queue<Item> queue;

                if ( !queues.TryGetValue(townId, out queue) )
                {
                    queue = new Queue<Item>();

                    queues.Add(townId, queue);
                }

                queue.Enqueue(item);
            }

            public Queue<Item> GetItems(ushort townId)
            {
                Queue<Item> queue;

                queues.TryGetValue(townId, out queue);

                return queue;
            }
        }

        private Dictionary<int, Inbox> inboxes = new Dictionary<int, Inbox>();

        public void AddItem(int databasePlayerId, ushort townId, Item item)
        {
            Inbox inbox;

            if ( !inboxes.TryGetValue(databasePlayerId, out inbox) )
            {
                inbox = new Inbox();

                inboxes.Add(databasePlayerId, inbox);
            }

            inbox.AddItem(townId, item);
        }

        public Queue<Item> GetItems(int databasePlayerId, ushort townId)
        {
            Inbox inbox;

            if (inboxes.TryGetValue(databasePlayerId, out inbox) )
            {
                return inbox.GetItems(townId);
            }

            return null;
        }
    }
}