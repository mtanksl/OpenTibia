using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseLookFromContainerCommand : ParseLookCommand
    {
        public ParseLookFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort tibiaId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            TibiaId = tibiaId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort TibiaId { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.Containers.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item item = fromContainer.GetContent(FromContainerIndex) as Item;

                if (item != null && item.Metadata.TibiaId == TibiaId)
                {
                    return Context.AddCommand(new PlayerLookItemCommand(this, Player, item) );
                }
            }

            return Promise.Break;
        }
    }
}