using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class Safe : IContainer
    {
        public Safe(Player player)
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
                throw new ArgumentException("Content must be a locker.");
            }

            contents[index] = content;

            content.Parent = this;
        }

        /// <exception cref="ArgumentException"></exception>

        public void ReplaceContent(int index, IContent content)
        {
            if ( !(content is Locker) )
            {
                throw new ArgumentException("Content must be a locker.");
            }

            IContent oldContent = GetContent(index);

            contents[index] = content;

            oldContent.Parent = null;

            content.Parent = this;
        }

        public void RemoveContent(int index)
        {
            IContent content = GetContent(index);

            contents.Remove(index);

            content.Parent = null;
        }

        /// <exception cref="InvalidOperationException"></exception>

        public int GetIndex(IContent content)
        {
            if ( !(content is Locker) )
            {
                throw new ArgumentException("Content must be a locker.");
            }

            if (contents.ContainsKey( ( (Locker)content ).TownId) )
            {
                return ( (Locker)content ).TownId;
            }

            throw new InvalidOperationException("Content not found.");
        }

        public bool TryGetIndex(IContent content, out int _index)
        {
            if ( !(content is Locker) )
            {
                throw new ArgumentException("Content must be a locker.");
            }

            if (contents.ContainsKey( ( (Locker)content ).TownId) )
            {
                _index = ( (Locker)content ).TownId;

                return true;
            }

            _index = 0;

            return false;
        }

        public IContent GetContent(int index)
        {
            IContent content;

            contents.TryGetValue(index, out content); 

            return content;
        }

        public IEnumerable<IContent> GetContents()
        {
            return contents.Values;
        }

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