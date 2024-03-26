using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class LockerCollection
    {
        private Dictionary<int, Dictionary<ushort, Locker>> items = new Dictionary<int, Dictionary<ushort, Locker>>();

        public void AddLocker(int databasePlayerId, Locker locker)
        {
            Dictionary<ushort, Locker> item;

            if ( !items.TryGetValue(databasePlayerId, out item) )
            {
                item = new Dictionary<ushort, Locker>();

                items.Add(databasePlayerId, item);
            }

            item[locker.TownId] = locker;
        }

        public Locker GetLocker(int databasePlayerId, ushort townId)
        {
            Dictionary<ushort, Locker> item;

            if ( items.TryGetValue(databasePlayerId, out item) )
            {
                Locker locker;

                item.TryGetValue(townId, out locker);

                return locker;
            }

            return null;
        }

        public IEnumerable< KeyValuePair<ushort, Locker> > GetIndexedLockers(int databasePlayerId)
        {
            Dictionary<ushort, Locker> item;

            if ( items.TryGetValue(databasePlayerId, out item) )
            {
                foreach (var pair in item)
                {
                    yield return new KeyValuePair<ushort, Locker>(pair.Key, pair.Value);
                }
            }
        }
    }
}