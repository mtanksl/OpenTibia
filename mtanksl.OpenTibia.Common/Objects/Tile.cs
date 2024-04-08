using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Tile : IContainer
    {
        private RecomputableSource recomputableSource;

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

        public bool ProtectionZone { get; set; }

        public bool NoLogoutZone { get; set; }

        private Recomputable<Item> ground;

        public Item Ground
        {
            get
            {
                if (ground == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    ground = new Recomputable<Item>(recomputableSource, () =>
                    {
                        return GetItems().Where(i => i.TopOrder == TopOrder.Ground).LastOrDefault();
                    } );
                }

                return ground.Value;
            }
        }

        private Recomputable<Item> topItem;

        public Item TopItem
        {
            get
            {
                if (topItem == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    topItem = new Recomputable<Item>(recomputableSource, () =>
                    {
                        return GetItems().Where(i => i.TopOrder == TopOrder.Other).FirstOrDefault() ??

                               GetItems().Where(i => i.TopOrder == TopOrder.HighPriority || i.TopOrder == TopOrder.MediumPriority || i.TopOrder == TopOrder.LowPriority).LastOrDefault();
                    } );
                }

                return topItem.Value;
            }
        }

        private Recomputable<Creature> topCreature;

        public Creature TopCreature
        {
            get
            {
                if (topCreature == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    topCreature = new Recomputable<Creature>(recomputableSource, () =>
                    {
                        return GetCreatures().LastOrDefault();
                    } );
                }

                return topCreature.Value;
            }
        }

        private Recomputable<FloorChange> floorChange;

        public FloorChange FloorChange
        {
            get
            {
                if (floorChange == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    floorChange = new Recomputable<FloorChange>(recomputableSource, () =>
                    {
                        FloorChange floorChange = FloorChange.None;

                        foreach (var item in GetItems() )
                        {
                            if (item.Metadata.FloorChange != null)
                            {
                                floorChange |= item.Metadata.FloorChange.Value;
                            }
                        }

                        return floorChange;
                    } );
                }

                return floorChange.Value;
            }
        }

        private Recomputable<int> height;

        public int Height
        {
            get
            {
                if (height == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    height = new Recomputable<int>(recomputableSource, () =>
                    {
                        int height = 0;

                        foreach (var item in GetItems() )
                        {
                            if (item.Metadata.Flags.Is(ItemMetadataFlags.HasHeight) )
                            {
                                height++;
                            }
                        }

                        return height;
                    } );
                }

                return height.Value;
            }
        }

        private Recomputable<bool> blockProjectile;

        public bool BlockProjectile
        {
            get
            {
                if (blockProjectile == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    blockProjectile = new Recomputable<bool>(recomputableSource, () =>
                    {
                        return GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.BlockProjectile) );
                    } );
                }

                return blockProjectile.Value;
            }
        }

        private Recomputable<bool> notWalkable;

        public bool NotWalkable
        {
            get
            {
                if (notWalkable == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    notWalkable = new Recomputable<bool>(recomputableSource, () =>
                    {
                        return GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) );
                    } );
                }

                return notWalkable.Value;
            }
        }

        private Recomputable<bool> blockPathFinding;

        public bool BlockPathFinding
        {
            get
            {
                if (blockPathFinding == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    blockPathFinding = new Recomputable<bool>(recomputableSource, () =>
                    {
                        return GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) );
                    } );
                }

                return blockPathFinding.Value;
            }
        }

        private Recomputable<bool> block;

        public bool Block
        {
            get
            {
                if (block == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    block = new Recomputable<bool>(recomputableSource, () =>
                    {
                        return GetCreatures().Any(c => c.Block);
                    } );
                }

                return block.Value;
            }
        }

        private Recomputable<bool> canUseWith;

        public bool CanUseWith
        {
            get
            {
                if (canUseWith == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    canUseWith = new Recomputable<bool>(recomputableSource, () =>
                    {
                        return !NotWalkable && !BlockPathFinding;
                    } );
                }

                return canUseWith.Value;
            }
        }

        private Recomputable<bool> canWalk;

        public bool CanWalk
        {
            get
            {
                if (canWalk == null)
                {
                    if (recomputableSource == null)
                    {
                        recomputableSource = new RecomputableSource();
                    }

                    canWalk = new Recomputable<bool>(recomputableSource, () =>
                    {
                        return !NotWalkable && !BlockPathFinding && !Block;
                    } );
                }

                return canWalk.Value;
            }
        }

        private List<IContent> contents = new List<IContent>(1);

        public int Count
        {
            get
            {
                return contents.Count;
            }
        }

        public int AddContent(IContent content)
        {
            if (recomputableSource != null)
            {
                recomputableSource.Change();
            }

            //13 Other 1
            //12 Other 2 
            //11 Other 3
            //10 Creature 3
            //9 Creature 2
            //8 Creature 1
            //7 LowPriority 2
            //6 LowPriority 1
            //5 MediumPriority 2
            //4 MediumPriority 1
            //3 HighPriority 2
            //2 HighPriority 1
            //1 Ground 2
            //0 Ground 1

            int index = 0;

            if (content.TopOrder == TopOrder.Other)
            {
                while (index < contents.Count)
                {
                    if (contents[index].TopOrder == TopOrder.Other)
                    {
                        break;
                    }

                    index++;
                }
            }
            else
            {
                while (index < contents.Count)
                {
                    if (contents[index].TopOrder > content.TopOrder)
                    {
                        break;
                    }

                    index++;
                }
            }

            contents.Insert(index, content);

            content.Parent = this;

            return index;
        }

        /// <exception cref="NotSupportedException"></exception>

        public void AddContent(IContent content, int index)
        {
            throw new NotSupportedException();
        }

        public void ReplaceContent(int index, IContent content)
        {
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

        public IEnumerable<Creature> GetCreatures()
        {
            return GetContents().OfType<Creature>();
        }

        public IEnumerable<Monster> GetMonsters()
        {
            return GetContents().OfType<Monster>();
        }

        public IEnumerable<Npc> GetNpcs()
        {
            return GetContents().OfType<Npc>();
        }

        public IEnumerable<Player> GetPlayers()
        {
            return GetContents().OfType<Player>();
        }       
    }
}