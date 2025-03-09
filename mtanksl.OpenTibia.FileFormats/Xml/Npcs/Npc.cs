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

                Addon = (int?)lookNode.Attribute("addon") ?? 0,

                Mount = (int?)lookNode.Attribute("mount") ?? 0
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

        public XDocument Serialize()
        {
            return Serialize(this);
        }

        private static XDocument Serialize(Npc npc)
        {
            var xml = new XDocument();

            var npcNode =
                new XElement("npc");

            xml.Add(npcNode);

            if ( !string.IsNullOrEmpty(npc.Name) )
            {
                npcNode.Add(new XAttribute("name", npc.Name) );
            }

            if ( !string.IsNullOrEmpty(npc.NameDisplayed) )
            {
                npcNode.Add(new XAttribute("nameDisplayed", npc.NameDisplayed) );
            }

            if ( !string.IsNullOrEmpty(npc.NameDescription) )
            {
                npcNode.Add(new XAttribute("nameDescription", npc.NameDescription) );
            }

            npcNode.Add(new XAttribute("speed", npc.Speed) );

            var healthNode =
                new XElement("health");

            npcNode.Add(healthNode);

            healthNode.Add(new XAttribute("now", npc.Health.Now) );

            healthNode.Add(new XAttribute("max", npc.Health.Max) );

            if (npc.Light != null)
            {
                var lightNode =
                    new XElement("light");

                npcNode.Add(lightNode);

                lightNode.Add(new XAttribute("level", npc.Light.Level) );

                lightNode.Add(new XAttribute("color", npc.Light.Color) );
            }

            var lookNode =
                new XElement("look");

            npcNode.Add(lookNode);

            lookNode.Add(new XAttribute("typeex", npc.Look.TypeEx) );

            lookNode.Add(new XAttribute("type", npc.Look.Type) );

            lookNode.Add(new XAttribute("head", npc.Look.Head) );

            lookNode.Add(new XAttribute("body", npc.Look.Body) );

            lookNode.Add(new XAttribute("legs", npc.Look.Legs) );

            lookNode.Add(new XAttribute("feet", npc.Look.Feet) );

            // lookNode.Add(new XAttribute("addon", npc.Look.Addon) );

            // lookNode.Add(new XAttribute("mount", npc.Look.Mount) );

            if (npc.Voices != null)
            {
                var voicesNode =
                    new XElement("voices");

                npcNode.Add(voicesNode);

                voicesNode.Add(new XAttribute("interval", npc.Voices.Interval) );

                voicesNode.Add(new XAttribute("chance", npc.Voices.Chance) );

                foreach (var voice in npc.Voices.Items)
                {
                    var voiceNode =
                        new XElement("voice");

                    voicesNode.Add(voiceNode);

                    voiceNode.Add(new XAttribute("sentence", voice.Sentence) );
                }
            }

            return xml;
        }

        public void Save(string path)
        {
            Save(this, path);
        }

        private static void Save(Npc npc, string path)
        {
            npc.Serialize().Save(path);
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