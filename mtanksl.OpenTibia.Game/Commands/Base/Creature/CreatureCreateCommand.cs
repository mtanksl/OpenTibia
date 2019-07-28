using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Commands
{
    public class CreatureCreateCommand : Command
    {
        public CreatureCreateCommand(string name, Position position)
        {
            Name = name;

            Position = position;
        }

        public string Name { get; set; }

        public Position Position { get; set; }

        public override void Execute(Server server, CommandContext context)
        {
            Creature creature = server.MonsterFactory.Create(Name);

            if (creature != null)
            {
                Tile tile = server.Map.GetTile(Position);

                if (tile != null)
                {
                    server.Map.AddCreature(creature);

                    new TileAddCreatureCommand(tile, creature).Execute(server, context);

                    base.Execute(server, context);
                }
            }
        }
    }
}