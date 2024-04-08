using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromContainerToTileCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromContainerToTileCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort fromTibiaId, Position toPosition, byte toIndex, ushort toTibiaId) :base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            FromTibiaId = fromTibiaId;

            ToPosition = toPosition;

            ToIndex = toIndex;

            ToTibiaId = toTibiaId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort FromTibiaId { get; set; }

        public Position ToPosition { get; set; }

        public byte ToIndex { get; set; }

        public ushort ToTibiaId { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.Containers.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == FromTibiaId)
                {
                    Tile toTile = Context.Server.Map.GetTile(ToPosition);

                    if (toTile != null)
                    {
                        switch (Player.Client.GetContent(toTile, ToIndex) )
                        {
                            case Item toItem:

                                if (toItem.Metadata.TibiaId == ToTibiaId)
                                {
                                    if ( IsUseable(fromItem) )
                                    {
                                        return Context.AddCommand(new PlayerUseItemWithItemCommand(this, Player, fromItem, toItem) );
                                    }
                                }

                                break;

                            case Creature toCreature:

                                if (ToTibiaId == 99)
                                {
                                    if ( IsUseable(fromItem) )
                                    {
                                        return Context.AddCommand(new PlayerUseItemWithCreatureCommand(this, Player, fromItem, toCreature) );
                                    }
                                }

                                break;
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}