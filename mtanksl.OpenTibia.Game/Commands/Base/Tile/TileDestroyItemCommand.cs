using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TileDestroyItemCommand : Command
    {
        public TileDestroyItemCommand(Tile tile, Item item)
        {
            Tile = tile;

            Item = item;
        }

        public Tile Tile { get; set; }

        public Item Item { get; set; }

        public override void Execute(Context context)
        {
            Command command = context.TransformCommand(new TileRemoveItemCommand(Tile, Item) );

            command.Completed += (s, e) =>
            {
                context.Server.ItemFactory.Destroy(Item);

                base.OnCompleted(e.Context);
            };

            command.Execute(context);
        }
    }
}