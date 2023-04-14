using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class ParseOpenedPrivateChannelCommand : Command
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
            Player observer = Context.Server.GameObjects.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();
            
            if (observer != null && observer != Player)
            {
                Context.AddPacket(Player.Client.Connection, new OpenPrivateChannelOutgoingPacket(Name) );

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}