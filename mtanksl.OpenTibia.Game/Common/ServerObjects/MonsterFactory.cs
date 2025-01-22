using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.Game.GameObjectScripts;
using System.Collections.Generic;
using System.Linq;
using LootItem = OpenTibia.Common.Objects.LootItem;
using Monster = OpenTibia.Common.Objects.Monster;
using VoiceCollection = OpenTibia.Common.Objects.VoiceCollection;
using VoiceItem = OpenTibia.Common.Objects.VoiceItem;

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
                MonsterMetadata monsterMetadata = new MonsterMetadata()
                {
                    Name = xmlMonster.Name,

                    Description = xmlMonster.NameDescription,

                    Speed = (ushort)xmlMonster.Speed,

                    Experience = xmlMonster.Experience,

                    Race = xmlMonster.Race,

                    Health = (ushort)xmlMonster.Health.Now,

                    MaxHealth = (ushort)xmlMonster.Health.Max,

                    Outfit = xmlMonster.Look.TypeEx != 0 ? new Outfit(xmlMonster.Look.TypeEx) : new Outfit(xmlMonster.Look.Type, xmlMonster.Look.Head, xmlMonster.Look.Body, xmlMonster.Look.Legs, xmlMonster.Look.Feet, Addon.None),

                    Corpse = (ushort)xmlMonster.Look.Corpse,

                    Voices = xmlMonster.Voices == null ? null : new VoiceCollection()
                    {
                        Interval = xmlMonster.Voices.Interval,

                        Chance = xmlMonster.Voices.Chance,

                        Items = xmlMonster.Voices.Items.Select(v => new VoiceItem() { Sentence = v.Sentence, Yell = v.Yell == 1 } ).ToArray()
                    },

                    Loot = xmlMonster.Loot?.Select(l => new LootItem() { OpenTibiaId = l.Id, KillsToGetOne = l.KillsToGetOne ?? 1, CountMin = l.CountMin ?? 1, CountMax = l.CountMax ?? 1 } ).ToArray(),

                    DamageTakenFromElements = new Dictionary<DamageType, double>()
                };

                if (xmlMonster.Elements != null)
                {
                    foreach (var elementItem in xmlMonster.Elements)
                    {
                        if (elementItem.HolyPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Holy] = (100 - elementItem.HolyPercent.Value) / 100.0;
                        }
                        else if (elementItem.IcePercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Ice] = (100 - elementItem.IcePercent.Value) / 100.0;
                        }
                        else if (elementItem.DeathPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Death] = (100 - elementItem.DeathPercent.Value) / 100.0;
                        }
                        else if (elementItem.PhysicalPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Physical] = (100 - elementItem.PhysicalPercent.Value) / 100.0;
                        }
                        else if (elementItem.Earthpercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Earth] = (100 - elementItem.Earthpercent.Value) / 100.0;
                        }
                        else if (elementItem.EnergyPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Energy] = (100 - elementItem.EnergyPercent.Value) / 100.0;
                        }
                        else if (elementItem.FirePercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Fire] = (100 - elementItem.FirePercent.Value) / 100.0;
                        }
                    }
                }

                metadatas.Add(xmlMonster.Name, monsterMetadata);
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