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
            return Promise.Run( (resolve, reject) =>
            {
                Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

                if (fromContainer != null)
                {
                    Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                    if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                    {
                        Creature toCreature = context.Server.GameObjects.GetCreature(ToCreatureId);

                        if (toCreature != null)
                        {
                            if ( IsUseable(context, fromItem) )
                            {
                                context.AddCommand(new PlayerUseItemWithCreatureCommand(Player, fromItem, toCreature) ).Then(ctx =>
                                {
                                    resolve(ctx);
                                } );
                            }
                        }
                    }
                }
            } );
        }
    }
}