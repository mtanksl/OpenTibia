using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.Game.GameObjectScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using Monster = OpenTibia.Common.Objects.Monster;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class MonsterFactory : IMonsterFactory
    {
        private IServer server;

        public MonsterFactory(IServer server)
        {
            this.server = server;
        }

        public void Start(MonsterFile monsterFile)
        {
            metadatas = new Dictionary<string, MonsterMetadata>(monsterFile.Monsters.Count);

            foreach (var xmlMonster in monsterFile.Monsters)
            {
                metadatas.Add(xmlMonster.Name, new MonsterMetadata()
                {
                    Name = xmlMonster.Name,

                    Description = xmlMonster.NameDescription,

                    Speed = (ushort)xmlMonster.Speed,

                    Health = (ushort)xmlMonster.Health.Now,

                    MaxHealth = (ushort)xmlMonster.Health.Max,

                    Outfit = xmlMonster.Look.TypeEx != 0 ? new Outfit(xmlMonster.Look.TypeEx) : new Outfit(xmlMonster.Look.Type, xmlMonster.Look.Head, xmlMonster.Look.Body, xmlMonster.Look.Legs, xmlMonster.Look.Feet, Addon.None),

                    Corpse = (ushort)xmlMonster.Look.Corpse,

                    Sentences = xmlMonster.Voices?.Select(v => v.Sentence).ToArray()
                } );
            }

            gameObjectScripts = new Dictionary<string, GameObjectScript<string, Monster> >();
#if AOT
            foreach (var gameObjectScript in _AotCompilation.Monsters)
            {
                gameObjectScripts.Add(gameObjectScript.Key, gameObjectScript);
            }
#else
            foreach (var type in server.PluginLoader.GetTypes(typeof(GameObjectScript<string, Monster>) ) )
            {
                GameObjectScript<string, Monster> gameObjectScript = (GameObjectScript<string, Monster>)Activator.CreateInstance(type);

                gameObjectScripts.Add(gameObjectScript.Key, gameObjectScript);
            }
#endif
        }

        private Dictionary<string, MonsterMetadata> metadatas;

        public MonsterMetadata GetMonsterMetadata(string name)
        {
            MonsterMetadata metadata;

            if (metadatas.TryGetValue(name, out metadata) )
            {
                return metadata;
            }

            return null;
        }

        private Dictionary<string, GameObjectScript<string, Monster> > gameObjectScripts;

        public GameObjectScript<string, Monster> GetMonsterGameObjectScript(string name)
        {
            GameObjectScript<string, Monster> gameObjectScript;

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

        public Monster Create(string name, Tile spawn)
        {
            MonsterMetadata metadata = GetMonsterMetadata(name);

            if (metadata == null)
            {
                return null;
            }

            Monster monster = new Monster(metadata)
            {
                Town = spawn,

                Spawn = spawn
            };

            return monster;
        }

        public void Attach(Monster monster)
        {
            monster.IsDestroyed = false;

            server.GameObjects.AddGameObject(monster);

            GameObjectScript<string, Monster> gameObjectScript = GetMonsterGameObjectScript(monster.Name);

            if (gameObjectScript != null)
            {
                gameObjectScript.Start(monster);
            }
        }

        public bool Detach(Monster monster)
        {
            if (server.GameObjects.RemoveGameObject(monster) )
            {
                GameObjectScript<string, Monster> gameObjectScript = GetMonsterGameObjectScript(monster.Name);

                if (gameObjectScript != null)
                {
                    gameObjectScript.Stop(monster);
                }

                return true;
            }

            return false;
        }

        public void ClearComponentsAndEventHandlers(Monster monster)
        {
            server.GameObjectComponents.ClearComponents(monster);

            server.GameObjectEventHandlers.ClearEventHandlers(monster);
        }
    }
}