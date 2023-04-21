using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class QueueList<TValue>
    {
        private LinkedList<TValue> queue = new LinkedList<TValue>();

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

            return node.Value;
        }

        public void Add(TValue value)
        {
            queue.AddLast(value);
        }

        public bool Remove(TValue value)
        {
            return queue.Remove(value);
        }
    }
}