using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerLogoutCommand : Command
    {
        public PlayerLogoutCommand(Player player, Tile tile)
        {
            Player = player;

            Tile = tile;
        }

        public Player Player { get; set; }

        public Tile Tile { get; set; }

        public override Promise Execute()
        {
            if (Player.Health == 0)
            {
                Context.AddPacket(Player.Client.Connection, new OpenYouAreDeathDialogOutgoingPacket() );
            }
            else
            {
                Context.Disconnect(Player.Client.Connection);
            }

            Context.AddEvent(new PlayerLogoutEventArgs(Tile, Player) );

            return Promise.Completed;
        }
    }
}