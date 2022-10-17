using OpenTibia.Game;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class LockerCollection
    {
        private Dictionary<int, Dictionary<ushort, Container>> players = new Dictionary<int, Dictionary<ushort, Container>>();

        public Container GetLocker(Context context, int databasePlayerId, ushort townId)
        {
            if ( !players.TryGetValue(databasePlayerId, out var towns) )
            {
                towns = new Dictionary<ushort, Container>();

                players.Add(databasePlayerId, towns);
            }

            if ( !towns.TryGetValue(townId, out var locker) )
            {
                locker = (Container)context.Server.ItemFactory.Create(2591, 1);

                towns.Add(townId, locker);
            }

            return locker;
        }
    }
}