using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithItemFromHotkeyToContainerCommand : ParseUseItemWithItemCommand
    {
        public ParseUseItemWithItemFromHotkeyToContainerCommand(Player player, ushort fromItemId, byte toContainerId, byte toContainerIndex, ushort toItemId) :base(player)
        {
            FromItemId = fromItemId;

            ToContainerId = toContainerId;

            ToContainerIndex = toContainerIndex;

            ToItemId = toItemId;
        }

        public ushort FromItemId { get; set; }

        public byte ToContainerId { get; set; }

        public byte ToContainerIndex { get; set; }

        public ushort ToItemId { get; set; }

        public override Promise Execute()
        {
            Inventory fromInventory = Player.Inventory;

            foreach (var pair in fromInventory.GetIndexedContents() )
            {
                Item fromItem = (Item)pair.Value;

                if (fromItem.Metadata.TibiaId == FromItemId)
                {
                    Container toContainer = Player.Client.ContainerCollection.GetContainer(ToContainerId);

                    if (toContainer != null)
                    {
                        Item toItem = toContainer.GetContent(ToContainerIndex) as Item;

                        if (toItem != null && toItem.Metadata.TibiaId == ToItemId)
                        {
                            if ( IsUseable(Context, fromItem) )
                            {
                                return Context.AddCommand(new PlayerUseItemWithItemCommand(Player, fromItem, toItem) );
                            }
                        }
                    }

                    break;
                }
            }

            return Promise.Break;
        }
    }
}