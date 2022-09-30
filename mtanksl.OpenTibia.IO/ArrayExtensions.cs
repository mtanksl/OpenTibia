using System;
using System.Text;

namespace OpenTibia.IO
{
    public static class ArrayExtensions
    {
        public static T[] Combine<T>(this T[] first, T[] second)
        {
            T[] result = new T[first.Length + second.Length];

            Buffer.BlockCopy(first, 0, result, 0, first.Length);

            Buffer.BlockCopy(second, 0, result, first.Length, second.Length);

            return result;
        }

        private static Random random = new Random();

        public static T Random<T>(this T[] array)
        {
            return array[ random.Next(0, array.Length) ];
        }

        public static T[] Shuffle<T>(this T[] array)
        {
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

        public static string Print(this byte[] bytes)
        {
            var builder = new StringBuilder();

            foreach (var b in bytes)
            {
                builder.Append(b.ToString("X2") + " ");
            }

            return builder.ToString();
        }
    }
}