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
            return Print(bytes, 0, bytes.Length);
        }

        public static string Print(this byte[] bytes, int offset)
        {
            return Print(bytes, offset, bytes.Length - offset);
        }

        public static string Print(this byte[] bytes, int offset, int count)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < count; i++)
            {
                builder.Append(bytes[i + offset].ToString("X2") + " ");
            }

            return builder.ToString();
        }
    }
}