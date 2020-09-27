using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class TileCreateItemCommand : Command
    {
        public TileCreateItemCommand(Tile tile, ushort openTibiaId, byte count)
        {
            Tile = tile;

            OpenTibiaId = openTibiaId;

            Count = count;
        }

        public Tile Tile { get; set; }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public override void Execute(Context context)
        {
            Item item = context.Server.ItemFactory.Create(OpenTibiaId);

            if (item is StackableItem stackableItem)
            {
                stackableItem.Count = Count;
            }
            else if (item is FluidItem fluidItem)
            {
                fluidItem.FluidType = (FluidType)Count;
            }

            if (item != null)
            {
                Command command = context.TransformCommand(new TileAddItemCommand(Tile, item));

                command.Completed += (s, e) =>
                {
                    base.OnCompleted(e.Context);
                };

                command.Execute(context);
            }
        }
    }
}