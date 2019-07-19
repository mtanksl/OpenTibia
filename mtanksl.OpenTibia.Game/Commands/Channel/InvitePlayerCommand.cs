using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
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

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Player observer = server.Map.GetPlayers().Where(p => p.Name == Name).FirstOrDefault();

            //Act

            if (observer != null)
            {
                if (observer != Player)
                {
                    PrivateChannel privateChannel = server.Channels.GetPrivateChannel(Player);

                    if (privateChannel != null)
                    {
                        if ( !privateChannel.ContainsInvitation(observer) )
                        {
                            if ( !privateChannel.ContainsPlayer(observer) )
                            {
                                privateChannel.AddInvitation(observer);

                                //Notify

                                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been invited.") );

                                context.Write(observer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " invites you to his private chat channel." ) );
                            }
                        }
                    }
                }
            }
        }
    }
}