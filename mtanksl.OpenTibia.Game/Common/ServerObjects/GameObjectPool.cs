using OpenTibia.Common.Objects;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class GameObjectPool : IGameObjectPool
    {
        private List<Player> players = new List<Player>();

        public void AddPlayer(Player player)
        {
            players.Add(player);
        }

        public IEnumerable<Player> GetPlayers() 
        { 
            return players;
        }

        public Player GetPlayerByName(string name)
        {
            return GetPlayers()
                .Where(p => p.Name == name)
                .FirstOrDefault();
        }
    }
}