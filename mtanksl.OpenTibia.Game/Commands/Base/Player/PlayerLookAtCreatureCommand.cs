using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerLookAtCreatureCommand : Command
    {
        public PlayerLookAtCreatureCommand(Player player, Creature creature)
        {
            Player = player;

            Creature = creature;
        }

        public Player Player { get; set; }

        public Creature Creature { get; set; }

        public override void Execute(Context context)
        {
            if (Player == Creature)
            {
                context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see yourself.") );
            }
            else
            {
                context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see " + Creature.Name + ".") );
            }

            base.OnCompleted(context);
        }
    }
}