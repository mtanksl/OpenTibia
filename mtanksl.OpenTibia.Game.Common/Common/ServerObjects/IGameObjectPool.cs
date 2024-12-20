using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IGameObjectPool
    {
        void AddPlayer(Player player);

        IEnumerable<Player> GetPlayers();

        Player GetPlayerByName(string name);
    }
}