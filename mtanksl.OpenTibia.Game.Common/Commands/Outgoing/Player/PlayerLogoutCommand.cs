using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerLogoutCommand : Command
    {
        public PlayerLogoutCommand(Player player) : this(player, false)
        {

        }

        public PlayerLogoutCommand(Player player, bool death)
        {
            Player = player;

            Death = death;
        }

        public Player Player { get; set; }

        public bool Death { get; set; }

        public override Promise Execute()
        {
            if (Death)
            {
                Player.Spawn = Player.Town;

                Context.AddPacket(Player, new OpenYouAreDeathDialogOutgoingPacket(DeathType.Regular, 100) ); //TODO: FeatureFlag.DeathType
            }
            else
            {
                Player.Spawn = Player.Tile;

                Context.Disconnect(Player);
            }

            Context.AddEvent(new PlayerLogoutEventArgs(Player) );

            return Promise.Completed;
        }
    }
}