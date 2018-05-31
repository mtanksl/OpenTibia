using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Inventory : IContainer
    {
        private Player player;

        public Inventory(Player player)
        {
            this.player = player;
        }

        private IContent[] contents = new IContent[11];

        public byte AddContent(IContent content)
        {
            throw new NotSupportedException();
        }

        public void AddContent(byte index, IContent content)
        {
            contents[index] = content;

            content.Container = this;
        }

        public byte RemoveContent(IContent content)
        {
            byte index = GetIndex(content);

            contents[index] = null;

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
            for (byte index = 0; index < contents.Length; index++)
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
            if (index < 0 || index > contents.Length - 1)
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
    }
}