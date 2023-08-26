using OpenTibia.Game;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class WaitingList
    {
        private static readonly int MaxPlayers = 100;

        private class Item
        {
            public int PlayerId { get; set; }

            public DateTime Timeout { get; set; }
        }

        private Server server;

        public WaitingList(Server server)
        {
            this.server = server;
        }

        private LinkedList<Item> queue = new LinkedList<Item>();

        public bool CanLogin(int playerId, out int position, out byte time)
        {
            int onlinePlayers = server.GameObjects.GetPlayers().Count();

            if (queue.Count == 0)
            {
                if (onlinePlayers < MaxPlayers)
                {
                    position = 0;

                    time = 0;

                    return true;
                }
            }

            for (var node = queue.First; node != null; node = node.Next)
            {
                if (DateTimeOffset.UtcNow >= node.Value.Timeout)
                {
                    queue.Remove(node);
                }
            }

            Item item = GetItem(playerId);

            if (item == null)
            {
                position = queue.Count + 1;

                time = GetTime(position);

                queue.AddLast(new Item()
                {
                    PlayerId = playerId,

                    Timeout = DateTime.UtcNow.AddSeconds(time + 1)
                } );

                return false;
            }
            else
            {
                position = GetIndex(playerId) + 1;

                time = GetTime(position);

                if (onlinePlayers + position > MaxPlayers)
                {
                    item.Timeout = DateTime.UtcNow.AddSeconds(time + 1);

                    return false;
                }

                queue.Remove(item);

                return true;
            }
        }

        private Item GetItem(int playerId)
        {
            for (var node = queue.First; node != null; node = node.Next)
            {
                if (node.Value.PlayerId == playerId)
                {
                    return node.Value;
                }
            }

            return null;
        }

        private int GetIndex(int playerId)
        {
            int i = 0;

            for (var node = queue.First; node != null; node = node.Next, i++)
            {
                if (node.Value.PlayerId == playerId)
                {
                    return i;
                }
            }

            return -1;
        }

        private byte GetTime(int position)
        {
            if (position < 5)
            {
                return 5;
            }

            if (position < 10)
            {
                return 10;
            }

            if (position < 20)
            {
                return 20;
            }

            if (position < 50)
            {
                return 60;
            }

            return 120;
       }
    }
}