using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseExcludePlayerCommand : IncomingCommand
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
            // #x <player>

            PrivateChannel privateChannel = Context.Server.Channels.GetPrivateChannel(Player);

            if (privateChannel != null)
            {
                Player observer = Context.Server.GameObjects.GetPlayerByName(Name);

                if (observer != null && observer != Player)
                {              
                    if (privateChannel.ContainsInvitation(observer) )
                    {
                        privateChannel.RemoveInvitation(observer);

                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, observer.Name + " has been excluded.") );

                        return Promise.Completed;
                    }
                    else if (privateChannel.ContainerMember(observer) )
                    {
                        privateChannel.RemoveMember(observer);

                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(MessageMode.Look, observer.Name + " has been excluded.") );

                        Context.AddPacket(observer, new CloseChannelOutgoingPacket(privateChannel.Id) );

                        return Promise.Completed;
                    }
                }
            }

            return Promise.Break;
        }
    }
}