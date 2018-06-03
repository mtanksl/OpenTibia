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

        private List<IContent> contents = new List<IContent>();
        
        public byte AddContent(IContent content)
        {
            byte index = 0;

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

        public IEnumerable< KeyValuePair<byte, IContent> > GetIndexedContents()
        {
            for (byte index = 0; index < contents.Count; index++)
            {
                yield return new KeyValuePair<byte, IContent>( index, contents[index] );
            }
        }
    }
}