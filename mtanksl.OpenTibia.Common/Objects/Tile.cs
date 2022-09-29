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
                return GetItems().Aggregate(FloorChange.None, (s, i) => s |= i.Metadata.FloorChange);
            }
        }

        private List<IContent> contents = new List<IContent>(1);
        
        public byte AddContent(IContent content)
        {
            //10 Other
            //11 Other
            //12 Other
            //9 Creature
            //8 Creature
            //7 LowPriority
            //6 LowPriority
            //5 MediumPriority
            //4 MediumPriority
            //3 HighPriority
            //2 HighPriority   
            //1 Ground
            //0 Ground

            byte index = 0;
            
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

            content.Parent = this;

            return index;
        }

        /// <exception cref="NotSupportedException"></exception>
        /// 
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
    }
}