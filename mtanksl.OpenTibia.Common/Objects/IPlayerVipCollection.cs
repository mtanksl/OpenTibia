using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IPlayerVipCollection
    {
        bool TryGetVip(int databasePlayerId, out string name);

        bool AddVip(int databasePlayerId, string name);

        void RemoveVip(int databasePlayerId);

        IEnumerable< KeyValuePair<int, string> > GetIndexed();
    }
}