using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class UseItemWithCreatureFromContainerCommand : UseItemWithCreatureCommand
    {
        public UseItemWithCreatureFromContainerCommand(Player player, byte fromContainerId, byte fromContainerIndex, ushort itemId, uint toCreatureId)
        {
            Player = player;

            FromContainerId = fromContainerId;

            FromContainerIndex = fromContainerIndex;

            ItemId = itemId;

            ToCreatureId = toCreatureId;
        }

        public Player Player { get; set; }

        public byte FromContainerId { get; set; }

        public byte FromContainerIndex { get; set; }

        public ushort ItemId { get; set; }

        public uint ToCreatureId { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Container fromContainer = Player.Client.ContainerCollection.GetContainer(FromContainerId);

            if (fromContainer != null)
            {
                Item fromItem = fromContainer.GetContent(FromContainerIndex) as Item;

                if (fromItem != null && fromItem.Metadata.TibiaId == ItemId)
                {
                    Creature ToCreature = server.Map.GetCreature(ToCreatureId);

                    if (ToCreature != null)
                    {
                        //Act

                        base.Execute(server, context);
                    }
                }
            }
        }
    }
}