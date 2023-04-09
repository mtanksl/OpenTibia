using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseTradeWithFromContainerCommand : ParseTradeWithCommand
    {
        public ParseTradeWithFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, uint creatureId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToCreatureId = creatureId;
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
                        Player toPlayer = context.Server.GameObjects.GetPlayer(ToCreatureId);

                        if (toPlayer != null && toPlayer != Player)
                        {
                            resolve(context);
                        }
                    }
                }            
            } );
        }
    }
}