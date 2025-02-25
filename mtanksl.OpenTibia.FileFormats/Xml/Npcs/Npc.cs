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

            npc.Health = new HealthItem()
            {
                Now = (int)healthNode.Attribute("now"),

                Max = (int)healthNode.Attribute("max")
            };

            XElement lightNode = npcNode.Element("light");

            if (lightNode != null)
            {
                npc.Light = new LightItem()
                {
                    Level = (int?)lightNode.Attribute("level") ?? 0,

                    Color = (int?)lightNode.Attribute("color") ?? 0
                };
            }

            XElement lookNode = npcNode.Element("look");

            npc.Look = new LookItem()
            {
                TypeEx = (int?)lookNode.Attribute("typeex") ?? 0,

                Type = (int?)lookNode.Attribute("type") ?? 0,

                Head = (int?)lookNode.Attribute("head") ?? 0,

                Body = (int?)lookNode.Attribute("body") ?? 0,

                Legs = (int?)lookNode.Attribute("legs") ?? 0,

                Feet = (int?)lookNode.Attribute("feet") ?? 0,

                Addon = (int?)lookNode.Attribute("addon") ?? 0
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

        public HealthItem Health { get; set; }

        public LightItem Light { get; set; }

        public LookItem Look { get; set; }

        public VoiceCollection Voices { get; set; }
    }
}