using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public abstract class DialoguePlugin : Plugin
    {
        public abstract PromiseResult<bool> ShouldGreet(Npc npc, Player player, string message);

        public abstract PromiseResult<bool> ShouldFarewell(Npc npc, Player player, string message);

        public abstract Promise OnGreet(Npc npc, Player player);

        public abstract Promise OnBusy(Npc npc, Player player);

        public abstract Promise OnSay(Npc npc, Player player, string message);

        public abstract Promise OnFarewell(Npc npc, Player player);

        public abstract Promise OnDismiss(Npc npc, Player player);
    }
}