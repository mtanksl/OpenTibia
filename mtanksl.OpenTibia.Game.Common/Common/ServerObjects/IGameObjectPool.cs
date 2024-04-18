using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IGameObjectPool
    {
        Player GetPlayer(string name);

        void AddPlayer(Player player);

        IEnumerable<Player> GetPlayers();
    }
}