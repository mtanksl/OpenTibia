using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Container : Item, IContainer
    {
        public Container(ItemMetadata metadata) : base(metadata)
        {

        }

        private List<IContent> contents = new List<IContent>();
        
        public byte AddContent(IContent content)
        {
            byte index = 0;

            contents.Insert(index, content);

            content.Container = this;

            return index;
        }

        public void AddContent(byte index, IContent content)
        {
            throw new NotSupportedException();
        }

        public void RemoveContent(byte index)
        {
            IContent content = GetContent(index);

            contents.RemoveAt(index);

            content.Container = null;
        }
        
        public byte GetIndex(IContent content)
        {
            for (byte index = 0; index < contents.Count; index++)
            {
                if (contents[index] == content)
                {
                    return index;
                }
            }

            throw new Exception("Content not found.");
        }

        public bool TryGetIndex(IContent content, out byte i)
        {
            for (byte index = 0; index < contents.Count; index++)
            {
                if (contents[index] == content)
                {
                    i = index;

                    return true;
                }
            }

            i = 0;

            return false;
        }

        public IContent GetContent(byte index)
        {
            if (index < 0 || index > contents.Count - 1)
            {
                return null;
            }

            return contents[index];
        }

        public IEnumerable<IContent> GetContents()
        {
            return contents;
        }

        public IEnumerable<Item> GetItems()
        {
            return contents.OfType<Item>();
        }

        public IEnumerable< KeyValuePair<byte, IContent> > GetIndexedContents()
        {
            for (byte index = 0; index < contents.Count; index++)
            {
                yield return new KeyValuePair<byte, IContent>( index, contents[index] );
            }
        }

        protected Dictionary<Player, int> players = new Dictionary<Player, int>();

        public void AddPlayer(Player player)
        {
            int references;

            if ( !players.TryGetValue(player, out references) )
            {
                references = 1;

                players.Add(player, references);
            }
            else
            {
                players[player] = references + 1;
            }
        }

        public void RemovePlayer(Player player)
        {
            int references;

            if ( players.TryGetValue(player, out references) )
            {
                if (references == 1)
                {
                    players.Remove(player);
                }
                else
                {
                    players[player] = references - 1;
                }
            }
        }

        public bool ContainsPlayer(Player player)
        {
            return players.ContainsKey(player);
        }

        public IEnumerable<Player> GetPlayers()
        {
            return players.Keys;
        }
    }
}