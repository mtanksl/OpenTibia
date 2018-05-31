using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia
{
    public class Tile : IContainer
    {
        public Tile(Position position)
        {
            this.position = position;
        }

        private Position position;

        public Position Position
        {
            get
            {
                return position;
            }
        }

        private List<IContent> contents = new List<IContent>(1);
        
        public int AddContent(IContent content)
        {
            if (content.Container != null)
            {
                throw new Exception("Content already attached");
            }

            int index = 0;
            
            if (content.TopOrder == TopOrder.Other)
	        {
                while (index < contents.Count && contents[index].TopOrder != content.TopOrder)
                {
                    index++;
                }
	        }
            else
            {
                while (index < contents.Count && contents[index].TopOrder <= content.TopOrder)
                {
                    index++;
                }
            }

            contents.Insert(index, content);

            content.Container = this;

            return index;
        }

        public void AddContent(int index, IContent content)
        {
            throw new NotSupportedException();
        }

        public int RemoveContent(IContent content)
        {
            if (content.Container == null)
            {
                throw new Exception("Content already dettached");
            }

            int index = GetIndex(content);

            contents.RemoveAt(index);

            content.Container = null;

            return index;
        }

        public int ReplaceContent(IContent before, IContent after)
        {
            if (before.Container == null)
            {
                throw new Exception("Content already dettached");
            }

            if (after.Container != null)
            {
                throw new Exception("Content already attached");
            }

            int index = GetIndex(before);

            contents[index] = after;

            before.Container = null;

            after.Container = this;

            return index;
        }

        public int GetIndex(IContent content)
        {
            for (int index = 0; index < contents.Count; index++)
            {
                if (contents[index] == content)
                {
                    return index;
                }
            }

            return -1;
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

        public IEnumerable<Item> GetItems()
        {
            return contents.OfType<Item>();
        }

        public IEnumerable<Creature> GetCreatures()
        {
            return contents.OfType<Creature>();
        }
        
        public IEnumerable<Monster> GetMonsters()
        {
            return contents.OfType<Monster>();
        }

        public IEnumerable<Npc> GetNpcs()
        {
            return contents.OfType<Npc>();
        }

        public IEnumerable<Player> GetPlayers()
        {
            return contents.OfType<Player>();
        }



















        public Item Ground
        {
            get
            {
                return GetItems().Where(item => item.TopOrder == TopOrder.Ground).FirstOrDefault();
            }
        }

        public bool Walkable
        {
            get
            {
                bool walkable = false;

                foreach (var item in GetItems() )
                {
                    if (item.TopOrder == TopOrder.Ground)
                    {
                        walkable = true;
                    }

                    if (item.Metadata.Flags.Any(ItemMetadataFlags.NotWalkable) )
                    {
                        walkable = false;

                        break;
                    }
                }
                return walkable;
            }
        }

        public bool BlockPathFinding
        {
            get
            {
                bool blockPathFinding = true;

                foreach (var item in GetItems() )
                {
                    if (item.TopOrder == TopOrder.Ground)
                    {
                        blockPathFinding = false;
                    }

                    if (item.Metadata.Flags.Any(ItemMetadataFlags.NotWalkable) || item.Metadata.Flags.Any(ItemMetadataFlags.BlockPathFinding) )
                    {
                        blockPathFinding = true;

                        break;
                    }
                }
                return blockPathFinding;
            }
        }

        public bool HasHeight
        {
            get
            {
                int height = 0;

                foreach (var item in GetItems() )
                {
                    if (item.Metadata.Flags.Any(ItemMetadataFlags.HasHeight) )
                    {
                        height++;

                        if (height > 2)
                        {
                            return true;
                        }
                    }
                }

                return false;
            }
        }

        public FloorChange FloorChange
        {
            get
            {
                return GetItems().Aggregate(FloorChange.None, (seed, item) => seed |= item.Metadata.FloorChange);
            }
        }

        public Item GetItem(ushort serverId)
        {
            return contents.OfType<Item>().Where(item => item.Metadata.ServerId == serverId).FirstOrDefault();
        }

        public Item GetTopMostItem()
        {
            return contents.OfType<Item>().TakeUntil(item => item.TopOrder == TopOrder.Other).LastOrDefault();
        }

        public Creature GetTopMostCreature()
        {
            return contents.OfType<Creature>().LastOrDefault();
        }
    }
}