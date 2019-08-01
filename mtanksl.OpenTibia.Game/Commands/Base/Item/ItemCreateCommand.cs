using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class ItemCreateCommand : Command
    {
        public ItemCreateCommand(ushort openTibiaId, Position position)
        {
            OpenTibiaId = openTibiaId;

            Position = position;
        }

        public ushort OpenTibiaId { get; set; }

        public Position Position { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Item item = server.ItemFactory.Create(OpenTibiaId);

            if (item != null)
            {
                Tile tile = server.Map.GetTile(Position);

                if (tile != null)
                {
                    //Act

                    new TileAddItemCommand(tile, item).Execute(server, context);

                    //Notify

                    base.Execute(server, context);
                }
            }
        }
    }
}
