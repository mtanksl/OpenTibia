using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Inventory : IContainer
    {
        public Inventory(Player player)
        {
            this.player = player;
        }

        private Player player;

        public Player Player
        {
            get
            {
                return player;
            }
        }

        private IContent[] contents = new IContent[11];

        /// <exception cref="NotSupportedException"></exception>

        public byte AddContent(IContent content)
        {
            throw new NotSupportedException();
        }

        /// <exception cref="ArgumentException"></exception>
        
        public void AddContent(IContent content, byte index)
        {
            if ( !(content is Item) )
            {
                throw new ArgumentException("Content must be an item.");
            }

            contents[index] = content;

            content.Parent = this;
        }

        /// <exception cref="ArgumentException"></exception>

        public void ReplaceContent(byte index, IContent content)
        {
            if ( !(content is Item) )
            {
                throw new ArgumentException("Content must be an item.");
            }

            IContent oldContent = GetContent(index);

            contents[index] = content;

            oldContent.Parent = null;

            content.Parent = this;
        }

        public void RemoveContent(byte index)
        {
            IContent content = GetContent(index);

            contents[index] = null;

            content.Parent = null;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public byte GetIndex(IContent content)
        {
            for (byte index = 0; index < contents.Length; index++)
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
            for (byte index = 0; index < contents.Length; index++)
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
            if (index < 0 || index > contents.Length - 1)
            {
                return null;
            }

            return contents[index];
        }

        public IEnumerable<IContent> GetContents()
        {
            foreach (var content in contents)
            {
                if (content != null)
                {
                    yield return content;
                }
            }
        }

        public IEnumerable< KeyValuePair<byte, IContent> > GetIndexedContents()
        {
            for (byte index = 0; index < contents.Length; index++)
            {
                if (contents[index] != null)
                {
                    yield return new KeyValuePair<byte, IContent>( index, contents[index] );
                }
            }
        }

        public IEnumerable<Item> GetItems()
        {
            return GetContents().OfType<Item>();
        }
    }
}