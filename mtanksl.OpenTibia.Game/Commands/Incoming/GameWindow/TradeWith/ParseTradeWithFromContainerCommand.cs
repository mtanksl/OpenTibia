using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ParseTradeWithFromContainerCommand : ParseTradeWithCommand
    {
        public ParseTradeWithFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort tibiaId, uint creatureId) : base(player)
        {
            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            TibiaId = tibiaId;

            ToCreatureId = creatureId;
        }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort TibiaId { get; set; }

        public uint ToCreatureId { get; set; }

        public override Promise Execute()
        {
            Container fromContainer = Player.Client.Containers.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == TibiaId)
                {
                    Player toPlayer = Context.Server.GameObjects.GetPlayer(ToCreatureId);

                    if (toPlayer != null && toPlayer != Player)
                    {
                        if ( IsPickupable(fromItem) )
                        {
                            return Context.AddCommand(new PlayerTradeWithCommand(Player, fromItem, toPlayer) );
                        }
                    }
                }
            }

            return Promise.Break;
        }
    }
}