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

        public NpcFactory(Server server)
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

                    Speed = (ushort)xmlNpc.Speed,

                    Health = (ushort)xmlNpc.Health.Now,

                    MaxHealth = (ushort)xmlNpc.Health.Max,

                    Outfit = new Outfit(xmlNpc.Look.Type, xmlNpc.Look.Head, xmlNpc.Look.Body, xmlNpc.Look.Legs, xmlNpc.Look.Feet, Addon.None),

                    Sentences = xmlNpc.Voices?.Select(v => v.Sentence).ToArray()
                } ); 
            }

            gameObjectScripts = new Dictionary<string, GameObjectScript<string, Npc> >();
#if AOT
            foreach (var gameObjectScript in _AotCompilation.Npcs)
            {
                gameObjectScripts.Add(gameObjectScript.Key, gameObjectScript);
            }
#else
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(GameObjectScript<string, Npc>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                GameObjectScript<string, Npc> gameObjectScript = (GameObjectScript<string, Npc>)Activator.CreateInstance(type);

                gameObjectScripts.Add(gameObjectScript.Key, gameObjectScript);
            }
#endif
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

        private Dictionary<string, GameObjectScript<string, Npc> > gameObjectScripts;

        public GameObjectScript<string, Npc> GetNpcGameObjectScript(string name)
        {
            GameObjectScript<string, Npc> gameObjectScript;

            if (gameObjectScripts.TryGetValue(name, out gameObjectScript) )
            {
                return gameObjectScript;
            }
            
            if (gameObjectScripts.TryGetValue("", out gameObjectScript) )
            {
                return gameObjectScript;
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

            GameObjectScript<string, Npc> gameObjectScript = GetNpcGameObjectScript(npc.Name);

            if (gameObjectScript != null)
            {
                gameObjectScript.Start(npc);
            }
        }

        public bool Detach(Npc npc)
        {
            if (server.GameObjects.RemoveGameObject(npc) )
            {
                GameObjectScript<string, Npc> gameObjectScript = GetNpcGameObjectScript(npc.Name);

                if (gameObjectScript != null)
                {
                    gameObjectScript.Stop(npc);
                }

                return true;
            }

            return false;
        }

        public void ClearComponentsAndEventHandlers(Npc npc)
        {
            server.GameObjectComponents.ClearComponents(npc);

            server.GameObjectEventHandlers.ClearEventHandlers(npc);
        }
    }
}