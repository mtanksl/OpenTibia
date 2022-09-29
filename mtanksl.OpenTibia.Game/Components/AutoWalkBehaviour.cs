using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class AutoWalkBehaviour : TimeBehaviour
    {
        private Creature creature;

        public override void Start(Server server)
        {
            creature = (Creature)GameObject;
        }

        public override void Stop(Server server)
        {
            creature = null;
        }

        public override void Update(Context context)
        {
            foreach (var observer in context.Server.GameObjects.GetPlayers() )
            {
                if (creature.Tile.Position.IsInClientRange(observer.Tile.Position) )
                {
                    foreach (var direction in new[] { Direction.East, Direction.North, Direction.West, Direction.South }.Shuffle() )
                    {
                        Tile toTile = context.Server.Map.GetTile(creature.Tile.Position.Offset(direction) );

                        if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || toTile.GetCreatures().Any(c => c.Block) )
                        {

                        }
                        else
                        {
                            context.AddCommand(new CreatureMoveCommand(creature, toTile) );

                            break;
                        }
                    }

                    break;
                }
            }
        }
    }
}