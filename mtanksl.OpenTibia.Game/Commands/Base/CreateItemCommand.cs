using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class CreateItemCommand : Command
    {
        public CreateItemCommand(ushort openTibiaId, Position position)
        {
            OpenTibiaId = openTibiaId;

            Position = position;
        }

        public ushort OpenTibiaId { get; set; }

        public Position Position { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Item item = server.ItemFactory.Create(OpenTibiaId);

            if (item != null)
            {
                Tile tile = server.Map.GetTile(Position);

                if (tile != null)
                {
                    SequenceCommand command = new SequenceCommand(

                        new TileAddItemCommand(tile, item), 

                        new MagicEffectCommand(Position, MagicEffectType.BlueShimmer) );

                    command.Completed += (s, e) =>
                    {
                        //Act

                        base.Execute(e.Server, e.Context);
                    };

                    command.Execute(server, context);
                }
            }
        }
    }
}