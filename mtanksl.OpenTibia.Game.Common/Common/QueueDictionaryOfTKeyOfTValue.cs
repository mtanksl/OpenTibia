using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common
{
    public class QueueDictionary<TKey, TValue>
    {
        private LinkedList< Tuple<TKey, TValue> > queue = new LinkedList< Tuple<TKey, TValue> >();

        private Dictionary< TKey, LinkedListNode< Tuple<TKey, TValue> > > dictionary = new Dictionary< TKey, LinkedListNode< Tuple<TKey, TValue> > >();

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        public TValue Peek()
        {
            if (queue.First != null)
            {
                return queue.First.Value.Item2;
            }

            return default(TValue);
        }

        public TValue Dequeue()
        {
            var node = queue.First;

            queue.RemoveFirst();

            dictionary.Remove(node.Value.Item1);

            return node.Value.Item2;
        }

        public bool Add(TKey key, TValue value)
        {
            LinkedListNode<Tuple<TKey, TValue> > node;

            if ( !dictionary.TryGetValue(key, out node) )
            {
                node = queue.AddLast(Tuple.Create(key, value) );

                dictionary.Add(key, node);

                return true;
            }

            return false;
        }

        public bool Remove(TKey key)
        {
            LinkedListNode<Tuple<TKey, TValue> > node;

            if (dictionary.TryGetValue(key, out node) )
            {
                queue.Remove(node);

                dictionary.Remove(node.Value.Item1);

                return true;
            }

            return false;           
        }

        public bool Contains(TKey key)
        {
            return dictionary.ContainsKey(key);
        }
    }
}