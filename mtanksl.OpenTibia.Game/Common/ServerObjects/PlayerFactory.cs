using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
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

            scripts = new Dictionary<string, GameObjectScript<string, Player> >();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(GameObjectScript<string, Player>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                GameObjectScript<string, Player> script = (GameObjectScript<string, Player>)Activator.CreateInstance(type);

                scripts.Add(script.Key, script);
            }
        }

        private Dictionary<string, GameObjectScript<string, Player> > scripts;

        public GameObjectScript<string, Player> GetPlayerScript(string name)
        {
            GameObjectScript<string, Player> script;

            if (scripts.TryGetValue(name, out script) )
            {
                return script;
            }
            
            if (scripts.TryGetValue("", out script) )
            {
                return script;
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

            GameObjectScript<string, Player> script = GetPlayerScript(player.Name);

            if (script != null)
            {
                script.Start(player);
            }

            return player;
        }

        public bool Detach(Player player)
        {
            if (server.GameObjects.RemoveGameObject(player) )
            {
                GameObjectScript<string, Player> script = GetPlayerScript(player.Name);

                if (script != null)
                {
                    script.Stop(player);
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