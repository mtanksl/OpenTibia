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
            Item item = Context.Server.ItemFactory.Create(OpenTibiaId, Count);

            if (item != null)
            {
                Context.Server.ItemFactory.Attach(item);

                return Context.AddCommand(new ContainerAddItemCommand(Container, item) ).Then( () =>
                {
                    return Promise.FromResult(item);
                } );
            }

            return Promise.FromResult(item);
        }
    }
}