using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenedPrivateChannelCommand : IncomingCommand
    {
        public ParseOpenedPrivateChannelCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override Promise Execute()
        {
            Player observer = Context.Server.GameObjects.GetPlayerByName(Name);

            if (observer != null && observer != Player)
            {
                Context.AddPacket(Player, new OpenPrivateChannelOutgoingPacket(Name) );

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}