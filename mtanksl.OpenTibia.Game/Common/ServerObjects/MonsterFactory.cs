using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.Game.GameObjectScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Monster = OpenTibia.Common.Objects.Monster;

namespace OpenTibia.Game
{
    public class MonsterFactory
    {
        private Server server;

        public MonsterFactory(Server server, MonsterFile monsterFile)
        {
            this.server = server;

            metadatas = new Dictionary<string, MonsterMetadata>(monsterFile.Monsters.Count);

            foreach (var xmlMonster in monsterFile.Monsters)
            {
                metadatas.Add(xmlMonster.Name, new MonsterMetadata()
                {
                    Name = xmlMonster.Name,

                    Speed = (ushort)xmlMonster.Speed,

                    Description = xmlMonster.NameDescription,

                    Health = (ushort)xmlMonster.Health.Now,

                    MaxHealth = (ushort)xmlMonster.Health.Max,

                    Outfit = xmlMonster.Look.TypeEx != 0 ? new Outfit(xmlMonster.Look.TypeEx) : new Outfit(xmlMonster.Look.Type, xmlMonster.Look.Head, xmlMonster.Look.Body, xmlMonster.Look.Legs, xmlMonster.Look.Feet, Addon.None),

                    Corpse = (ushort)xmlMonster.Look.Corpse,

                    Sentences = xmlMonster.Voices?.Select(v => v.Sentence).ToArray()
                } );
            }

            scripts = new Dictionary<string, GameObjectScript<string, Monster> >();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(GameObjectScript<string, Monster>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                GameObjectScript<string, Monster> script = (GameObjectScript<string, Monster>)Activator.CreateInstance(type);

                scripts.Add(script.Key, script);
            }
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

        private Dictionary<string, GameObjectScript<string, Monster> > scripts;

        public GameObjectScript<string, Monster> GetMonsterScript(string name)
        {
            GameObjectScript<string, Monster> script;

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

        public Monster Create(string name)
        {
            MonsterMetadata metadata = GetMonsterMetadata(name);

            if (metadata == null)
            {
                return null;
            }

            Monster monster = new Monster(metadata);
                        
            server.GameObjects.AddGameObject(monster);

            GameObjectScript<string, Monster> script = GetMonsterScript(monster.Name);

            if (script != null)
            {
                script.Start(monster);
            }

            return monster;
        }

        public bool Detach(Monster monster)
        {
            if (server.GameObjects.RemoveGameObject(monster) )
            {
                GameObjectScript<string, Monster> script = GetMonsterScript(monster.Name);

                if (script != null)
                {
                    script.Stop(monster);
                }

                return true;
            }

            return false;
        }

        public void Destroy(Monster monster)
        {
            server.GameObjectComponents.ClearComponents(monster);

            server.GameObjectEventHandlers.ClearEventHandlers(monster);
        }
    }
}