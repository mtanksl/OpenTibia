using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.CommandHandlers
{
    public class WelcomeHandler : EventHandlers.EventHandler<PlayerLoginEventArgs>
    {
        public override Promise Handle(PlayerLoginEventArgs e)
        {
            Context.AddPacket(e.Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindowAndServerLog, "Welcome to MTOTS.") );

            return Promise.Completed;
        }
    }
}