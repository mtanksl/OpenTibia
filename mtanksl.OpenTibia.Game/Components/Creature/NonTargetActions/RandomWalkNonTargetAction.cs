using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class RandomWalkNonTargetAction : NonTargetAction
    {
        private int radius;

        public RandomWalkNonTargetAction(int radius)
        {
            this.radius = radius;
        }

        private Tile spawn;

        private DateTime walkCooldown;

        public override Promise Update(Creature creature)
        {
            if (DateTime.UtcNow > walkCooldown)
            {
                if (spawn == null)
                {
                    spawn = creature.Tile;
                }

                Tile toTile = GetNext(Context.Current.Server, spawn, creature);

                if (toTile != null)
                {
                    walkCooldown = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / creature.Speed);

                    return Context.Current.AddCommand(new CreatureWalkCommand(creature, toTile) );
                }

                walkCooldown = DateTime.UtcNow.AddMilliseconds(500);
            }

            return Promise.Completed;
        }

        private Tile GetNext(Server server, Tile spawn, Creature attacker)
        {
            foreach (var direction in server.Randomization.Shuffle(new[] { Direction.North, Direction.East, Direction.South, Direction.West } ) )
            {
                Tile toTile = server.Map.GetTile(attacker.Tile.Position.Offset(direction) );

                if (toTile == null || 

                    toTile.GetItems().Any(i => i.Metadata.Flags.Is(ItemMetadataFlags.NotWalkable) || i.Metadata.Flags.Is(ItemMetadataFlags.BlockPathFinding) ) || 

                    toTile.GetCreatures().Any(c => c.Block) || 

                    Math.Abs(toTile.Position.X - spawn.Position.X) > radius || 

                    Math.Abs(toTile.Position.Y - spawn.Position.Y) > radius)
                {

                }
                else
                {
                    return toTile;
                }
            }

            return null;
        }
    }
}