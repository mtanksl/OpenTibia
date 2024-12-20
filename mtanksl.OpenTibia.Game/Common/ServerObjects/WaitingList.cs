using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class WaitingList : IWaitingList
    {
        private class WaitingItem
        {
            public string Name { get; set; }

            public DateTime Timeout { get; set; }
        }

        private IServer server;

        public WaitingList(IServer server)
        {
            this.server = server;
        }

        private LinkedList<WaitingItem> queue = new LinkedList<WaitingItem>();

        public bool CanLogin(string name, out int position, out byte time)
        {
            int onlinePlayers = server.GameObjects.GetPlayers().Count();

            if (queue.Count == 0)
            {
                if (onlinePlayers < server.Config.GameplayMaxPlayers)
                {
                    position = 0;

                    time = 0;

                    return true;
                }
            }

            for (var node = queue.First; node != null; node = node.Next)
            {
                if (DateTime.UtcNow >= node.Value.Timeout)
                {
                    queue.Remove(node);
                }
            }

            WaitingItem item = GetItem(name);

            if (item == null)
            {
                position = queue.Count + 1;

                time = GetTime(position);

                queue.AddLast(new WaitingItem()
                {
                    Name = name,

                    Timeout = DateTime.UtcNow.AddSeconds(time + 1)
                } );

                return false;
            }
            else
            {
                position = GetIndex(name) + 1;

                time = GetTime(position);

                if (onlinePlayers + position > server.Config.GameplayMaxPlayers)
                {
                    item.Timeout = DateTime.UtcNow.AddSeconds(time + 1);

                    return false;
                }

                queue.Remove(item);

                return true;
            }
        }

        private WaitingItem GetItem(string name)
        {
            for (var node = queue.First; node != null; node = node.Next)
            {
                if (node.Value.Name == name)
                {
                    return node.Value;
                }
            }

            return null;
        }

        private int GetIndex(string name)
        {
            int i = 0;

            for (var node = queue.First; node != null; node = node.Next, i++)
            {
                if (node.Value.Name == name)
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