using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class Randomization : IRandomization
    {
        private Random random = new Random();

        /// <exception cref="ArgumentException"></exception>

        public bool HasProbability(double probability)
        {
            if (probability < 0)
            {
                throw new ArgumentException("Chance must be greater then or equals to 0.");
            }

            if (probability == 0)
            {
                return false;
            }

            if (probability == 1)
            {
                return true;
            }

            if (probability > 1)
            {
                probability = 0.9;
            }

            return random.NextDouble() < probability;
        }

        /// <exception cref="ArgumentException"></exception>

        public int Take(int minInclusive, int maxInclusive)
        {
            if (minInclusive < 0)
            {
                throw new ArgumentException("MinInclusive must be greater then or equals to 0.");
            }

            if (maxInclusive < minInclusive)
            {
                throw new ArgumentException("MaxInclusive must be greater then or equals to MinInclusive.");
            }

            if (minInclusive == maxInclusive)
            {
                return minInclusive;
            }

            return random.Next(minInclusive, maxInclusive + 1);
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>

        public T Take<T>(IList<T> array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array) );
            }

            if (array.Count == 0)
            {
                throw new ArgumentException("Array must have at least one item.");
            }

            return array[ random.Next(0, array.Count) ];
        }

        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentException"></exception>

        public T Take<T>(IList<T> array, int[] weights)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array) );
            }

            if (weights == null)
            {
                throw new ArgumentNullException(nameof(weights) );
            }

            if (array.Count == 0)
            {
                throw new ArgumentException("Array must have at least one item.");
            }

            if (array.Count != weights.Length)
            {
                throw new ArgumentException("Array must have the same length of Weights.");
            }

            int value = random.Next(0, weights.Sum() );

            for (int i = 0; i < array.Count; i++)
            {
                if (value < weights[i] )
                {
                    return array[i];
                }

                value -= weights[i];
            }

            throw new NotImplementedException();
        }

        /// <exception cref="ArgumentNullException"></exception>

        public T[] Shuffle<T>(T[] array)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array) );
            }

            int currentIndex = array.Length - 1;

            while (currentIndex > 0)
            {
                int newIndex = random.Next(0, currentIndex + 1);

                if (currentIndex != newIndex)
                {
                    T temp = array[currentIndex]; array[currentIndex] = array[newIndex]; array[newIndex] = temp;
                }

                currentIndex--;
            }

            return array;
        }
    }
}