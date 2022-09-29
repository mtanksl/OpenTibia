using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithItemFromContainerToTileCommand : UseItemWithItemCommand
    {
        public UseItemWithItemFromContainerToTileCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort fromItemId, Position toPosition, byte toIndex, ushort toItemId) :base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            FromItemId = fromItemId;

            ToPosition = toPosition;

            ToIndex = toIndex;

            ToItemId = toItemId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort FromItemId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override void Execute(Context context)
        {
            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromItemId)
                {
                    Tile toTile = context.Server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        switch (toTile.GetContent(ToIndex) )
                        {
                            case Item toItem:

                                if (toItem.Metadata.TibiaId == ToItemId)
                                {
                                    if ( IsUseable(context, fromItem))
                                    {
                                        UseItemWithItem(context, fromItem, toItem);
                                    }
                                }

                                break;

                            case Creature toCreature:

                                if (ToItemId == 99)
                                {
                                    if ( IsUseable(context, fromItem))
                                    {
                                        UseItemWithCreature(context, fromItem, toCreature);
                                    }
                                }

                                break;
                        }
                    }
                }
            }
        }
    }
}