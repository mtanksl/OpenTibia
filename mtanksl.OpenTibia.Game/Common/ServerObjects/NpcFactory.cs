using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Npcs;
using OpenTibia.Game.GameObjectScripts;
using OpenTibia.Game.Plugins;
using System.Collections.Generic;
using System.Linq;
using Npc = OpenTibia.Common.Objects.Npc;
using VoiceCollection = OpenTibia.Common.Objects.VoiceCollection;
using VoiceItem = OpenTibia.Common.Objects.VoiceItem;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class NpcFactory : INpcFactory
    {
        private IServer server;

        public NpcFactory(IServer server)
        {
            this.server = server;            
        }

        public void Start(NpcFile npcFile)
        {
            metadatas = new Dictionary<string, NpcMetadata>(npcFile.Npcs.Count);

            foreach (var xmlNpc in npcFile.Npcs)
            {
                metadatas.Add(xmlNpc.Name, new NpcMetadata()
                {
                    Name = xmlNpc.Name,

                    NameDisplayed = xmlNpc.NameDisplayed,

                    Description = xmlNpc.NameDescription,

                    Speed = (ushort)xmlNpc.Speed,

                    Health = (ushort)xmlNpc.Health.Now,

                    MaxHealth = (ushort)xmlNpc.Health.Max,

                    Light = xmlNpc.Light == null ? Light.None : new Light( (byte)xmlNpc.Light.Level, (byte)xmlNpc.Light.Color),

                    Outfit = xmlNpc.Look.TypeEx != 0 ? new Outfit(xmlNpc.Look.TypeEx) : new Outfit(xmlNpc.Look.Type, xmlNpc.Look.Head, xmlNpc.Look.Body, xmlNpc.Look.Legs, xmlNpc.Look.Feet, (Addon)xmlNpc.Look.Addon, xmlNpc.Look.Mount),

                    Voices = (xmlNpc.Voices == null || xmlNpc.Voices.Items == null || xmlNpc.Voices.Items.Count == 0) ? null : new VoiceCollection()
                    {
                        Interval = xmlNpc.Voices.Interval,

                        Chance = xmlNpc.Voices.Chance,

                        Items = xmlNpc.Voices.Items.Select(v => new VoiceItem() { Sentence = v.Sentence, Yell = false } ).ToArray()
                    }
                } ); 
            }
        }

        private Dictionary<string, NpcMetadata> metadatas;

        public NpcMetadata GetNpcMetadata(string name)
        {
            NpcMetadata metadata;

            if (metadatas.TryGetValue(name, out metadata) )
            {
                return metadata;
            }

            return null;
        }

        public Npc Create(string name, Tile spawn)
        {
            NpcMetadata metadata = GetNpcMetadata(name);

            if (metadata == null)
            {
                return null;
            }

            Npc npc = new Npc(metadata)
            {
                Town = spawn,

                Spawn = spawn
            };

            return npc;
        }

        public void Attach(Npc npc)
        {
            npc.IsDestroyed = false;

            server.GameObjects.AddGameObject(npc);

            GameObjectScript<Npc> gameObjectScript = server.GameObjectScripts.GetNpcGameObjectScript(npc.Name) ?? server.GameObjectScripts.GetNpcGameObjectScript("");

            if (gameObjectScript != null)
            {
                gameObjectScript.Start(npc);
            }

            NpcCreationPlugin plugin = server.Plugins.GetNpcCreationPlugin(npc.Name) ?? server.Plugins.GetNpcCreationPlugin("");

            if (plugin != null)
            {
                if (plugin.OnStart(npc).Result)
                {
                    //
                }
            }
        }

        public bool Detach(Npc npc)
        {
            if (server.GameObjects.RemoveGameObject(npc) )
            {
                GameObjectScript<Npc> gameObjectScript = server.GameObjectScripts.GetNpcGameObjectScript(npc.Name) ?? server.GameObjectScripts.GetNpcGameObjectScript("");

                if (gameObjectScript != null)
                {
                    gameObjectScript.Stop(npc);
                }

                NpcCreationPlugin plugin = server.Plugins.GetNpcCreationPlugin(npc.Name) ?? server.Plugins.GetNpcCreationPlugin("");

                if (plugin != null)
                {
                    if (plugin.OnStop(npc).Result)
                    {
                        //
                    }
                }

                return true;
            }

            return false;
        }

        public void ClearComponentsAndEventHandlers(Npc npc)
        {
            server.GameObjectComponents.ClearComponents(npc);

            server.GameObjectEventHandlers.ClearEventHandlers(npc);

            server.PositionalEventHandlers.ClearEventHandlers(npc);
        }
    }
}