using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class NpcEventHandler
    {
        public abstract Promise OnGreet(Npc npc, Player player);

        public abstract Promise OnGreetBusy(Npc npc, Player player);

        public abstract Promise OnSay(Npc npc, Player player, string message);

        public abstract Promise OnFarewell(Npc npc, Player player);

        public abstract Promise OnDisappear(Npc npc);
    }
}