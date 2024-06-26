﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.Game.GameObjectScripts;
using System.Collections.Generic;
using System.Linq;
using LootItem = OpenTibia.Common.Objects.LootItem;
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

                    Experience = xmlMonster.Experience,

                    Health = (ushort)xmlMonster.Health.Now,

                    MaxHealth = (ushort)xmlMonster.Health.Max,

                    Outfit = xmlMonster.Look.TypeEx != 0 ? new Outfit(xmlMonster.Look.TypeEx) : new Outfit(xmlMonster.Look.Type, xmlMonster.Look.Head, xmlMonster.Look.Body, xmlMonster.Look.Legs, xmlMonster.Look.Feet, Addon.None),

                    Corpse = (ushort)xmlMonster.Look.Corpse,

                    Sentences = xmlMonster.Voices?.Select(v => v.Sentence).ToArray(),

                    LootItems = xmlMonster.LootItems?.Select(l => new LootItem() { OpenTibiaId = l.Id, KillsToGetOne = l.KillsToGetOne ?? 1, CountMin = l.CountMin ?? 1, CountMax = l.CountMax ?? 1 } ).ToArray()
                } );
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

            GameObjectScript<Monster> gameObjectScript = server.GameObjectScripts.GetMonsterGameObjectScript(monster.Name);

            if (gameObjectScript != null)
            {
                gameObjectScript.Start(monster);
            }
        }

        public bool Detach(Monster monster)
        {
            if (server.GameObjects.RemoveGameObject(monster) )
            {
                GameObjectScript<Monster> gameObjectScript = server.GameObjectScripts.GetMonsterGameObjectScript(monster.Name);

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