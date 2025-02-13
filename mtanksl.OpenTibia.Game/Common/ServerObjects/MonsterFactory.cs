using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Monsters;
using OpenTibia.Game.GameObjectScripts;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;
using System.Linq;
using AttackItem = OpenTibia.Common.Objects.AttackItem;
using DefenseItem = OpenTibia.Common.Objects.DefenseItem;
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

                    NameDisplayed = xmlMonster.NameDisplayed,

                    Description = xmlMonster.NameDescription,

                    Race = xmlMonster.Race,

                    Speed = (ushort)xmlMonster.Speed,

                    Experience = xmlMonster.Experience,

                    ManaCost = (ushort)xmlMonster.ManaCost,

                    Health = (ushort)xmlMonster.Health.Now,

                    MaxHealth = (ushort)xmlMonster.Health.Max,

                    Light = xmlMonster.Light == null ? Light.None : new Light( (byte)xmlMonster.Light.Level, (byte)xmlMonster.Light.Color),

                    Outfit = xmlMonster.Look.TypeEx != 0 ? new Outfit(xmlMonster.Look.TypeEx) : new Outfit(xmlMonster.Look.Type, xmlMonster.Look.Head, xmlMonster.Look.Body, xmlMonster.Look.Legs, xmlMonster.Look.Feet, Addon.None),

                    Corpse = (ushort)xmlMonster.Look.Corpse,

                    Voices = (xmlMonster.Voices == null || xmlMonster.Voices.Items == null || xmlMonster.Voices.Items.Count == 0) ? null : new VoiceCollection()
                    {
                        Interval = xmlMonster.Voices.Interval,

                        Chance = xmlMonster.Voices.Chance,

                        Items = xmlMonster.Voices.Items.Select(v => new VoiceItem() { Sentence = v.Sentence, Yell = v.Yell == 1 } ).ToArray()
                    },

                    Loot = xmlMonster.Loot?.Select(l => new LootItem() { OpenTibiaId = l.Id, KillsToGetOne = l.KillsToGetOne, CountMin = l.CountMin ?? 1, CountMax = l.CountMax ?? 1 } ).ToArray(),

                    Immunities = new HashSet<DamageType>(),

                    DamageTakenFromElements = new Dictionary<DamageType, double>(),
    
                    Summonable = xmlMonster.Flags?.Any(f => f.Summonable == 1) ?? false,

                    Attackable = xmlMonster.Flags?.Any(f => f.Attackable == 1) ?? false,

                    Hostile = xmlMonster.Flags?.Any(f => f.Hostile == 1) ?? false,

                    Illusionable = xmlMonster.Flags?.Any(f => f.Illusionable == 1) ?? false,

                    Convinceable = xmlMonster.Flags?.Any(f => f.Convinceable == 1) ?? false,

                    Pushable = xmlMonster.Flags?.Any(f => f.Pushable == 1) ?? false,

                    CanPushItems = xmlMonster.Flags?.Any(f => f.CanPushItems == 1) ?? false,

                    CanPushCreatures = xmlMonster.Flags?.Any(f => f.CanPushCreatures == 1) ?? false,

                    TargetDistance = xmlMonster.Flags?.Where(f => f.TargetDistance != null).Select(f => f.TargetDistance.Value).FirstOrDefault() ?? 0,

                    RunOnHealth = xmlMonster.Flags?.Where(f => f.RunOnHealth != null).Select(f => f.RunOnHealth.Value).FirstOrDefault() ?? 0,

                    Attacks = xmlMonster.Attacks?.Select(a => new AttackItem() { Name = a.Name, Interval = a.Interval, Chance = a.Chance, Min = a.Min ?? 0, Max = a.Max ?? 0, Attributes = a.Attributes } ).ToArray(),

                    Mitigation = xmlMonster.Defenses?.Mitigation ?? 0,

                    Defense = xmlMonster.Defenses?.Defense ?? 0,

                    Armor = xmlMonster.Defenses?.Armor ?? 0,

                    Defenses = xmlMonster.Defenses?.Items?.Select(d => new DefenseItem() { Name = d.Name, Interval = d.Interval, Chance = d.Chance, Min = d.Min ?? 0, Max = d.Max ?? 0, Attributes = d.Attributes } ).ToArray(),

                    ChangeTargetInterval = xmlMonster.ChangeTargetStrategy?.Interval ?? 1000,

                    ChangeTargetChance = xmlMonster.ChangeTargetStrategy?.Chance ?? 0.0,

                    TargetNearestChance = xmlMonster.TargetStrategy?.Nearest ?? 0.0,

                    TargetWeakestChance = xmlMonster.TargetStrategy?.Weakest ?? 0.0,

                    TargetMostDamageChance = xmlMonster.TargetStrategy?.MostDamage ?? 0.0,

                    TargetRandomChance = xmlMonster.TargetStrategy?.Random ?? 0.0
                };

                if (xmlMonster.Immunities != null)
                {
                    foreach (var immunityItem in xmlMonster.Immunities)
                    {
                        if (immunityItem.Physical == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.Physical);
                        }
                        else if (immunityItem.Earth == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.Earth);
                        }
                        else if (immunityItem.Fire == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.Fire);
                        }
                        else if (immunityItem.Energy == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.Energy);
                        }
                        else if (immunityItem.Ice == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.Ice);
                        }
                        else if (immunityItem.Death == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.Death);
                        }
                        else if (immunityItem.Holy == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.Holy);
                        }
                        else if (immunityItem.Drown == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.Drown);
                        }
                        else if (immunityItem.ManaDrain == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.ManaDrain);
                        }
                        else if (immunityItem.LifeDrain == 1)
                        {
                            monsterMetadata.Immunities.Add(DamageType.LifeDrain);
                        }
                        else if (immunityItem.Paralyze == 1)
                        {
                            monsterMetadata.ImmuneToParalyse = true;
                        }
                        else if (immunityItem.Invisible == 1)
                        {
                            monsterMetadata.ImmuneToInvisible = true;
                        }
                    }
                }

                if (xmlMonster.Elements != null)
                {
                    foreach (var elementItem in xmlMonster.Elements)
                    {
                        if (elementItem.PhysicalPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Physical] = (100 - elementItem.PhysicalPercent.Value) / 100.0;
                        }
                        else if (elementItem.Earthpercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Earth] = (100 - elementItem.Earthpercent.Value) / 100.0;
                        }
                        else if (elementItem.FirePercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Fire] = (100 - elementItem.FirePercent.Value) / 100.0;
                        }
                        else if (elementItem.EnergyPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Energy] = (100 - elementItem.EnergyPercent.Value) / 100.0;
                        }
                        else if (elementItem.IcePercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Ice] = (100 - elementItem.IcePercent.Value) / 100.0;
                        }
                        else if (elementItem.DeathPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Death] = (100 - elementItem.DeathPercent.Value) / 100.0;
                        }
                        else if (elementItem.HolyPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Holy] = (100 - elementItem.HolyPercent.Value) / 100.0;
                        }
                        else if (elementItem.DrownPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.Drown] = (100 - elementItem.DrownPercent.Value) / 100.0;
                        }
                        else if (elementItem.ManaDrainPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.ManaDrain] = (100 - elementItem.ManaDrainPercent.Value) / 100.0;
                        }
                        else if (elementItem.LifeDrainPercent != null)
                        {
                            monsterMetadata.DamageTakenFromElements[DamageType.LifeDrain] = (100 - elementItem.LifeDrainPercent.Value) / 100.0;
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

            GameObjectScript<Monster> gameObjectScript = server.GameObjectScripts.GetMonsterGameObjectScript(monster.Name) ?? server.GameObjectScripts.GetMonsterGameObjectScript("");

            if (gameObjectScript != null)
            {
                gameObjectScript.Start(monster);
            }

            MonsterCreationPlugin plugin = server.Plugins.GetMonsterCreationPlugin(monster.Name) ?? server.Plugins.GetMonsterCreationPlugin("");

            if (plugin != null)
            {
                if (plugin.OnStart(monster).Result)
                {
                    //
                }
            }
        }

        public bool Detach(Monster monster)
        {
            if (server.GameObjects.RemoveGameObject(monster) )
            {
                GameObjectScript<Monster> gameObjectScript = server.GameObjectScripts.GetMonsterGameObjectScript(monster.Name) ?? server.GameObjectScripts.GetMonsterGameObjectScript("");

                if (gameObjectScript != null)
                {
                    gameObjectScript.Stop(monster);
                }

                MonsterCreationPlugin plugin = server.Plugins.GetMonsterCreationPlugin(monster.Name) ?? server.Plugins.GetMonsterCreationPlugin("");

                if (plugin != null)
                {
                    if (plugin.OnStop(monster).Result)
                    {
                        //
                    }
                }

                return true;
            }

            return false;
        }

        public void ClearComponentsAndEventHandlers(Monster monster)
        {
            server.GameObjectComponents.ClearComponents(monster);

            server.GameObjectEventHandlers.ClearEventHandlers(monster);

            server.PositionalEventHandlers.ClearEventHandlers(monster);
        }
    }
}