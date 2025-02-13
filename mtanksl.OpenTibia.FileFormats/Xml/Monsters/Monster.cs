using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    public class Monster
    {
        public static Monster Load(XElement monsterNode)
        {           
            Monster monster = new Monster();

            monster.Name = (string)monsterNode.Attribute("name");

            monster.NameDisplayed = (string)monsterNode.Attribute("nameDisplayed");

            monster.NameDescription = (string)monsterNode.Attribute("nameDescription");

            switch ( (string)monsterNode.Attribute("race") )
            {
                case "blood":

                    monster.Race = Race.Blood;

                    break;

                case "energy":

                    monster.Race = Race.Energy;

                    break;

                case "fire":

                    monster.Race = Race.Fire;

                    break;

                case "venom":

                    monster.Race = Race.Venom;

                    break;

                case "undead":

                    monster.Race = Race.Undead;

                    break;
            }

            monster.Speed = (int)monsterNode.Attribute("speed");

            monster.Experience = (uint)monsterNode.Attribute("experience");

            monster.ManaCost = (int)monsterNode.Attribute("manacost");

            XElement healthNode = monsterNode.Element("health");

            monster.Health = new Health()
            {
                Now = (int)healthNode.Attribute("now"),

                Max = (int)healthNode.Attribute("max")
            };

            XElement outfitNode = monsterNode.Element("look");

            monster.Look = new Look()
            {
                TypeEx = (int?)outfitNode.Attribute("typeex") ?? 0,

                Type = (int?)outfitNode.Attribute("type") ?? 0,

                Head = (int?)outfitNode.Attribute("head") ?? 0,

                Body = (int?)outfitNode.Attribute("body") ?? 0,

                Legs = (int?)outfitNode.Attribute("legs") ?? 0,

                Feet = (int?)outfitNode.Attribute("feet") ?? 0,

                Corpse = (int?)outfitNode.Attribute("corpse") ?? 3058
            };
 
            XElement voicesNode = monsterNode.Element("voices");

            if (voicesNode != null)
            {
                monster.Voices = new VoiceCollection()
                {
                    Interval = (int)voicesNode.Attribute("interval"),

                    Chance = Math.Max(0, Math.Min(100, (double)voicesNode.Attribute("chance") ) ),

                    Items = new List<VoiceItem>()
                };

                foreach (var voiceNode in voicesNode.Elements("voice") )
                {
                    monster.Voices.Items.Add(new VoiceItem() 
                    { 
                        Sentence = (string)voiceNode.Attribute("sentence"),

                        Yell = (int)voiceNode.Attribute("yell")
                    } );
                }
            }

            XElement lootNode = monsterNode.Element("loot");

            if (lootNode != null)
            {
                monster.Loot = new List<LootItem>();

                foreach (var itemNode in lootNode.Elements("item") )
                {
                    monster.Loot.Add(new LootItem() 
                    { 
                        Id = (ushort)(int)itemNode.Attribute("id"),

                        CountMin = (int?)itemNode.Attribute("countmin"),

                        CountMax = (int?)itemNode.Attribute("countmax"),

                        KillsToGetOne = (int)itemNode.Attribute("killsToGetOne")
                    } );
                }
            }

            XElement immunities = monsterNode.Element("immunities");

            if (immunities != null)
            {
                monster.Immunities = new List<ImmunityItem>();

                foreach (var immunityNode in immunities.Elements("immunity") )
                {
                    monster.Immunities.Add(new ImmunityItem() 
                    {
                        Physical = (int?)immunityNode.Attribute("physical"),

                        Earth = (int?)immunityNode.Attribute("earth"),

                        Fire = (int?)immunityNode.Attribute("fire"),

                        Energy = (int?)immunityNode.Attribute("energy"),

                        Ice = (int?)immunityNode.Attribute("ice"),

                        Death = (int?)immunityNode.Attribute("death"),

                        Holy = (int?)immunityNode.Attribute("holy"),

                        Drown = (int?)immunityNode.Attribute("drown"),

                        ManaDrain = (int?)immunityNode.Attribute("manaDrain"),

                        LifeDrain = (int?)immunityNode.Attribute("lifeDrain"),


                        Paralyze = (int?)immunityNode.Attribute("paralyze"),

                        Invisible = (int?)immunityNode.Attribute("invisible")
                    } );
                }
            }

            XElement elementsNode = monsterNode.Element("elements");

            if (elementsNode != null)
            {
                monster.Elements = new List<ElementItem>();

                foreach (var elementNode in elementsNode.Elements("element") )
                {
                    monster.Elements.Add(new ElementItem() 
                    {
                        PhysicalPercent = (int?)elementNode.Attribute("physicalPercent"),

                        Earthpercent = (int?)elementNode.Attribute("earthpercent"),

                        FirePercent = (int?)elementNode.Attribute("firePercent"),

                        EnergyPercent = (int?)elementNode.Attribute("energyPercent"),

                        IcePercent = (int?)elementNode.Attribute("icePercent"),

                        DeathPercent = (int?)elementNode.Attribute("deathPercent"),

                        HolyPercent = (int?)elementNode.Attribute("holyPercent"),

                        DrownPercent = (int?)elementNode.Attribute("drownPercent"),

                        ManaDrainPercent = (int?)elementNode.Attribute("manaDrainPercent"),

                        LifeDrainPercent = (int?)elementNode.Attribute("lifeDrainPercent")
                    } );
                }
            }

            XElement flagsNode = monsterNode.Element("flags");

            if (flagsNode != null)
            {
                monster.Flags = new List<FlagItem>();

                foreach (var flagNode in flagsNode.Elements("flag") )
                {
                    monster.Flags.Add(new FlagItem() 
                    {
                        Summonable = (int?)flagNode.Attribute("summonable"),

                        Attackable = (int?)flagNode.Attribute("attackable"),

                        Hostile = (int?)flagNode.Attribute("hostile"),

                        Illusionable = (int?)flagNode.Attribute("illusionable"),

                        Convinceable = (int?)flagNode.Attribute("convinceable"),

                        Pushable = (int?)flagNode.Attribute("pushable"),

                        CanPushItems = (int?)flagNode.Attribute("canpushitems"),

                        CanPushCreatures = (int?)flagNode.Attribute("canpushcreatures"),

                        TargetDistance = (int?)flagNode.Attribute("targetdistance"),

                        RunOnHealth = (int?)flagNode.Attribute("runonhealth")
                    } );
                }
            }

            XElement attacksNode = monsterNode.Element("attacks");

            if (attacksNode != null)
            {
                monster.Attacks = new List<AttackItem>();

                foreach (var attackNode in attacksNode.Elements("attack") )
                {
                    Dictionary<string, string> attributes = new Dictionary<string, string>();

                    foreach (var attributeNode in attackNode.Attributes().Where(a => a.Name != "name" && a.Name != "interval" && a.Name != "chance" && a.Name != "min" && a.Name != "max") )
                    {
                        var key = attributeNode.Name.LocalName;

                        if ( !attributes.ContainsKey(key) )
                        {
                            attributes.Add(key, attributeNode.Value);
                        }
                    }

                    foreach (var attributeNode in attackNode.Elements("attribute") )
                    {
                        var key = (string)attributeNode.Attribute("key");

                        if ( !attributes.ContainsKey(key) )
                        {
                            attributes.Add(key, (string)attributeNode.Attribute("value") );
                        }
                    }

                    monster.Attacks.Add(new AttackItem() 
                    {
                        Name = (string)attackNode.Attribute("name"),

                        Interval = (int)attackNode.Attribute("interval"),

                        Chance = Math.Max(0, Math.Min(100, (double)attackNode.Attribute("chance") ) ),

                        Min = (int?)attackNode.Attribute("min"),

                        Max = (int?)attackNode.Attribute("max"),

                        Attributes = attributes
                    } );
                }
            }

            XElement defensesNode = monsterNode.Element("defenses");

            if (defensesNode != null)
            {
                monster.Defenses = new DefenseCollection()
                {
                    Mitigation = (double)defensesNode.Attribute("mitigation"),

                    Defense = (int)defensesNode.Attribute("defense"),

                    Armor = (int)defensesNode.Attribute("armor"),

                    Items = new List<DefenseItem>()
                };

                foreach (var defenseNode in defensesNode.Elements("defense") )
                {
                    Dictionary<string, string> attributes = new Dictionary<string, string>();

                    foreach (var attributeNode in defenseNode.Attributes().Where(a => a.Name != "name" && a.Name != "interval" && a.Name != "chance" && a.Name != "min" && a.Name != "max") )
                    {
                        var key = attributeNode.Name.LocalName;

                        if ( !attributes.ContainsKey(key) )
                        {
                            attributes.Add(key, attributeNode.Value);
                        }
                    }

                    foreach (var attributeNode in defenseNode.Elements("attribute") )
                    {
                        var key = (string)attributeNode.Attribute("key");

                        if ( !attributes.ContainsKey(key) )
                        {
                            attributes.Add(key, (string)attributeNode.Attribute("value") );
                        }
                    }

                    monster.Defenses.Items.Add(new DefenseItem() 
                    {
                        Name = (string)defenseNode.Attribute("name"),

                        Interval = (int)defenseNode.Attribute("interval"),

                        Chance = Math.Max(0, Math.Min(100, (double)defenseNode.Attribute("chance") ) ),

                        Min = (int?)defenseNode.Attribute("min"),

                        Max = (int?)defenseNode.Attribute("max"),

                        Attributes = attributes
                    } );
                }
            }

            XElement changeTargetStrategyNode = monsterNode.Element("changetargetstrategy");

            if (changeTargetStrategyNode != null)
            {
                monster.ChangeTargetStrategy = new ChangeTargetStrategy()
                {
                    Interval = (int)changeTargetStrategyNode.Attribute("interval"),

                    Chance = Math.Max(0, Math.Min(100, (double)changeTargetStrategyNode.Attribute("chance") ) )
                };
            }

            XElement targetStrategyNode = monsterNode.Element("targetstrategy");

            if (targetStrategyNode != null)
            {
                monster.TargetStrategy = new TargetStrategy()
                {
                    Nearest = Math.Max(0, Math.Min(100, (double)targetStrategyNode.Attribute("nearest") ) ),
                     
                    Weakest = Math.Max(0, Math.Min(100, (double)targetStrategyNode.Attribute("weakest") ) ),

                    MostDamage = Math.Max(0, Math.Min(100, (double)targetStrategyNode.Attribute("mostdamage") ) ),

                    Random = Math.Max(0, Math.Min(100, (double)targetStrategyNode.Attribute("random") ) )
                };
            }

            return monster;
        }

        public string Name { get; set; }

        public string NameDisplayed { get; set; }

        public string NameDescription { get; set; }

        public Race Race { get; set; }

        public int Speed { get; set; }

        public uint Experience { get; set; }

        public int ManaCost { get; set; }

        public Health Health { get; set; }

        public Look Look { get; set; }

        public VoiceCollection Voices { get; set; }

        public List<LootItem> Loot { get; set; }

        public List<ImmunityItem> Immunities { get; set; }

        public List<ElementItem> Elements { get; set; }

        public List<FlagItem> Flags { get; set; }

        public List<AttackItem> Attacks { get; set; }

        public DefenseCollection Defenses { get; set; }

        public ChangeTargetStrategy ChangeTargetStrategy { get; set; }

        public TargetStrategy TargetStrategy { get; set; }
    }
}