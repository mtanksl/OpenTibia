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

        public static string Print(this byte[] bytes)
        {
            StringBuilder builder = new StringBuilder();

            foreach (var b in bytes)
            {
                builder.Append(b.ToString("X2") + " ");
            }

            return builder.ToString();
        }
    }
}