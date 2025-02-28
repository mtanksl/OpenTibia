using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

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

            monster.Health = new HealthItem()
            {
                Now = (int)healthNode.Attribute("now"),

                Max = (int)healthNode.Attribute("max")
            };

            XElement lightNode = monsterNode.Element("light");

            if (lightNode != null)
            {
                monster.Light = new LightItem()
                {
                    Level = (int?)lightNode.Attribute("level") ?? 0,

                    Color = (int?)lightNode.Attribute("color") ?? 0
                };
            }

            XElement lookNode = monsterNode.Element("look");

            monster.Look = new LookItem()
            {
                TypeEx = (int?)lookNode.Attribute("typeex") ?? 0,

                Type = (int?)lookNode.Attribute("type") ?? 0,

                Head = (int?)lookNode.Attribute("head") ?? 0,

                Body = (int?)lookNode.Attribute("body") ?? 0,

                Legs = (int?)lookNode.Attribute("legs") ?? 0,

                Feet = (int?)lookNode.Attribute("feet") ?? 0,

                Addon = (int?)lookNode.Attribute("addon") ?? 0,

                Corpse = (int?)lookNode.Attribute("corpse") ?? 3058
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

        public XDocument Serialize(ItemsFile itemsFile = null)
        {
            return Serialize(this, itemsFile);
        }

        private static XDocument Serialize(Monster monster, ItemsFile itemsFile)
        {
            var xml = new XDocument();

            var monsterNode =
                new XElement("monster");

            xml.Add(monsterNode);

            if ( !string.IsNullOrEmpty(monster.Name) )
            {
                monsterNode.Add(new XAttribute("name", monster.Name) );
            }

            if ( !string.IsNullOrEmpty(monster.NameDisplayed) )
            {
                monsterNode.Add(new XAttribute("nameDisplayed", monster.NameDisplayed) );
            }

            if ( !string.IsNullOrEmpty(monster.NameDescription) )
            {
                monsterNode.Add(new XAttribute("nameDescription", monster.NameDescription) );
            }

            monsterNode.Add(new XAttribute("race", monster.Race.ToString().ToLower() ) );

            monsterNode.Add(new XAttribute("speed", monster.Speed) );

            monsterNode.Add(new XAttribute("experience", monster.Experience) );

            monsterNode.Add(new XAttribute("manacost", monster.ManaCost) );

            var healthNode =
                new XElement("health");

            monsterNode.Add(healthNode);

            healthNode.Add(new XAttribute("now", monster.Health.Now) );

            healthNode.Add(new XAttribute("max", monster.Health.Max) );

            if (monster.Light != null)
            {
                var lightNode =
                    new XElement("light");

                monsterNode.Add(lightNode);

                lightNode.Add(new XAttribute("level", monster.Light.Level) );

                lightNode.Add(new XAttribute("color", monster.Light.Color) );
            }

            var lookNode =
                new XElement("look");

            monsterNode.Add(lookNode);

            lookNode.Add(new XAttribute("typeex", monster.Look.TypeEx) );

            lookNode.Add(new XAttribute("type", monster.Look.Type) );

            lookNode.Add(new XAttribute("head", monster.Look.Head) );

            lookNode.Add(new XAttribute("body", monster.Look.Body) );

            lookNode.Add(new XAttribute("legs", monster.Look.Legs) );

            lookNode.Add(new XAttribute("feet", monster.Look.Feet) );

            // lookNode.Add(new XAttribute("addon", monster.Look.Addon) );

            lookNode.Add(new XAttribute("corpse", monster.Look.Corpse) );

            if (monster.Flags != null)
            {
                var flagsNode =
                    new XElement("flags");

                monsterNode.Add(flagsNode);

                foreach (var flag in monster.Flags)
                {
                    var flagNode =
                        new XElement("flag");

                    flagsNode.Add(flagNode);

                    if (flag.Summonable != null) flagNode.Add(new XAttribute("summonable", flag.Summonable) );

                    if (flag.Attackable != null) flagNode.Add(new XAttribute("attackable", flag.Attackable) );

                    if (flag.Hostile != null) flagNode.Add(new XAttribute("hostile", flag.Hostile) );

                    if (flag.Illusionable != null) flagNode.Add(new XAttribute("illusionable", flag.Illusionable) );

                    if (flag.Convinceable != null) flagNode.Add(new XAttribute("convinceable", flag.Convinceable) );

                    if (flag.Pushable != null) flagNode.Add(new XAttribute("pushable", flag.Pushable) );

                    if (flag.CanPushItems != null) flagNode.Add(new XAttribute("canpushitems", flag.CanPushItems) );

                    if (flag.CanPushCreatures != null) flagNode.Add(new XAttribute("canpushcreatures", flag.CanPushCreatures) );

                    if (flag.TargetDistance != null) flagNode.Add(new XAttribute("targetdistance", flag.TargetDistance) );

                    if (flag.RunOnHealth != null) flagNode.Add(new XAttribute("runonhealth", flag.RunOnHealth) );
                }
            }

            if (monster.Immunities != null)
            {
                var immunitiesNode =
                    new XElement("immunities");

                monsterNode.Add(immunitiesNode);

                foreach (var immunity in monster.Immunities)
                {
                    var immunityNode =
                        new XElement("immunity");

                    immunitiesNode.Add(immunityNode);

                    if (immunity.Physical != null) immunityNode.Add(new XAttribute("physical", immunity.Physical) );

                    if (immunity.Earth != null) immunityNode.Add(new XAttribute("earth", immunity.Earth) );

                    if (immunity.Fire != null) immunityNode.Add(new XAttribute("fire", immunity.Fire) );

                    if (immunity.Energy != null) immunityNode.Add(new XAttribute("energy", immunity.Energy) );

                    if (immunity.Ice != null) immunityNode.Add(new XAttribute("ice", immunity.Ice) );

                    if (immunity.Death != null) immunityNode.Add(new XAttribute("death", immunity.Death) );

                    if (immunity.Holy != null) immunityNode.Add(new XAttribute("holy", immunity.Holy) );

                    if (immunity.Drown != null) immunityNode.Add(new XAttribute("drown", immunity.Drown) );

                    if (immunity.ManaDrain != null) immunityNode.Add(new XAttribute("manaDrain", immunity.ManaDrain) );

                    if (immunity.LifeDrain != null) immunityNode.Add(new XAttribute("lifeDrain", immunity.LifeDrain) );

                    if (immunity.Paralyze != null) immunityNode.Add(new XAttribute("paralyze", immunity.Paralyze) );

                    if (immunity.Invisible != null) immunityNode.Add(new XAttribute("invisible", immunity.Invisible) );
                }
            }

            if (monster.Elements != null)
            {
                var elementsNode =
                    new XElement("elements");

                monsterNode.Add(elementsNode);

                foreach (var element in monster.Elements)
                {
                    var elementNode =
                        new XElement("element");

                    elementsNode.Add(elementNode);

                    if (element.PhysicalPercent != null) elementNode.Add(new XAttribute("physicalPercent", element.PhysicalPercent) );

                    if (element.Earthpercent != null) elementNode.Add(new XAttribute("earthpercent", element.Earthpercent) );

                    if (element.FirePercent != null) elementNode.Add(new XAttribute("firePercent", element.FirePercent) );

                    if (element.EnergyPercent != null) elementNode.Add(new XAttribute("energyPercent", element.EnergyPercent) );

                    if (element.IcePercent != null) elementNode.Add(new XAttribute("icePercent", element.IcePercent) );

                    if (element.DeathPercent != null) elementNode.Add(new XAttribute("deathPercent", element.DeathPercent) );

                    if (element.HolyPercent != null) elementNode.Add(new XAttribute("holyPercent", element.HolyPercent) );

                    if (element.DrownPercent != null) elementNode.Add(new XAttribute("drownPercent", element.DrownPercent) );

                    if (element.ManaDrainPercent != null) elementNode.Add(new XAttribute("manaDrainPercent", element.ManaDrainPercent) );

                    if (element.LifeDrainPercent != null) elementNode.Add(new XAttribute("lifeDrainPercent", element.LifeDrainPercent) );
                }
            }

            if (monster.Voices != null)
            {
                var voicesNode =
                    new XElement("voices");

                monsterNode.Add(voicesNode);

                voicesNode.Add(new XAttribute("interval", monster.Voices.Interval) );

                voicesNode.Add(new XAttribute("chance", monster.Voices.Chance) );

                foreach (var voice in monster.Voices.Items)
                {
                    var voiceNode =
                        new XElement("voice");

                    voicesNode.Add(voiceNode);

                    voiceNode.Add(new XAttribute("yell", voice.Yell) );

                    voiceNode.Add(new XAttribute("sentence", voice.Sentence) );
                }
            }

            if (monster.Defenses != null)
            {
                var defensesNode =
                    new XElement("defenses");

                monsterNode.Add(defensesNode);

                defensesNode.Add(new XAttribute("mitigation", monster.Defenses.Mitigation) );

                defensesNode.Add(new XAttribute("defense", monster.Defenses.Defense) );

                defensesNode.Add(new XAttribute("armor", monster.Defenses.Armor) );

                foreach (var defenses in monster.Defenses.Items)
                {
                    var defenseNode =
                        new XElement("defense");

                    defensesNode.Add(defenseNode);

                    defenseNode.Add(new XAttribute("name", defenses.Name) );

                    defenseNode.Add(new XAttribute("interval", defenses.Interval) );

                    defenseNode.Add(new XAttribute("chance", defenses.Chance) );

                    if (defenses.Min != null) defenseNode.Add(new XAttribute("min", defenses.Min) );

                    if (defenses.Max != null) defenseNode.Add(new XAttribute("max", defenses.Max) );

                    //TODO: attributes
                }
            }

            if (monster.ChangeTargetStrategy != null)
            {
                var changetargetstrategyNode =
                    new XElement("changetargetstrategy");

                monsterNode.Add(changetargetstrategyNode);

                changetargetstrategyNode.Add(new XAttribute("interval", monster.ChangeTargetStrategy.Interval) );

                changetargetstrategyNode.Add(new XAttribute("chance", monster.ChangeTargetStrategy.Chance) );
            }

            if (monster.TargetStrategy != null)
            {
                var targetstrategyNode =
                    new XElement("targetstrategy");

                monsterNode.Add(targetstrategyNode);

                targetstrategyNode.Add(new XAttribute("nearest", monster.TargetStrategy.Nearest) );

                targetstrategyNode.Add(new XAttribute("weakest", monster.TargetStrategy.Weakest) );

                targetstrategyNode.Add(new XAttribute("mostdamage", monster.TargetStrategy.MostDamage) );

                targetstrategyNode.Add(new XAttribute("random", monster.TargetStrategy.Random) );
            }
                                                            
            if (monster.Attacks != null)
            {
                var attacksNode =
                    new XElement("attacks");

                monsterNode.Add(attacksNode);

                foreach (var attacks in monster.Attacks)
                {
                    var attackNode =
                        new XElement("attack");

                    attacksNode.Add(attackNode);

                    attackNode.Add(new XAttribute("name", attacks.Name) );

                    attackNode.Add(new XAttribute("interval", attacks.Interval) );

                    attackNode.Add(new XAttribute("chance", attacks.Chance) );

                    if (attacks.Min != null) attackNode.Add(new XAttribute("min", attacks.Min) );

                    if (attacks.Max != null) attackNode.Add(new XAttribute("max", attacks.Max) );

                    //TODO: attributes
                }
            }

            if (monster.Loot != null)
            {
                var lootNode =
                    new XElement("loot");

                monsterNode.Add(lootNode);

                foreach (var loot in monster.Loot)
                {
                    var itemNode =
                        new XElement("item");

                    lootNode.Add(itemNode);

                    itemNode.Add(new XAttribute("id", loot.Id) );

                    if (loot.CountMin != null) itemNode.Add(new XAttribute("countmin", loot.CountMin) );

                    if (loot.CountMax != null) itemNode.Add(new XAttribute("countmax", loot.CountMax) );

                    itemNode.Add(new XAttribute("killsToGetOne", loot.KillsToGetOne) );

                    if (itemsFile != null)
                    {
                        var item = itemsFile.Items.Where(i => i.OpenTibiaId == loot.Id).FirstOrDefault();

                        if (item != null)
                        {
                            itemNode.AddBeforeSelf(new XComment(" " + item.Name + " ") );
                        }
                    }
                }
            }

            return xml;
        }

        public void Save(string path, ItemsFile itemsFile = null)
        {
            Save(this, path, itemsFile);
        }

        private static void Save(Monster monster, string path, ItemsFile itemsFile)
        {
            monster.Serialize(itemsFile).Save(path);
        }

        public string Name { get; set; }

        public string NameDisplayed { get; set; }

        public string NameDescription { get; set; }

        public Race Race { get; set; }

        public int Speed { get; set; }

        public uint Experience { get; set; }

        public int ManaCost { get; set; }

        public HealthItem Health { get; set; }

        public LightItem Light { get; set; }

        public LookItem Look { get; set; }

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