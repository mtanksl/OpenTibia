using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ContainerCreateItemCommand : CommandResult<Item>
    {
        public ContainerCreateItemCommand(Container container, ushort openTibiaId, byte count)
        {
            Container = container;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Container Container { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override PromiseResult<Item> Execute()
        {
            return PromiseResult<Item>.Run( (resolve, reject) =>
            {
                Item item = context.Server.ItemFactory.Create(OpenTibiaId, Count);

                if (item != null)
                {
                    context.AddCommand(new ContainerAddItemCommand(Container, item) );
                }

                resolve(context, item);
            } );
        }
    }
}