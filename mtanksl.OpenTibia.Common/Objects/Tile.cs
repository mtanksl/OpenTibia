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
                return GetItems().Where(item => item.TopOrder == TopOrder.Ground).FirstOrDefault();
            }
        }
        
        private List<IContent> contents = new List<IContent>(1);
        
        public byte AddContent(IContent content)
        {
            //10 Other
            //11 Other
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

            content.Container = this;

            return index;
        }

        public void AddContent(byte index, IContent content)
        {
            throw new NotSupportedException();
        }

        public byte RemoveContent(IContent content)
        {
            byte index = GetIndex(content);

            contents.RemoveAt(index);

            content.Container = null;

            return index;
        }

        public byte ReplaceContent(IContent before, IContent after)
        {
            byte index = GetIndex(before);

            contents[index] = after;

            before.Container = null;

            after.Container = this;

            return index;
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

            return 255;
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

        public IEnumerable< KeyValuePair<byte, IContent> > GetIndexedContents()
        {
            for (byte index = 0; index < contents.Count; index++)
            {
                yield return new KeyValuePair<byte, IContent>( index, contents[index] );
            }
        }
    }
}