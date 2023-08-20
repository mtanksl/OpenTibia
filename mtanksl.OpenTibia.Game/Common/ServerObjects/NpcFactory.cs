using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Npcs;
using OpenTibia.Game.GameObjectScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Npc = OpenTibia.Common.Objects.Npc;

namespace OpenTibia.Game
{
    public class NpcFactory
    {
        private Server server;

        public NpcFactory(Server server, NpcFile npcFile)
        {
            this.server = server;

            metadatas = new Dictionary<string, NpcMetadata>(npcFile.Npcs.Count);

            foreach (var xmlNpc in npcFile.Npcs)
            {
                metadatas.Add(xmlNpc.Name, new NpcMetadata()
                {
                    Name = xmlNpc.Name,

                    Speed = (ushort)xmlNpc.Speed,

                    Health = (ushort)xmlNpc.Health.Now,

                    MaxHealth = (ushort)xmlNpc.Health.Max,

                    Outfit = new Outfit(xmlNpc.Look.Type, xmlNpc.Look.Head, xmlNpc.Look.Body, xmlNpc.Look.Legs, xmlNpc.Look.Feet, Addon.None),

                    Sentences = xmlNpc.Voices?.Select(v => v.Sentence).ToArray()
                } ); 
            }

            scripts = new Dictionary<string, GameObjectScript<string, Npc> >();

            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(GameObjectScript<string, Npc>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                GameObjectScript<string, Npc> script = (GameObjectScript<string, Npc>)Activator.CreateInstance(type);

                scripts.Add(script.Key, script);
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

        private Dictionary<string, GameObjectScript<string, Npc> > scripts;

        public GameObjectScript<string, Npc> GetNpcScript(string name)
        {
            GameObjectScript<string, Npc> script;

            if (scripts.TryGetValue(name, out script) )
            {
                return script;
            }
            
            if (scripts.TryGetValue("", out script) )
            {
                return script;
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

            Npc npc = new Npc(metadata);

            npc.Spawn = spawn;

            server.GameObjects.AddGameObject(npc);

            GameObjectScript<string, Npc> script = GetNpcScript(npc.Name);

            if (script != null)
            {
                script.Start(npc);
            }
            
            return npc;
        }

        public bool Detach(Npc npc)
        {
            if (server.GameObjects.RemoveGameObject(npc) )
            {
                GameObjectScript<string, Npc> script = GetNpcScript(npc.Name);

                if (script != null)
                {
                    script.Stop(npc);
                }

                return true;
            }

            return false;
        }

        public void Destroy(Npc npc)
        {
            server.GameObjectComponents.ClearComponents(npc);

            server.GameObjectEventHandlers.ClearEventHandlers(npc);
        }
    }
}