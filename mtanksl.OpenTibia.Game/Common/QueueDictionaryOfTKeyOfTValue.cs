using System;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class QueueDictionary<TKey, TValue>
    {
        private LinkedList<Tuple<TKey, TValue>> queue = new LinkedList<Tuple<TKey, TValue>>();

        private Dictionary<TKey, LinkedListNode<Tuple<TKey, TValue>>> dictionary = new Dictionary<TKey, LinkedListNode<Tuple<TKey, TValue>>>();

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        public TValue Dequeue()
        {
            var node = queue.First;

            queue.RemoveFirst();

            dictionary.Remove(node.Value.Item1);

            return node.Value.Item2;
        }

        public void Add(TKey key, TValue value)
        {
            var node = queue.AddLast(Tuple.Create(key, value) );

            dictionary.Add(key, node);
        }

        public bool Remove(TKey key)
        {
            if (dictionary.TryGetValue(key, out var node) )
            {
                queue.Remove(node);

                dictionary.Remove(node.Value.Item1);

                return true;
            }

            return false;           
        }
    }
}