using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Xml.Npcs;
using System.Collections.Generic;
using Npc = OpenTibia.Common.Objects.Npc;

namespace OpenTibia.Game
{
    public class NpcFactory
    {
        private Dictionary<string, NpcMetadata> metadatas;

        public NpcFactory(NpcFile npcFile)
        {
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
        
        public Npc Create(string name)
        {
            NpcMetadata metadata;

            if ( !metadatas.TryGetValue(name, out metadata) )
            {
                return null;
            }

            return new Npc(metadata);
        }
    }
}