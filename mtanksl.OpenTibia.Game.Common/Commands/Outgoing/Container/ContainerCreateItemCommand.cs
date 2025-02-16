using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Commands
{
    public class ContainerCreateItemCommand : CommandResult<Item>
    {
        public ContainerCreateItemCommand(Container container, ushort openTibiaId, byte typeCount)
        {
            Container = container;

            OpenTibiaId = openTibiaId;

            TypeCount = typeCount;
        }

        public Container Container { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte TypeCount { get; set; }

        public override PromiseResult<Item> Execute()
        {
            Item item = Context.Server.ItemFactory.Create(OpenTibiaId, TypeCount);

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