using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

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
            Item item = Context.Server.ItemFactory.Create(OpenTibiaId, Count);

            if (item != null)
            {
                Context.Server.ItemFactory.Attach(item);

                return Context.AddCommand(new TileAddItemCommand(Tile, item) ).Then( () =>
                {
                    return Promise.FromResult(item);
                } );
            }

            return Promise.FromResult(item);
        }
    }
}