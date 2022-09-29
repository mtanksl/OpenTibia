using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToTileCommand : MoveItemCommand
    {
        public MoveItemFromTileToTileCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, Position toPosition, byte count) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToPosition = toPosition;

            Count = count;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            Tile fromTile = context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Tile toTile = context.Server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        if (IsMoveable(context, fromItem, Count) && CanThrow(context, fromTile, toTile) )
                        {
                            MoveItem(context, fromItem, toTile, 0, Count);
                        }
                    }
                }
            }
        }
    }
}