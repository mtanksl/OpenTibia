using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class MoveItemFromTileToContainerCommand : MoveItemCommand
    {
        public MoveItemFromTileToContainerCommand(Player player, Position fromPosition, byte fromIndex, ushort itemId, byte toContainerId, byte toContainerIndex, byte count) : base(player)
        {
            FromPosition = fromPosition;

            FromIndex = fromIndex;

            ItemId = itemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            Count = count;
        }

        public Position FromPosition { get; set; }

        public byte FromIndex { get; set; }

        public ushort ItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            Tile fromTile = context.Server.Map.GetTile(FromPosition);

            if (fromTile != null)
            {
                Item fromItem = fromTile.GetContent(FromIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                    if (toContainer != null)
                    {
                        if ( IsMoveable(fromItem, context) &&

                             IsNextTo(fromTile, context) &&
                            
                             IsPickupable(fromItem, context) &&
                             
                             IsPossible(fromItem, toContainer, context) && 
                             
                             IsEnoughtSpace(fromItem, toContainer, context) )
                        {
                            MoveItem(fromItem, toContainer, ToContainerIndex, Count, context);
                        }
                    }
                }
            }
        }
    }
}