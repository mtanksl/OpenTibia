using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseUseItemWithCreatureFromContainerCommand : ParseUseItemWithCreatureCommand
    {
        public ParseUseItemWithCreatureFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, uint toCreatureId) : base(player)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToCreatureId = toCreatureId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.Containers.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Creature toCreature = Context.Server.GameObjects.GetCreature(ToCreatureId);

                    if (toCreature != null)
                    {
                        if ( IsUseable(fromItem) )
                        {
                            return Context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) );
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}