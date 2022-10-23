using System.Xml.Linq;
using System.Xml.Serialization;

namespace OpenTibia.FileFormats.Xml.Npcs
{
    [XmlRoot("npc")]
    public class Npc
    {
        public static Npc Load(XElement npcNode)
        {
            Npc npc = new Npc();

            npc.Name = (string)npcNode.Attribute("name");

            npc.Speed = (int)npcNode.Attribute("speed");

            XElement healthNode = npcNode.Element("health");

            npc.Health = new Health()
            {
                Now = (int)healthNode.Attribute("now"),

                Max = (int)healthNode.Attribute("max")
            };

            XElement outfitNode = npcNode.Element("look");

            npc.Look = new Look()
            {
                Type = (int)outfitNode.Attribute("type"),

                Head = (int)outfitNode.Attribute("head"),

                Body = (int)outfitNode.Attribute("body"),

                Legs = (int)outfitNode.Attribute("legs"),

                Feet = (int)outfitNode.Attribute("feet"),
            };

            return npc;
        }

        [XmlAttribute("name")]
        public string Name { get; set; }

        [XmlAttribute("speed")]
        public int Speed { get; set; }

        [XmlElement("health")]
        public Health Health { get; set; }

        [XmlElement("look")]
        public Look Look { get; set; }        
    }
}