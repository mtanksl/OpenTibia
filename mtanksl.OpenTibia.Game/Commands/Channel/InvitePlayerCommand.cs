using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class InvitePlayerCommand : Command
    {
        public InvitePlayerCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override void Execute(Context context)
        {  
            PrivateChannel privateChannel = context.Server.Channels.GetPrivateChannelByOwner(Player);

            if (privateChannel != null)
            {
                Player observer = context.Server.GameObjects.GetPlayers()
                    .Where(p => p.Name == Name)
                    .FirstOrDefault();

                if (observer != null && observer != Player)
                {
                    if ( !privateChannel.ContainsPlayer(observer) && !privateChannel.ContainsInvitation(observer) )
                    {
                        privateChannel.AddInvitation(observer);

                        context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been invited.") );

                        context.WritePacket(observer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " invites you to his private chat channel." ) );

                        base.OnCompleted(context);
                    }
                }
            }
        }
    }
}