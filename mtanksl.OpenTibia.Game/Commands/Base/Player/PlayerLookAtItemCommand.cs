using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerLookAtItemCommand : Command
    {
        public PlayerLookAtItemCommand(Player player, Item item)
        {
            Player = player;

            Item = item;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            context.WritePacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see nothing special.") );

            base.OnCompleted(context);
        }
    }
}