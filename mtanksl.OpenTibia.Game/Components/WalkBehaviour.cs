using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class WalkBehaviour : Behaviour
    {
        private int radius;

        public WalkBehaviour(int radius)
        {
            this.radius = radius;
        }

        private Position spawnPosition;

        public override void Start(Server server)
        {
            Creature creature = (Creature)GameObject;

            spawnPosition = creature.Tile.Position;
        }

        public override void Update(Server server, Context context)
        {
            Creature creature = (Creature)GameObject;

            foreach (var observer in server.Map.GetPlayers() )
            {
                if (creature != observer && creature.Tile.Position.IsInPlayerRange(observer.Tile.Position) )
                {
                    foreach (var direction in new[] { Direction.East, Direction.North, Direction.West, Direction.South }.Shuffle() )
                    {
                        Position toPosition = creature.Tile.Position.Offset(direction);

                        if (toPosition.X > spawnPosition.X + radius || toPosition.X < spawnPosition.X - radius || toPosition.Y > spawnPosition.Y + radius || toPosition.Y < spawnPosition.Y - radius)
                        {

                        }
                        else
                        {
                            Tile toTile = server.Map.GetTile(toPosition);

                            if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || toTile.GetCreatures().Any(c => c.Block) )
                            {
                   
                            }
                            else
                            {
                                new CreatureMoveCommand(creature, toTile).Execute(server, context);

                                break;
                            }
                        }
                    }

                    break;
                }
            }
        }
    }
}