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

            monster.Description = (string)monsterNode.Attribute("nameDescription");

            monster.Speed = (ushort)(uint)monsterNode.Attribute("speed");

            XElement healthNode = monsterNode.Element("health");

            monster.Health = (ushort)(uint)healthNode.Attribute("now");

            monster.MaxHealth = (ushort)(uint)healthNode.Attribute("max");

            XElement outfitNode = monsterNode.Element("look");

            monster.Outfit = new Outfit( (int)outfitNode.Attribute("type"), (int)outfitNode.Attribute("head"), (int)outfitNode.Attribute("body"), (int)outfitNode.Attribute("legs"), (int)outfitNode.Attribute("feet"), Addon.None );

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

        public string Description { get; set; }

        public ushort Speed { get; set; }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public Outfit Outfit { get; set; }

        public List<Voice> Voices { get; set; }
    }
}