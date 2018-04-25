using System;
using System.Linq;
using System.Collections.Generic;

namespace OpenTibia
{
    public class Slots : IContainer
    {
        private IContent[] contents = new IContent[11];

        public int AddContent(IContent content)
        {
            throw new NotSupportedException();
        }

        public void AddContent(int index, IContent content)
        {
            if (content.Container != null)
            {
                throw new Exception("Content already attached");
            }

            contents[index] = content;

            content.Container = this;
        }
        
        public int RemoveContent(IContent content)
        {
            if (content.Container == null)
            {
                throw new Exception("Content already dettached");
            }

            int index = GetIndex(content);

            contents[index] = null;

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
            for (int index = 0; index < contents.Length; index++)
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