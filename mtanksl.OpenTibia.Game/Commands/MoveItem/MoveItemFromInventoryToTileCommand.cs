using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromInventoryToTileCommand : MoveItemCommand
    {
        public MoveItemFromInventoryToTileCommand(Player player, byte fromSlot, ushort itemId, Position toPosition, byte count)
        {
            Player = player;

            FromSlot = fromSlot;

            ItemId = itemId;

            ToPosition = toPosition;

            Count = count;
        }

        public Player Player { get; set; }

        public byte FromSlot { get; set; }

        public ushort ItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
            {
                Tile toTile = server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    //Act

                    Container parent = fromItem as Container;

                    if (parent != null)
                    {
                        CloseContainer(toTile, parent, server, context);
                    }

                    RemoveItem(fromInventory, FromSlot, server, context);

                    AddItem(toTile, fromItem, server, context);

                    base.Execute(server, context);
                }
            }
        }
    }
}