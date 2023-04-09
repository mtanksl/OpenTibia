using System;
using System.Collections;
using System.Collections.Generic;

namespace OpenTibia.Threading
{
    //Reference: https://algs4.cs.princeton.edu/24pq

    public class PriorityQueue<T> : IEnumerable<T> where T : IComparable<T>
    {
        private List<T> items = new List<T>();

        public T this[int index]
        {
            get
            {
                return items[index];
            }
        }

        public int Count
        {
            get
            {
                return items.Count;
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return items.GetEnumerator();
        }

               IEnumerator IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
        
        public T Peek()
        {
            return items[0];
        }

        public void Enqueue(T item)
        {
            items.Add(item);

            SwimUp(items.Count - 1);
        }

        public T Dequeue()
        {
            T item = items[0];

            items[0] = items[items.Count - 1];

            items.RemoveAt(items.Count - 1);

            SinkDown(0);

            return item;
        }

        public T RemoveAt(int index)
        {
            T item = items[index];

            items.RemoveAt(index);

            for (int i = (items.Count / 2 - 1); i >= 0; i--)
            {
                SinkDown(i);
            }

            return item;
        }

        private void SwimUp(int childIndex)
        {
            while (childIndex > 0)
            {
                int parentIndex = (childIndex - 1) / 2;

                if (items[parentIndex].CompareTo(items[childIndex] ) > 0)
                {
                    var temp = items[childIndex]; items[childIndex] = items[parentIndex]; items[parentIndex] = temp;

                    childIndex = parentIndex;
                }
                else
                {
                    break;
                }
            }
        }

        private void SinkDown(int parentIndex)
        {
            while (true)
            {
                int childIndex = parentIndex;
                
                int childIndexA = 2 * parentIndex + 1;

                if (childIndexA < items.Count)
                {
                    if (items[childIndex].CompareTo(items[childIndexA] ) > 0)
                    {
                        childIndex = childIndexA;
                    }
                }

                int childIndexB = 2 * parentIndex + 2;

                if (childIndexB < items.Count)
                {
                    if (items[childIndex].CompareTo(items[childIndexB] ) > 0)
                    {
                        childIndex = childIndexB;
                    }
                }

                if (parentIndex != childIndex)
                {
                    var temp = items[childIndex]; items[childIndex] = items[parentIndex]; items[parentIndex] = temp;

                    parentIndex = childIndex;
                }
                else
                {
                    break;
                }
            }
        }
    }
}