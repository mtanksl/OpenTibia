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

        public override void Execute(Context context)
        {
            //Arrange

            Player observer = context.Server.Map.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();
            
            if (observer != null && observer != Player)
            {
                //Notify

                context.AddPacket(Player.Client.Connection, new OpenPrivateChannelOutgoingPacket(Name) );

                base.Execute(context);
            }
        }
    }
}