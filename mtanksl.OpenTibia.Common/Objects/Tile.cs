using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Tile : IContainer
    {
        private RecomputableSource recomputableSource = new RecomputableSource();

        private Recomputable<Item> ground;

        private Recomputable<Item> topItem;

        private Recomputable<Creature> topCreature;

        private Recomputable<FloorChange> floorChange;

        private Recomputable<int> height;

        private Recomputable<bool> canUseWith;

        private Recomputable<bool> canWalk;

        public Tile(Position position)
        {
            this.position = position;

            ground = new Recomputable<Item>(recomputableSource, () =>
            {
                return GetItems().Where(i => i.TopOrder == TopOrder.Ground).LastOrDefault();
            } );

            topItem = new Recomputable<Item>(recomputableSource, () =>
            {
                return GetItems().Where(i => i.TopOrder == TopOrder.Other).FirstOrDefault() ??

                       GetItems().Where(i => i.TopOrder == TopOrder.HighPriority || i.TopOrder == TopOrder.MediumPriority || i.TopOrder == TopOrder.LowPriority).LastOrDefault();
            } );

            topCreature = new Recomputable<Creature>(recomputableSource, () =>
            {
                return GetCreatures().LastOrDefault();
            } );

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

            canUseWith = new Recomputable<bool>(recomputableSource, () =>
            {
                return !GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) );
            } );

            canWalk = new Recomputable<bool>(recomputableSource, () =>
            {
                return !GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) && !GetCreatures().Any(c => c.Block);
            } );           
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

        public Item Ground
        {
            get
            {
                return ground.Value;
            }
        }

        public Item TopItem
        {
            get
            {
                return topItem.Value;
            }
        }

        public Creature TopCreature
        {
            get
            {
                return topCreature.Value;
            }
        }

        public FloorChange FloorChange
        {
            get
            {
                return floorChange.Value;
            }
        }

        public int Height
        {
            get
            {
                return height.Value;
            }
        }

        public bool CanUseWith
        {
            get
            {
                return canUseWith.Value;
            }
        }

        public bool CanWalk
        {
            get
            {
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
            recomputableSource.Change();

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
            recomputableSource.Change();

            IContent oldContent = GetContent(index);

            contents[index] = content;

            oldContent.Parent = null;

            content.Parent = this;
        }

        public void RemoveContent(int index)
        {
            recomputableSource.Change();

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