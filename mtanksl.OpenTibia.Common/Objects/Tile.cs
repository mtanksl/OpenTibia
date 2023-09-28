using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
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

        public bool ProtectionZone { get; set; }

        public Item Ground
        {
            get
            {
                return GetItems().Where(i => i.TopOrder == TopOrder.Ground).FirstOrDefault();
            }
        }

        public Item TopItem
        {
            get
            {
                return GetItems().Where(i => i.TopOrder == TopOrder.Other).FirstOrDefault() ??

                       GetItems().Where(i => i.TopOrder == TopOrder.HighPriority || i.TopOrder == TopOrder.MediumPriority || i.TopOrder == TopOrder.LowPriority).LastOrDefault();
            }
        }

        public Creature TopCreature
        {
            get
            {
                return GetCreatures().LastOrDefault();
            }
        }

        public FloorChange FloorChange
        {
            get
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
            }
        }

        public int Height
        {
            get
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
            }
        }

        public int Count
        {
            get
            {
                return contents.Count;
            }
        }

        private List<IContent> contents = new List<IContent>(1);

        public byte AddContent(IContent content)
        {
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

            byte index = 0;

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

        public void AddContent(IContent content, byte index)
        {
            throw new NotSupportedException();
        }

        public void ReplaceContent(byte index, IContent content)
        {
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

        public bool TryGetIndex(IContent content, out byte _index)
        {
            for (byte index = 0; index < contents.Count; index++)
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