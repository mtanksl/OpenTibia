using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class OpenedPrivateChannelCommand : Command
    {
        public OpenedPrivateChannelCommand(Player player, string name)
        {
            Player = player;

            Name = name;
        }

        public Player Player { get; set; }

        public string Name { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Player observer = server.Map.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();

            //Act
            
            if (observer != null)
            {
                if (observer != Player)
                {
                    //Notify

                    context.Write(Player.Client.Connection, new OpenPrivateChannelOutgoingPacket(Name) );
                }
            }

            base.Execute(server, context);
        }
    }
}