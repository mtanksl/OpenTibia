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
                        Tile toTile = server.Map.GetTile( creature.Tile.Position.Offset(direction) );

                        if (toTile == null || toTile.Position.X > spawnPosition.X + radius || toTile.Position.X < spawnPosition.X - radius || toTile.Position.Y > spawnPosition.Y + radius || toTile.Position.Y < spawnPosition.Y - radius || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
                        {
                   
                        }
                        else
                        {
                            new CreatureMoveCommand(creature, toTile).Execute(server, context);

                            break;
                        }
                    }

                    break;
                }
            }
        }
    }
}