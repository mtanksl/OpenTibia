using OpenTibia.Common.Structures;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Monsters
{
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
                Type = (int)outfitNode.Attribute("type"),

                Head = (int)outfitNode.Attribute("head"),

                Body = (int)outfitNode.Attribute("body"),

                Legs = (int)outfitNode.Attribute("legs"),

                Feet = (int)outfitNode.Attribute("feet"),

                Corpse = (int)outfitNode.Attribute("corpse")
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

        public string Name { get; set; }

        public string NameDescription { get; set; }

        public int Speed { get; set; }

        public Health Health { get; set; }

        public Look Look { get; set; }

        public List<Voice> Voices { get; set; }
    }
}