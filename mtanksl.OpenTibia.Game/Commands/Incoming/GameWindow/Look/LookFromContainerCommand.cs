using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class LookFromContainerCommand : LookCommand
    {
        public LookFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId) : base(player)
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
                    Item item = fromContainer.GetContent(FromContainerIndex) as Item;

                    if (item != null && item.Metadata.TibiaId == ItemId)
                    {
                        context.AddCommand(new PlayerLookItemCommand(Player, item) ).Then(ctx =>
                        {
                            resolve(ctx);
                        } );
                    }
                }
            } );
        }
    }
}