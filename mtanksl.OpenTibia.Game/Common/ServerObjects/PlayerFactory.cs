using OpenTibia.Common.Objects;
using OpenTibia.Game.GameObjectScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game
{
    public class PlayerFactory
    {
        private Server server;

        public PlayerFactory(Server server)
        {
            this.server = server;
        }

        public void Start()
        {
            gameObjectScripts = new Dictionary<string, GameObjectScript<string, Player> >();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(GameObjectScript<string, Player>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                GameObjectScript<string, Player> gameObjectScript = (GameObjectScript<string, Player>)Activator.CreateInstance(type);

                gameObjectScripts.Add(gameObjectScript.Key, gameObjectScript);
            }
        }

        private Dictionary<string, GameObjectScript<string, Player> > gameObjectScripts;

        public GameObjectScript<string, Player> GetPlayerGameObjectScript(string name)
        {
            GameObjectScript<string, Player> gameObjectScript;

            if (gameObjectScripts.TryGetValue(name, out gameObjectScript) )
            {
                return gameObjectScript;
            }
            
            if (gameObjectScripts.TryGetValue("", out gameObjectScript) )
            {
                return gameObjectScript;
            }

            return null;
        }

        public Player Create(IConnection connection, string name, Tile spawn)
        {
            Player player = new Player();

            player.Name = name;

            player.Spawn = spawn;

            Client client = new Client(server);

            client.Connection = connection;

            player.Client = client;

            server.GameObjects.AddGameObject(player);

            GameObjectScript<string, Player> gameObjectScript = GetPlayerGameObjectScript(player.Name);

            if (gameObjectScript != null)
            {
                gameObjectScript.Start(player);
            }

            return player;
        }

        public bool Detach(Player player)
        {
            if (server.GameObjects.RemoveGameObject(player) )
            {
                GameObjectScript<string, Player> gameObjectScript = GetPlayerGameObjectScript(player.Name);

                if (gameObjectScript != null)
                {
                    gameObjectScript.Stop(player);
                }

                return true;
            }

            return false;
        }

        public void Destroy(Player player)
        {
            server.GameObjectComponents.ClearComponents(player);

            server.GameObjectEventHandlers.ClearEventHandlers(player);
        }
    }
}