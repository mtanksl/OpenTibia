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
            return Promise.Run( (resolve, reject) =>
            {
                Player observer = context.Server.GameObjects.GetPlayers()
                    .Where(p => p.Name == Name)
                    .FirstOrDefault();
            
                if (observer != null && observer != Player)
                {
                    context.AddPacket(Player.Client.Connection, new OpenPrivateChannelOutgoingPacket(Name) );

                    resolve(context);
                }
            } );
        }
    }
}