using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IGameObjectPool
    {
        Player GetPlayerByName(string name);

        void AddPlayer(Player player);

        IEnumerable<Player> GetPlayers();
    }
}