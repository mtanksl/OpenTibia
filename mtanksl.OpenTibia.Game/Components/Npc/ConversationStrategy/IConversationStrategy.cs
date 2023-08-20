using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Components
{
    public interface IConversationStrategy
    {
        Promise Greeting(Npc npc, Player player);
        
        Promise Busy(Npc npc, Player player);

        Promise Say(Npc npc, Player player, string message);
        
        Promise Farewell(Npc npc, Player player);
        
        Promise Dismiss(Npc npc, Player player);
    }
}