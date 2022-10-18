using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class LockerCollection
    {
        private Dictionary<int, Dictionary<ushort, Container>> players = new Dictionary<int, Dictionary<ushort, Container>>();

        public void AddLocker(int databasePlayerId, ushort townId, Container container)
        {
            if ( !players.TryGetValue(databasePlayerId, out var towns) )
            {
                towns = new Dictionary<ushort, Container>();

                players.Add(databasePlayerId, towns);
            }

            towns.Add(townId, container);
        }

        public void RemoveLocker(int databasePlayerId, ushort townId)
        {
            if ( players.TryGetValue(databasePlayerId, out var towns) )
            {
                towns.Remove(townId);

                if (towns.Count == 0)
                {
                    players.Remove(databasePlayerId);
                }
            }           
        }

        public Container GetLocker(int databasePlayerId, ushort townId)
        {
            if ( players.TryGetValue(databasePlayerId, out var towns) )
            {
                if ( towns.TryGetValue(townId, out var locker) )
                {
                    return locker;
                }
            }

            return null;
        }

        public IEnumerable<KeyValuePair<ushort, Container>> GetIndexedLockers(int databasePlayerId)
        {
            if ( players.TryGetValue(databasePlayerId, out var towns) )
            {
                foreach (var pair in towns)
                {
                    yield return new KeyValuePair<ushort, Container>(pair.Key, pair.Value);
                }
            }
        }
    }
}