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
                monster.Voices = new List<Voice>();

                foreach (var voiceNode in voicesNode.Elements() )
                {
                    monster.Voices.Add(new Voice() 
                    { 
                        Sentence = (string)voiceNode.Attribute("sentence")
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

        [XmlElement("health")]
        public Health Health { get; set; }

        [XmlElement("look")]
        public Look Look { get; set; }

        [XmlArray("voices")]
        [XmlArrayItem("voice")]
        public List<Voice> Voices { get; set; }
    }
}