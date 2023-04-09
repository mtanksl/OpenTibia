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
            return Promise.Run( (resolve, reject) =>
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

                            context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been invited.") );

                            context.AddPacket(observer.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, Player.Name + " invites you to his private chat channel." ) );

                            resolve(context);
                        }
                    }
                }
            } );
        }
    }
}