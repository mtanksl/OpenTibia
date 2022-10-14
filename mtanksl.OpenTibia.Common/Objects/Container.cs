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

        public byte Count
        {
            get
            {
                return (byte)contents.Count;
            }
        }

        private List<IContent> contents = new List<IContent>();

        /// <exception cref="ArgumentException"></exception>

        public byte AddContent(IContent content)
        {
            if ( !(content is Item) )
            {
                throw new ArgumentException("Content must be an item.");
            }

            byte index = 0;

            contents.Insert(index, content);

            content.Parent = this;

            return index;
        }

        /// <exception cref="NotSupportedException"></exception>

        public void AddContent(IContent content, byte index)
        {
            throw new NotSupportedException();
        }

        /// <exception cref="ArgumentException"></exception>

        public void ReplaceContent(byte index, IContent content)
        {
            if ( !(content is Item) )
            {
                throw new ArgumentException("Content must be an item.");
            }

            IContent oldContent = GetContent(index);

            contents[index] = content;

            oldContent.Parent = null;

            content.Parent = this;
        }

        public void RemoveContent(byte index)
        {
            IContent content = GetContent(index);

            contents.RemoveAt(index);

            content.Parent = null;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public byte GetIndex(IContent content)
        {
            for (byte index = 0; index < contents.Count; index++)
            {
                if (contents[index] == content)
                {
                    return index;
                }
            }

            throw new InvalidOperationException("Content not found.");
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

        public IEnumerable< KeyValuePair<byte, IContent> > GetIndexedContents()
        {
            for (byte index = 0; index < contents.Count; index++)
            {
                yield return new KeyValuePair<byte, IContent>( index, contents[index] );
            }
        }

        public IEnumerable<Item> GetItems()
        {
            return GetContents().OfType<Item>();
        }

        private Dictionary<Player, int> players = new Dictionary<Player, int>();

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