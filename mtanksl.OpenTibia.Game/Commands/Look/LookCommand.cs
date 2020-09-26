using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public abstract class LookCommand : Command
    {
        public LookCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        protected void LookItem(Item item, Context context)
        {
            context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see nothing special.") );

            base.OnCompleted(context);
        }

        protected void LookCreature(Creature creature, Context context)
        {
            if (Player == creature)
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see yourself.") );
            }
            else
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see " + creature.Name + ".") );
            }

            base.OnCompleted(context);
        }
    }
}