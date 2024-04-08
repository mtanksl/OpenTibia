using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Tile : IContainer
    {
        private RecomputableSource recomputableSourceForItems;

        private RecomputableSource recomputableSourceForCreatures;

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
                    if (recomputableSourceForItems == null)
                    {
                        recomputableSourceForItems = new RecomputableSource();
                    }

                    ground = new Recomputable<Item>(recomputableSourceForItems, () =>
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
                    if (recomputableSourceForItems == null)
                    {
                        recomputableSourceForItems = new RecomputableSource();
                    }

                    topItem = new Recomputable<Item>(recomputableSourceForItems, () =>
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
                    if (recomputableSourceForCreatures == null)
                    {
                        recomputableSourceForCreatures = new RecomputableSource();
                    }

                    topCreature = new Recomputable<Creature>(recomputableSourceForCreatures, () =>
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
                    if (recomputableSourceForItems == null)
                    {
                        recomputableSourceForItems = new RecomputableSource();
                    }

                    floorChange = new Recomputable<FloorChange>(recomputableSourceForItems, () =>
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
                    if (recomputableSourceForItems == null)
                    {
                        recomputableSourceForItems = new RecomputableSource();
                    }

                    height = new Recomputable<int>(recomputableSourceForItems, () =>
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
                    if (recomputableSourceForItems == null)
                    {
                        recomputableSourceForItems = new RecomputableSource();
                    }

                    blockProjectile = new Recomputable<bool>(recomputableSourceForItems, () =>
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
                    if (recomputableSourceForItems == null)
                    {
                        recomputableSourceForItems = new RecomputableSource();
                    }

                    notWalkable = new Recomputable<bool>(recomputableSourceForItems, () =>
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
                    if (recomputableSourceForItems == null)
                    {
                        recomputableSourceForItems = new RecomputableSource();
                    }

                    blockPathFinding = new Recomputable<bool>(recomputableSourceForItems, () =>
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
                    if (recomputableSourceForCreatures == null)
                    {
                        recomputableSourceForCreatures = new RecomputableSource();
                    }

                    block = new Recomputable<bool>(recomputableSourceForCreatures, () =>
                    {
                        return GetCreatures().Any(c => c.Block);
                    } );
                }

                return block.Value;
            }
        }

        public bool CanUseWith
        {
            get
            {
                return !NotWalkable && !BlockPathFinding;
            }
        }

        public bool CanWalk
        {
            get
            {
                return !NotWalkable && !BlockPathFinding && !Block;
            }
        }

        private static HashSet<ushort> fields = new HashSet<ushort>() 
        {             
            // Blades
            1510, 1511,             

            //Campfire
            1423, 1424, 1425,

            // Gate of expertise
            1228, 1230, 1246, 1248, 1260, 1262, 3541, 3550, 5104, 5113, 5122, 5131, 5293, 5295, 6207, 6209, 6264, 6266, 6897, 6906, 7039, 7048, 8556, 8558, 9180, 9182, 9282, 9284, 10283, 10285, 10474, 10483, 10781, 10790,
        
            // Sealed door
            1224, 1226, 1242, 1244, 1256, 1258, 3543, 3552, 5106, 5115, 5124, 5133, 5289, 5291, 5746, 5749, 6203, 6205, 6260, 6262, 6899, 6908, 7041, 7050, 8552, 8554, 9176, 9178, 9278, 9280, 10279, 10281, 10476, 10485, 10783, 10792,
            
            // Energy field
            1491, 1495,

            // Fire field
            1487, 1488, 1492, 1493,

            // Jungle maw
            4208, 4209,

            // Open trap
            2579,

            // Poison field
            1490, 1496, 8062,

            // Searing fire
            1506, 1507,

            // Spikes
            1512, 1513
        };

        private Recomputable<bool> field;

        public bool Field
        {
            get
            {
                if (field == null)
                {
                    if (recomputableSourceForItems == null)
                    {
                        recomputableSourceForItems = new RecomputableSource();
                    }

                    field = new Recomputable<bool>(recomputableSourceForItems, () =>
                    {
                        return GetItems().Any(i => fields.Contains(i.Metadata.OpenTibiaId) );
                    } );
                }

                return field.Value;
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
            if (content is Item)
            {
                if (recomputableSourceForItems != null)
                {
                    recomputableSourceForItems.Change();
                }
            }
            else if (content is Creature)
            {
                if (recomputableSourceForCreatures != null)
                {
                    recomputableSourceForCreatures.Change();
                }
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
            if (content is Item)
            {
                if (recomputableSourceForItems != null)
                {
                    recomputableSourceForItems.Change();
                }
            }
            else if (content is Creature)
            {
                if (recomputableSourceForCreatures != null)
                {
                    recomputableSourceForCreatures.Change();
                }
            }

            IContent oldContent = GetContent(index);

            contents[index] = content;

            oldContent.Parent = null;

            content.Parent = this;
        }

        public void RemoveContent(int index)
        {
            IContent content = GetContent(index);

            if (content is Item)
            {
                if (recomputableSourceForItems != null)
                {
                    recomputableSourceForItems.Change();
                }
            }
            else if (content is Creature)
            {
                if (recomputableSourceForCreatures != null)
                {
                    recomputableSourceForCreatures.Change();
                }
            }

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