using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Xml.Npcs;
using Npc = OpenTibia.Common.Objects.Npc;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface INpcFactory
    {
        void Start(NpcFile npcFile);

        NpcMetadata GetNpcMetadata(string name);

        Npc Create(string name, Tile spawn);

        void Attach(Npc npc);

        bool Detach(Npc npc);

        void ClearComponentsAndEventHandlers(Npc npc);
    }
}