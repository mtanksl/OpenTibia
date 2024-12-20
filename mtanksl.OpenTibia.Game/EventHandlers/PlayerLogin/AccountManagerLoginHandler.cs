using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.CommandHandlers
{
    public class AccountManagerLoginHandler : EventHandlers.EventHandler<PlayerLoginEventArgs>
    {
        public override Promise Handle(PlayerLoginEventArgs e)
        {
            Context.AddPacket(e.Player, new ShowWindowTextOutgoingPacket(TextColor.TealDefault, "Type 'account' to manage your account and if you want to start over then type 'cancel'.") );

            return Promise.Completed;
        }
    }
}