using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Xml.Npcs;
using OpenTibia.Game.Components;
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

                    Health = xmlNpc.Health,

                    MaxHealth = xmlNpc.MaxHealth,

                    Outfit = xmlNpc.Outfit,

                    Speed = xmlNpc.Speed
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

            server.Components.AddComponent(npc, new AutoWalkBehaviour() );

            return npc;
        }

        public void Destroy(Npc npc)
        {
            server.GameObjects.RemoveGameObject(npc);

            foreach (var component in server.Components.GetComponents<Component>(npc).ToList() )
            {
                server.Components.RemoveComponent(npc, component);
            }
        }
    }
}