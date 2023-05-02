using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.FileFormats.Xml.Npcs;
using OpenTibia.Game.Components;
using System;
using System.Collections.Generic;
using System.Linq;
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
        }

        private Dictionary<string, NpcMetadata> metadatas;

        public Npc Create(string name)
        {
            NpcMetadata metadata;

            if ( !metadatas.TryGetValue(name, out metadata) )
            {
                return null;
            }

            Npc npc = new Npc(metadata);

            server.GameObjects.AddGameObject(npc);

            List<NonTargetAction> nonTargetActions = new List<NonTargetAction>();

            nonTargetActions.Add(new RandomWalkNonTargetAction(1) );

            if (npc.Metadata.Sentences != null)
            {
                nonTargetActions.Add(new NpcSayNonTargetAction(npc.Metadata.Sentences, TimeSpan.FromSeconds(30) ) );
            }

            if (npc.Name == "Aldee")
            {
                server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(nonTargetActions.ToArray(), new AldeeNpcEventHandler() ) );
            }
            else if (npc.Name == "Cipfried")
            {
                server.GameObjectComponents.AddComponent(npc, new NpcThinkBehaviour(nonTargetActions.ToArray(), new CipfriedNpcEventHandler() ) );
            }

            return npc;
        }

        public bool Detach(Npc npc)
        {
            return server.GameObjects.RemoveGameObject(npc);
        }

        public void Destroy(Npc npc)
        {
            server.GameObjectComponents.ClearComponents(npc);

            server.GameObjectEventHandlers.ClearEventHandlers(npc);
        }
    }
}