using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerLogoutCommand : Command
    {
        public PlayerLogoutCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Player.Health == 0)
            {
                Player.Health = Player.MaxHealth;

                Player.Mana = Player.MaxMana;

                Player.Direction = Direction.South;

                Player.Spawn = Player.Town;

                Context.AddPacket(Player, new OpenYouAreDeathDialogOutgoingPacket() );
            }
            else
            {
                Player.Spawn = Player.Tile;

                Context.Disconnect(Player);
            }

            Context.AddEvent(new PlayerLogoutEventArgs(Player.Tile, Player) );

            return Promise.Completed;
        }
    }
}