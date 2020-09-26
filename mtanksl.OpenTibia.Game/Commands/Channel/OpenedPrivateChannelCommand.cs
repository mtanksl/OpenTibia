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
            Player observer = context.Server.GameObjects.GetPlayers()
                .Where(p => p.Name == Name)
                .FirstOrDefault();
            
            if (observer != null && observer != Player)
            {
                context.AddPacket(Player.Client.Connection, new OpenPrivateChannelOutgoingPacket(Name) );

                base.OnCompleted(context);
            }
        }
    }
}