using System;
using System.Collections.Generic;

namespace OpenTibia
{
    public static class IEnumerableExtensions
    {
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
        {
            return source.MinBy(selector, Comparer<TKey>.Default);
        }

        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector, IComparer<TKey> comparer)
        {
            using (var sourceIterator = source.GetEnumerator() )
            {
                if ( !sourceIterator.MoveNext() )
                {
                    throw new InvalidOperationException();
                }

                TSource minValue = sourceIterator.Current;

                TKey minKey = selector(minValue);

                while ( sourceIterator.MoveNext() )
                {
                    TSource value = sourceIterator.Current;

                    TKey key = selector(value);

                    if (comparer.Compare(key, minKey) < 0)
                    {
                        minValue = value;

                        minKey = key;
                    }
                }

                return minValue;
            }
        }

        public static IEnumerable<TSource> TakeUntil<TSource>(this IEnumerable<TSource> source, Func<TSource, bool> predicate)
        {
            foreach (TSource value in source)
            {
                yield return value;

                if ( predicate(value) )
                {
                    yield break;
                }
            }
        }
    }
}