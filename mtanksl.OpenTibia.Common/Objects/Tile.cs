using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Tile : IContainer
    {
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

        private RecomputableSource recomputableSource;

        private Recomputable recomputable;

        private void EnsureUpdated()
        {
            if (recomputableSource == null)
            {
                recomputableSource = new RecomputableSource();
            }

            if (recomputable == null)
            {
                recomputable = new Recomputable(recomputableSource, () =>
                {
                    ground = null;

                    topItem = null;

                    topCreature = null;

                    floorChange = FloorChange.None;

                    height = 0;

                    blockProjectile = false;

                    notWalkable = false;

                    blockPathFinding = false;

                    block = false;

                    field = false;

                    Item other = null;

                    Item high = null;

                    foreach (var content in GetContents() )
                    {
                        if (content is Item item)
                        {
                            if (item.TopOrder == TopOrder.Ground)
                            {
                                ground = item;
                            }
                            else if (item.TopOrder == TopOrder.HighPriority || item.TopOrder == TopOrder.MediumPriority || item.TopOrder == TopOrder.LowPriority)
                            {
                                high = item;
                            }
                            else if (item.TopOrder == TopOrder.Other)
                            {
                                if (other == null)
                                {
                                    other = item;
                                }
                            }

                            if (item.Metadata.FloorChange != null)
                            {
                                floorChange |= item.Metadata.FloorChange.Value;
                            }

                            if (item.Metadata.Flags.Is(ItemMetadataFlags.HasHeight) )
                            {
                                height++;
                            }

                            if (item.Metadata.Flags.Is(ItemMetadataFlags.BlockProjectile) )
                            {
                                blockProjectile = true;
                            }

                            if (item.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) )
                            {
                                notWalkable = true;
                            }

                            if (item.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) )
                            {
                                blockPathFinding = true;
                            }

                            if (fields.Contains(item.Metadata.OpenTibiaId) )
                            {
                                field = true;
                            }
                        }
                        else if (content is Creature creature)
                        {
                            topCreature = creature;

                            if (creature.Block)
                            {
                                block = true;
                            }
                        }                        
                    }

                    topItem = other ?? high;
                } );
            }

            recomputable.EnsureUpdated();
        }

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

        private Item ground;

        public Item Ground
        {
            get
            {
                EnsureUpdated();

                return ground;
            }
        }

        private Item topItem;

        public Item TopItem
        {
            get
            {
                EnsureUpdated();

                return topItem;
            }
        }

        private Creature topCreature;

        public Creature TopCreature
        {
            get
            {
                EnsureUpdated();

                return topCreature;
            }
        }

        private FloorChange floorChange;

        public FloorChange FloorChange
        {
            get
            {
                EnsureUpdated();

                return floorChange;
            }
        }

        private int height;

        public int Height
        {
            get
            {
                EnsureUpdated();

                return height;
            }
        }

        private bool blockProjectile;

        public bool BlockProjectile
        {
            get
            {
                EnsureUpdated();

                return blockProjectile;
            }
        }

        private bool notWalkable;

        public bool NotWalkable
        {
            get
            {
                EnsureUpdated();

                return notWalkable;
            }
        }

        private bool blockPathFinding;

        public bool BlockPathFinding
        {
            get
            {
                EnsureUpdated();

                return blockPathFinding;
            }
        }

        private bool block;

        public bool Block
        {
            get
            {
                EnsureUpdated();

                return block;
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

        private bool field;

        public bool Field
        {
            get
            {
                EnsureUpdated();

                return field;
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
            IContent content = GetContent(index);

            if (recomputableSource != null)
            {
                recomputableSource.Change();
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