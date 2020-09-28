using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class GameObjectCollection
    {
        private Server server;

        public GameObjectCollection(Server server)
        {
            this.server = server;
        }

        private Dictionary<Type, Dictionary<uint, GameObject>> buckets = new Dictionary<Type, Dictionary<uint, GameObject>>()
        {
            { typeof(Creature), new Dictionary<uint, GameObject>() },

            { typeof(Monster), new Dictionary<uint, GameObject>() },

            { typeof(Npc), new Dictionary<uint, GameObject>() },

            { typeof(Player), new Dictionary<uint, GameObject>() },

            { typeof(Item), new Dictionary<uint, GameObject>() }
        };

        private uint uniqueId = 0;

        private uint GenerateId()
        {
            uniqueId++;

            return uniqueId;
        }

        public void AddGameObject(GameObject gameObject)
        {
            if (gameObject.Id == 0)
            {
                gameObject.Id = GenerateId();
            }

            if (gameObject is Creature)
            {
                buckets[ typeof(Creature) ].Add(gameObject.Id, gameObject);
            }

            if (gameObject is Monster)
            {
                buckets[ typeof(Monster) ].Add(gameObject.Id, gameObject);
            }
            
            if (gameObject is Npc)
            {
                buckets[ typeof(Npc) ].Add(gameObject.Id, gameObject);
            }
            
            if (gameObject is Player)
            {
                buckets[ typeof(Player) ].Add(gameObject.Id, gameObject);
            }
            
            if (gameObject is Item)
            {
                buckets[ typeof(Item) ].Add(gameObject.Id, gameObject);
            }

            foreach (var component in gameObject.GetComponents<Behaviour>() )
            {
                component.Start(server);
            }
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            foreach (var component in gameObject.GetComponents<Behaviour>() )
            {
                component.Stop(server);
            }

            if (gameObject is Creature)
            {
                buckets[ typeof(Creature) ].Remove(gameObject.Id);
            }

            if (gameObject is Monster)
            {
                buckets[ typeof(Monster) ].Remove(gameObject.Id);
            }
            
            if (gameObject is Npc)
            {
                buckets[ typeof(Npc) ].Remove(gameObject.Id);
            }
            
            if (gameObject is Player)
            {
                buckets[ typeof(Player) ].Remove(gameObject.Id);
            }
            
            if (gameObject is Item)
            {
                buckets[ typeof(Item) ].Remove(gameObject.Id);
            }
        }

        public IEnumerable<T> GetGameObjects<T>() where T : GameObject
        {
            Dictionary<uint, GameObject> gameObjects;

            if ( buckets.TryGetValue(typeof(T), out gameObjects) )
            {
                return gameObjects.Values.Cast<T>();
            }

            return Enumerable.Empty<T>();
        }

        public T GetGameObject<T>(uint id) where T : GameObject
        {
            Dictionary<uint, GameObject> gameObjects;

            if ( buckets.TryGetValue(typeof(T), out gameObjects) )
            {
                GameObject gameObject;

                if (gameObjects.TryGetValue(id, out gameObject) )
                {
                    return (T)gameObject;
                }
            }

            return default(T);
        }

        public IEnumerable<Creature> GetCreatures()
        {
            return GetGameObjects<Creature>();
        }

        public IEnumerable<Monster> GetMonsters()
        {
            return GetGameObjects<Monster>();
        }

        public IEnumerable<Npc> GetNpcs()
        {
            return GetGameObjects<Npc>();
        }

        public IEnumerable<Player> GetPlayers()
        {
            return GetGameObjects<Player>();
        }

        public IEnumerable<Item> GetItems()
        {
            return GetGameObjects<Item>();
        }
    }
}