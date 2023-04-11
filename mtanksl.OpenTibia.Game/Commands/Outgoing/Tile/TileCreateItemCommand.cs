using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class TileCreateItemCommand : CommandResult<Item>
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

        public override PromiseResult<Item> Execute()
        {
            return Promise.Run<Item>( (resolve, reject) =>
            {
                Item item = context.Server.ItemFactory.Create(OpenTibiaId, Count);

                if (item != null)
                {
                    context.AddCommand(new TileAddItemCommand(Tile, item) );
                }

                resolve(context, item);
            } );
        }
    }
}