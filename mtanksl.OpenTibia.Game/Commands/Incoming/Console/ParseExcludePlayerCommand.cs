using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseExcludePlayerCommand : Command
    {
        public ParseExcludePlayerCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override Promise Execute()
        {
            PrivateChannel privateChannel = Context.Server.Channels.GetPrivateChannelByOwner(Player);

            if (privateChannel != null)
            {
                Player observer = Context.Server.GameObjects.GetPlayers()
                    .Where(p => p.Name == Name)
                    .FirstOrDefault();

                if (observer != null && observer != Player)
                {              
                    if (privateChannel.ContainsInvitation(observer) )
                    {
                        privateChannel.RemoveInvitation(observer);

                        Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been excluded.") );

                        return Promise.Completed;
                    }
                    else if (privateChannel.ContainsPlayer(observer) )
                    {
                        privateChannel.RemovePlayer(observer);

                        Context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, observer.Name + " has been excluded.") );

                        Context.AddPacket(observer.Client.Connection, new CloseChannelOutgoingPacket(privateChannel.Id) );

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}