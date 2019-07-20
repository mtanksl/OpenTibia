using System;
using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToInventoryCommand : MoveItemCommand
    {
        public MoveItemFromTileToInventoryCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, byte toSlot, byte count)
        {
            Player = player;

            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToSlot = toSlot;

            Count = count;
        }

        public Player Player { get; set; }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ToSlot { get; set; }

        public byte Count { get; set; }
        
        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Tile fromTile = server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Inventory toInventory = Player.Inventory;

                    Item toItem = toInventory.GetContent(ToSlot) as Item;

                    if (toItem == null)
                    {
                        //Act

                        RemoveItem(fromTile, FromIndex, server, context);

                        AddItem(toInventory, ToSlot, fromItem, server, context);

                        base.Execute(server, context);
                    }
                }
            }
        }
    }
}