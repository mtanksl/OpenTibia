using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class LockerCollection
    {
        private Dictionary<int, Safe> safes = new Dictionary<int, Safe>();

        public void AddLocker(int databasePlayerId, Locker locker)
        {
            Safe safe;

            if ( !safes.TryGetValue(databasePlayerId, out safe) )
            {
                safe = new Safe(databasePlayerId);

                safes.Add(databasePlayerId, safe);
            }

            safe.AddContent(locker, locker.TownId);
        }

        public Locker GetLocker(int databasePlayerId, ushort townId)
        {
            Safe safe;

            if ( safes.TryGetValue(databasePlayerId, out safe) )
            {
                return (Locker)safe.GetContent(townId);
            }

            return null;
        }

        public IEnumerable<KeyValuePair<ushort, Locker>> GetIndexedLockers(int databasePlayerId)
        {
            Safe safe;

            if ( safes.TryGetValue(databasePlayerId, out safe) )
            {
                foreach (var pair in safe.GetIndexedContents() )
                {
                    yield return new KeyValuePair<ushort, Locker>( (ushort)pair.Key, (Locker)pair.Value);
                }
            }
        }
    }
}