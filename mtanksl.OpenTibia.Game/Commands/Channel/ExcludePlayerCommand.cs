using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ExcludePlayerCommand : Command
    {
        public ExcludePlayerCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            PrivateChannel privateChannel = server.Channels.GetPrivateChannelByOwner(Player);

            if (privateChannel != null)
            {
                Player observer = server.Map.GetPlayers()
                    .Where(p => p.Name == Name)
                    .FirstOrDefault();

                if (observer != null && observer != Player)
                {              
                    if (privateChannel.ContainsInvitation(observer) )
                    {
                        //Act

                        privateChannel.RemoveInvitation(observer);

                        //Notify

                        context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been excluded.") );

                        base.Execute(server, context);
                    }
                    else if (privateChannel.ContainsPlayer(observer) )
                    {
                        //Act

                        privateChannel.RemovePlayer(observer);

                        //Notify

                        context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been excluded.") );

                        context.Write(observer.Client.Connection, new CloseChannelOutgoingPacket(privateChannel.Id) );

                        base.Execute(server, context);
                    }
                }
            }
        }
    }
}