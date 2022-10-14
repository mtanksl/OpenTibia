using OpenTibia.Common.Objects;
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

        private Dictionary<Type, Dictionary<uint, GameObject> > buckets = new Dictionary<Type, Dictionary<uint, GameObject> >()
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

                if (gameObject is Monster)
                {
                    buckets[ typeof(Monster) ].Add(gameObject.Id, gameObject);
                }
                else if (gameObject is Npc)
                {
                    buckets[ typeof(Npc) ].Add(gameObject.Id, gameObject);
                }
                else if (gameObject is Player)
                {
                    buckets[ typeof(Player) ].Add(gameObject.Id, gameObject);
                }
            }
            else if (gameObject is Item)
            {
                buckets[ typeof(Item) ].Add(gameObject.Id, gameObject);
            }
        }

        public void RemoveGameObject(GameObject gameObject)
        {
            if (gameObject is Creature)
            {
                buckets[ typeof(Creature) ].Remove(gameObject.Id);

                if (gameObject is Monster)
                {
                    buckets[ typeof(Monster) ].Remove(gameObject.Id);
                }
                else if (gameObject is Npc)
                {
                    buckets[ typeof(Npc) ].Remove(gameObject.Id);
                }
                else if (gameObject is Player)
                {
                    buckets[ typeof(Player) ].Remove(gameObject.Id);
                }
            }
            else if (gameObject is Item)
            {
                buckets[ typeof(Item) ].Remove(gameObject.Id);
            }
        }

        private T GetGameObject<T>(uint id) where T : GameObject
        {
            Dictionary<uint, GameObject> gameObjects;

            if (buckets.TryGetValue(typeof(T), out gameObjects) )
            {
                GameObject gameObject;

                if (gameObjects.TryGetValue(id, out gameObject) )
                {
                    return (T)gameObject;
                }
            }

            return default(T);
        }

        public Creature GetCreature(uint id)
        {
            return GetGameObject<Creature>(id);
        }

        public Monster GetMonster(uint id)
        {
            return GetGameObject<Monster>(id);
        }

        public Npc GetNpc(uint id)
        {
            return GetGameObject<Npc>(id);
        }

        public Player GetPlayer(uint id)
        {
            return GetGameObject<Player>(id);
        }

        public Item GetItem(uint id)
        {
            return GetGameObject<Item>(id);
        }

        private IEnumerable<T> GetGameObjects<T>() where T : GameObject
        {
            Dictionary<uint, GameObject> gameObjects;

            if (buckets.TryGetValue(typeof(T), out gameObjects) )
            {
                return gameObjects.Values.Cast<T>();
            }

            return Enumerable.Empty<T>();
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