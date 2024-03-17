using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Safe : IContainer
    {
        public Safe(int databasePlayerId)
        {
            this.databasePlayerId = databasePlayerId;
        }

        private int databasePlayerId;

        public int DatabasePlayerId
        {
            get
            {
                return databasePlayerId;
            }
        }

        private Dictionary<int, IContent> contents = new Dictionary<int, IContent>();

        /// <exception cref="NotSupportedException"></exception>

        public int AddContent(IContent content)
        {
            throw new NotSupportedException();
        }

        /// <exception cref="ArgumentException"></exception>

        public void AddContent(IContent content, int index)
        {
            if ( !(content is Locker) )
            {
                throw new ArgumentException("Content must be an item.");
            }

            contents[index] = content;

            content.Parent = this;
        }

        /// <exception cref="NotSupportedException"></exception>

        public void ReplaceContent(int index, IContent content)
        {
            throw new NotSupportedException();
        }

        /// <exception cref="NotSupportedException"></exception>

        public void RemoveContent(int index)
        {
            throw new NotSupportedException();
        }

        /// <exception cref="InvalidOperationException"></exception>

        public int GetIndex(IContent content)
        {
            foreach (var pair in contents)
            {
                if (pair.Value == content)
                {
                    return pair.Key;
                }
            }

            throw new InvalidOperationException("Content not found.");
        }

        public bool TryGetIndex(IContent content, out int _index)
        {
            foreach (var pair in contents)
            {
                if (pair.Value == content)
                {
                    _index = pair.Key;

                    return true;
                }
            }

            _index = 0;

            return false;
        }

        /// <exception cref="NotSupportedException"></exception>

        public IContent GetContent(int index)
        {
            IContent content;

            contents.TryGetValue(index, out content);

            return content;
        }

        /// <exception cref="NotSupportedException"></exception>

        public IEnumerable<IContent> GetContents()
        {
            return contents.Values;
        }

        /// <exception cref="NotSupportedException"></exception>

        public IEnumerable< KeyValuePair<int, IContent> > GetIndexedContents()
        {
            foreach (var pair in contents)
            {
                yield return new KeyValuePair<int, IContent>(pair.Key, pair.Value);
            }
        }

        public IEnumerable<Item> GetItems()
        {
            return GetContents().OfType<Item>();
        }
    }
}