using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToInventoryCommand : Command
    {
        public MoveItemFromTileToInventoryCommand(Player player, Position fromPosition, byte fromIndex, byte toSlot)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ToSlot = toSlot;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public byte ToSlot { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            Item fromItem = (Item)fromTile.GetContent(FromIndex);

            Inventory toInventory = Player.Inventory;

            Item toItem = (Item)toInventory.GetContent(ToSlot);

            //Act

            if (toItem == null)
            {
                fromTile.RemoveContent(fromItem);

                toInventory.AddContent(ToSlot, fromItem);

                //Notify

                foreach (var observer in server.Map.GetPlayers() )
                {
                    if (observer.Tile.Position.CanSee(fromTile.Position) )
                    {
                         context.Write(observer.Client.Connection, new ThingRemoveOutgoingPacket(fromTile.Position, FromIndex) );
                    }
                }

                context.Write(Player.Client.Connection, new SlotAddOutgoingPacket( (Slot)ToSlot, fromItem ) );
            }
            else
            {
                //Notify

                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.SorryNotPossible) );
            }
        }
    }
}