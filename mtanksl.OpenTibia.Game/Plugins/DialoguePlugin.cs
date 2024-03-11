using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Game.Components
{
    public abstract class DialoguePlugin : Plugin
    {
        public abstract PromiseResult<bool> ShouldGreet(Npc npc, Player player, string message);

        public abstract PromiseResult<bool> ShouldFarewell(Npc npc, Player player, string message);

        public abstract Promise OnGreet(Npc npc, Player player);

        public abstract Promise OnBusy(Npc npc, Player player);

        public abstract Promise OnSay(Npc npc, Player player, string message);

        public abstract Promise OnBuy(Npc npc, Player player, ushort openTibiaId, byte type, byte count, int price, bool ignoreCapacity, bool buyWithBackpacks);

        public abstract Promise OnSell(Npc npc, Player player, ushort openTibiaId, byte type, byte count, int price, bool keepEquipped);

        public abstract Promise OnFarewell(Npc npc, Player player);

        public abstract Promise OnDisappear(Npc npc, Player player);

        public abstract Promise OnEnqueue(Npc npc, Player player);

        public abstract Promise OnDequeue(Npc npc, Player player);
    }
}