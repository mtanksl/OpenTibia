using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class CreateMonsterCommand : Command
    {
        public CreateMonsterCommand(string name, Position position)
        {
            Name = name;

            Position = position;
        }

        public string Name { get; set; }

        public Position Position { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            //Arrange

            Monster monster = server.MonsterFactory.Create(Name);

            if (monster != null)
            {
                Tile tile = server.Map.GetTile(Position);

                if (tile != null)
                {
                    SequenceCommand command = new SequenceCommand(

                        new TileAddCreatureCommand(tile, monster), 

                        new MagicEffectCommand(Position, MagicEffectType.RedShimmer) );

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