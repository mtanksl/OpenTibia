using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class GameObjectPool
    {
        private Dictionary<string, Player> players = new Dictionary<string, Player>();

        public Player GetPlayer(string name)
        {
            Player player;

            players.TryGetValue(name, out player);

            return player;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player.Name, player);
        }
    }
}