using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.IO;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class WalkBehaviour : IBehaviour
    {
        public GameObject GameObject { get; set; }

        public void Start(Server server)
        {
            
        }

        public void Update(Server server, CommandContext context)
        {
            Creature creature = (Creature)GameObject;

            foreach (var direction in new[] { Direction.East, Direction.North, Direction.West, Direction.South }.Shuffle() )
            {
                Tile toTile = server.Map.GetTile( creature.Tile.Position.Offset(direction) );

                if (toTile == null || toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) ) || toTile.GetCreatures().Any(c => c.Block) )
                {
                   
                }
                else
                {
                    new CreatureMoveCommand(creature, toTile).Execute(server, context);

                    break;
                }
            }
        }
    }
}