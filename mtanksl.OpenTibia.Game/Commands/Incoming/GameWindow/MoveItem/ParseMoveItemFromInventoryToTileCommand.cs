using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromInventoryToTileCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromInventoryToTileCommand(Player player, byte fromSlot, ushort tibiaId, Position toPosition, byte count) : base(player)
        {
            FromSlot = fromSlot;

            TibiaId = tibiaId;

            ToPosition = toPosition;

            Count = count;
        }

        public byte FromSlot { get; set; }

        public ushort TibiaId { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            Item fromItem = fromInventory.GetContent(FromSlot) as Item;

            if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
            {
                Tile toTile = Context.Server.Map.GetTile(ToPosition);

                if (toTile != null)
                {
                    if (IsMoveable(fromItem, Count) )
                    {
                        return Context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toTile, 0, Count, true) );
                    }
                }
            }

            return Promise.Break;
        }
    }
}