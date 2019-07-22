using System;
using System.Collections.Generic;

namespace OpenTibia.Threading
{
    public class PriorityQueue<TKey, TValue> where TValue : IComparable<TValue>
    {
        private PriorityQueue<TValue> queue = new PriorityQueue<TValue>();

        private Dictionary<TKey, TValue> dictionary = new Dictionary<TKey, TValue>();

        private Func<TValue, TKey> keySelector;

        public PriorityQueue(Func<TValue, TKey> keySelector)
        {
            this.keySelector = keySelector;
        }

        public int Count
        {
            get
            {
                return queue.Count;
            }
        }

        public void Enqueue(TValue value)
        {
            queue.Enqueue(value);

            dictionary.Add( keySelector(value), value );
        }

        public TValue Dequeue()
        {
            TValue value = queue.Dequeue();

            dictionary.Remove( keySelector(value) );

            return value;
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return dictionary.TryGetValue(key, out value);
        }
    }
}