using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseLookFromContainerCommand : ParseLookCommand
    {
        public ParseLookFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item item = fromContainer.GetContent(FromContainerIndex) as Item;

                if (item != null && item.Metadata.TibiaId == ItemId)
                {
                    return Context.AddCommand(new PlayerLookItemCommand(Player, item) );
                }
            }

            return Promise.Break;
        }
    }
}