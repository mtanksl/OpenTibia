using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseInvitePlayerChannelCommand : Command
    {
        public ParseInvitePlayerChannelCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override Promise Execute()
        {
            // #i <player>

            PrivateChannel privateChannel = Context.Server.Channels.GetPrivateChannel(Player);

            if (privateChannel != null)
            {
                Player observer = Context.Server.GameObjects.GetPlayers()
                    .Where(p => p.Name == Name)
                    .FirstOrDefault();

                if (observer != null && observer != Player)
                {
                    if ( !privateChannel.ContainsPlayer(observer) && !privateChannel.ContainsInvitation(observer) )
                    {
                        privateChannel.AddInvitation(observer);

                        Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been invited.") );

                        Context.AddPacket(observer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " invites you to his private chat channel." ) );

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}