using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;

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

        public override void Execute(Context context)
        {
            Creature creature = context.Server.NpcFactory.Create(Name);

            if (creature == null)
            {
                creature = context.Server.MonsterFactory.Create(Name);
            }

            if (creature != null)
            {
                Tile tile = context.Server.Map.GetTile(Position);

                if (tile != null)
                {
                    new TileAddCreatureCommand(tile, creature).Execute(context);

                    foreach (var component in creature.GetComponents<Behaviour>() )
                    {
                        component.Start(context.Server);
                    }

                    base.OnCompleted(context);
                }
            }
        }
    }
}