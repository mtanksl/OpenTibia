using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class GameObjectPool : IGameObjectPool
    {
        private Dictionary<string, Player> players = new Dictionary<string, Player>();

        public Player GetPlayerByName(string name)
        {
            Player player;

            players.TryGetValue(name, out player);

            return player;
        }

        public void AddPlayer(Player player)
        {
            players.Add(player.Name, player);
        }

        public IEnumerable<Player> GetPlayers() 
        { 
            return players.Values;
        }
    }
}