using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseRotateItemFromContainerCommand : ParseRotateItemCommand
    {
        public ParseRotateItemFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

                if (fromContainer != null)
                {
                    Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                    if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                    {
                        if ( IsRotatable(context, fromItem) )
                        {
                            context.AddCommand(new PlayerRotateItemCommand(Player, fromItem) ).Then(ctx =>
                            {
                                resolve(ctx);
                            } );
                        }
                    }
                }
            } );            
        }
    }
}