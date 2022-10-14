using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class PlayerLookItemCommand : Command
    {
        public PlayerLookItemCommand(Player player, Item item)
        {
            Player = player;

            Item = item;
        }

        public Player Player { get; set; }

        public Item Item { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.GreenCenterGameWindowAndServerLog, "You see nothing special. (Id: " + Item.Metadata.OpenTibiaId + ")") );

                resolve(context);
            } );
        }
    }
}