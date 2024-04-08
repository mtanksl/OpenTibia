using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Container : Item, IContainer
    {
        private RecomputableSource recomputableSource;
                
        public Container(ItemMetadata metadata) : base(metadata)
        {
            
        }

        private Recomputable<uint> weight;

        public override uint Weight
        {
            get
            {
                if (weight == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    weight = new Recomputable<uint>(recomputableSource, () =>
                    {
                        uint weight = base.Weight;

                        foreach (var item in GetItems() )
                        {
                            weight += item.Weight;
                        }

                        return weight;
                    } );
                }

                return weight.Value;
            }
        }

        private List<IContent> contents = new List<IContent>();

        public int Count
        {
            get
            {
                return contents.Count;
            }
        }

        /// <exception cref="ArgumentException"></exception>

        public int AddContent(IContent content)
        {
            if ( !(content is Item) )
            {
                throw new ArgumentException("Content must be an item.");
            }

            if (recomputableSource != null)
            {
                recomputableSource.Change();
            }

            int index = 0;

            contents.Insert(index, content);

            content.Parent = this;

            return index;
        }

        /// <exception cref="NotSupportedException"></exception>

        public void AddContent(IContent content, int index)
        {
            throw new NotSupportedException();
        }

        /// <exception cref="ArgumentException"></exception>

        public void ReplaceContent(int index, IContent content)
        {
            if ( !(content is Item) )
            {
                throw new ArgumentException("Content must be an item.");
            }

            if (recomputableSource != null)
            {
                recomputableSource.Change();
            }

            IContent oldContent = GetContent(index);

            contents[index] = content;

            oldContent.Parent = null;

            content.Parent = this;
        }

        public void RemoveContent(int index)
        {
            if (recomputableSource != null)
            {
                recomputableSource.Change();
            }

            IContent content = GetContent(index);

            contents.RemoveAt(index);

            content.Parent = null;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public int GetIndex(IContent content)
        {
            for (int index = 0; index < contents.Count; index++)
            {
                if (contents[index] == content)
                {
                    return index;
                }
            }

            throw new InvalidOperationException("Content not found.");
        }

        public bool TryGetIndex(IContent content, out int _index)
        {
            for (int index = 0; index < contents.Count; index++)
            {
                if (contents[index] == content)
                {
                    _index = index;

                    return true;
                }
            }

            _index = 0;

            return false;
        }

        public IContent GetContent(int index)
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

        public IEnumerable< KeyValuePair<int, IContent> > GetIndexedContents()
        {
            for (int index = 0; index < contents.Count; index++)
            {
                yield return new KeyValuePair<int, IContent>( index, contents[index] );
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