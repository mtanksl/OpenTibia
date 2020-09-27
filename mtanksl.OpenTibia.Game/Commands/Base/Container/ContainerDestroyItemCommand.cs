using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class ContainerDestroyItemCommand : Command
    {
        public ContainerDestroyItemCommand(Container container, Item item)
        {
            Container = container;

            Item = item;
        }

        public Container Container { get; set; }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            Command command = context.TransformCommand(new ContainerRemoveItemCommand(Container, Item) );

            command.Completed += (s, e) =>
            {
                e.Context.Server.ItemFactory.Destroy(Item);

                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}