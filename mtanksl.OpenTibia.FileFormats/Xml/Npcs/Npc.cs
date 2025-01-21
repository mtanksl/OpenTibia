using System.Collections.Generic;
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

            npc.NameDescription = (string)npcNode.Attribute("nameDescription");

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
                TypeEx = (int?)outfitNode.Attribute("typeex") ?? 0,

                Type = (int?)outfitNode.Attribute("type") ?? 0,

                Head = (int?)outfitNode.Attribute("head") ?? 0,

                Body = (int?)outfitNode.Attribute("body") ?? 0,

                Legs = (int?)outfitNode.Attribute("legs") ?? 0,

                Feet = (int?)outfitNode.Attribute("feet") ?? 0
            };

            XElement voicesNode = npcNode.Element("voices");

            if (voicesNode != null)
            {
                npc.Voices = new List<VoiceItem>();

                foreach (var voiceNode in voicesNode.Elements() )
                {
                    npc.Voices.Add(new VoiceItem() 
                    { 
                        Sentence = (string)voiceNode.Attribute("sentence")
                    } );
                }
            }

            return npc;
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
        public List<VoiceItem> Voices { get; set; }
    }
}