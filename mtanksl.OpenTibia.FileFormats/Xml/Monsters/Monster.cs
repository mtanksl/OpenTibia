using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Monsters
{
    [XmlRoot("monster")]
    public class Monster
    {
        public static Monster Load(XElement monsterNode)
        {           
            Monster monster = new Monster();

            monster.Name = (string)monsterNode.Attribute("name");

            monster.NameDescription = (string)monsterNode.Attribute("nameDescription");

            monster.Speed = (int)monsterNode.Attribute("speed");

            monster.Experience = (uint)monsterNode.Attribute("experience");

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
                monster.Voices = new List<VoiceItem>();

                foreach (var voiceNode in voicesNode.Elements() )
                {
                    monster.Voices.Add(new VoiceItem() 
                    { 
                        Sentence = (string)voiceNode.Attribute("sentence")
                    } );
                }
            }

            XElement lootNode = monsterNode.Element("loot");

            if (lootNode != null)
            {
                monster.LootItems = new List<LootItem>();

                foreach (var itemNode in lootNode.Elements() )
                {
                    monster.LootItems.Add(new LootItem() 
                    { 
                        Id = (ushort)(int)itemNode.Attribute("id"),

                        CountMin = (int?)itemNode.Attribute("countmin"),

                        CountMax = (int?)itemNode.Attribute("countmax"),

                        KillsToGetOne = (int?)itemNode.Attribute("killsToGetOne")
                    } );
                }
            }

            XElement elementsNode = monsterNode.Element("elements");

            if (elementsNode != null)
            {
                monster.ElementItems = new List<ElementItem>();

                foreach (var elementNode in elementsNode.Elements() )
                {
                    monster.ElementItems.Add(new ElementItem() 
                    {
                        HolyPercent = (int?)elementNode.Attribute("holyPercent"),

                        IcePercent = (int?)elementNode.Attribute("icePercent"),

                        DeathPercent = (int?)elementNode.Attribute("deathPercent"),

                        PhysicalPercent = (int?)elementNode.Attribute("physicalPercent"),

                        Earthpercent = (int?)elementNode.Attribute("earthpercent"),

                        EnergyPercent = (int?)elementNode.Attribute("energyPercent"),

                        FirePercent = (int?)elementNode.Attribute("firePercent")
                    } );
                }
            }

            return monster;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("nameDescription")]
        public string NameDescription { get; set; }

        [XmlAttribute("speed")]
        public int Speed { get; set; }

        [XmlAttribute("experience")]
        public uint Experience { get; set; }

        [XmlElement("health")]
        public Health Health { get; set; }

        [XmlElement("look")]
        public Look Look { get; set; }

        [XmlArray("voices")]
        [XmlArrayItem("voice")]
        public List<VoiceItem> Voices { get; set; }

        [XmlArray("loot")]
        [XmlArrayItem("item")]
        public List<LootItem> LootItems { get; set; }

        [XmlArray("elements")]
        [XmlArrayItem("element")]
        public List<ElementItem> ElementItems { get; set; }
    }
}