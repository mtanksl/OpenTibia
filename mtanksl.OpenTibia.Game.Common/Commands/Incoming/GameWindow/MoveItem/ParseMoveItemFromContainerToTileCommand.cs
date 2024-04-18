using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ParseMoveItemFromContainerToTileCommand : ParseMoveItemCommand
    {
        public ParseMoveItemFromContainerToTileCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort tibiaId, Position toPosition, byte count) :base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            TibiaId = tibiaId;

            ToPosition = toPosition;

            Count = count;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort TibiaId { get; set; }

        public Position ToPosition { get; set; }

        public byte Count { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.Containers.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
                {
                    Tile toTile = Context.Server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        if (IsMoveable(fromItem, Count) )
                        {
                            return Context.AddCommand(new PlayerMoveItemCommand(Player, fromItem, toTile, 255, Count, true) );
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}