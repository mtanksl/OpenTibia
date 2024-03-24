using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class LockerCollection
    {
        private class Safe
        {
            private Dictionary<ushort, Locker> lockers = new Dictionary<ushort, Locker>();

            public void AddLocker(ushort townId, Locker locker)
            {
                lockers[townId] = locker;
            }

            public Locker GetLocker(ushort townId)
            {
                Locker locker;

                lockers.TryGetValue(townId, out locker);

                return locker;
            }

            public IEnumerable< KeyValuePair<ushort, Locker> > GetIndexedLockers()
            {
                return lockers;
            }
        }

        private Dictionary<int, Safe> safes = new Dictionary<int, Safe>();

        public void AddLocker(int databasePlayerId, Locker locker)
        {
            Safe safe;

            if ( !safes.TryGetValue(databasePlayerId, out safe) )
            {
                safe = new Safe();

                safes.Add(databasePlayerId, safe);
            }

            safe.AddLocker(locker.TownId, locker);
        }

        public Locker GetLocker(int databasePlayerId, ushort townId)
        {
            Safe safe;

            if ( safes.TryGetValue(databasePlayerId, out safe) )
            {
                return safe.GetLocker(townId);
            }

            return null;
        }

        public IEnumerable< KeyValuePair<ushort, Locker> > GetIndexedLockers(int databasePlayerId)
        {
            Safe safe;

            if ( safes.TryGetValue(databasePlayerId, out safe) )
            {
                foreach (var pair in safe.GetIndexedLockers() )
                {
                    yield return new KeyValuePair<ushort, Locker>(pair.Key, pair.Value);
                }
            }
        }
    }
}