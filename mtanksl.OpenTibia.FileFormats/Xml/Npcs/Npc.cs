using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Npcs
{
    public class Npc
    {
        public static Npc Load(XElement npcNode)
        {
            Npc npc = new Npc();

            npc.Name = (string)npcNode.Attribute("name");

            npc.NameDisplayed = (string)npcNode.Attribute("nameDisplayed");

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
                npc.Voices = new VoiceCollection()
                {
                    Interval = (int)voicesNode.Attribute("interval"),

                    Chance = Math.Max(0, Math.Min(100, (double)voicesNode.Attribute("chance") ) ),

                    Items = new List<VoiceItem>()
                };

                foreach (var voiceNode in voicesNode.Elements("voice") )
                {
                    npc.Voices.Items.Add(new VoiceItem() 
                    { 
                        Sentence = (string)voiceNode.Attribute("sentence")
                    } );
                }
            }

            return npc;
        }

        public string Name { get; set; }

        public string NameDisplayed { get; set; }

        public string NameDescription { get; set; }

        public int Speed { get; set; }

        public Health Health { get; set; }

        public Look Look { get; set; }

        public VoiceCollection Voices { get; set; }
    }
}