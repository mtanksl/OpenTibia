using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class GameObjectPool : IGameObjectPool
    {
        private IServer server;

        public GameObjectPool(IServer server)
        {
            this.server = server;
        }

        private Dictionary<string, string> names = new Dictionary<string, string>();

        private Dictionary<int, int> ids = new Dictionary<int, int>();

        public string GetPlayerNameFor(string ipAddress, int databasePlayerId, string playerName)
        {
            if (server.Config.GameplayAllowClones || (server.Config.LoginAccountManagerEnabled && playerName == server.Config.LoginAccountManagerPlayerName) )
            {
                if ( !names.TryGetValue(ipAddress + "_" + databasePlayerId, out var name) )
                {
                    if ( !ids.TryGetValue(databasePlayerId, out var id) )
                    {
                        id = 1;

                        ids.Add(databasePlayerId, id);
                    }
                    else
                    {
                        id = ids[databasePlayerId] + 1;

                        ids[databasePlayerId] = id;
                    }

                    name = playerName + " " + id;

                    names.Add(ipAddress + "_" + databasePlayerId, name);
                }

                return name;
            }

            return playerName;
        }

        private Dictionary<string, Player> players = new Dictionary<string, Player>();

        public void AddPlayer(Player player)
        {
            players.Add(player.Name, player);
        }

        public IEnumerable<Player> GetPlayers() 
        { 
            return players.Values;
        }

        public Player GetPlayerByName(string name)
        {
            Player player;

            players.TryGetValue(name, out player);

            return player;
        }
    }
}