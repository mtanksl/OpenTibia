using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public interface IConversationStrategy
    {
        void Start(Npc npc);

        PromiseResult<bool> ShouldGreet(Npc npc, Player player, string message);

        PromiseResult<bool> ShouldFarewell(Npc npc, Player player, string message);

        Promise OnGreet(Npc npc, Player player);
        
        Promise OnBusy(Npc npc, Player player);

        Promise OnSay(Npc npc, Player player, string message);
        
        Promise OnFarewell(Npc npc, Player player);
        
        Promise OnDismiss(Npc npc, Player player);

        void Stop();
    }
}