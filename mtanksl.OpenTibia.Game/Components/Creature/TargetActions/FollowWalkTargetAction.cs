using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public class FollowWalkTargetAction : TargetAction
    {
        private DateTime walkCooldown;

        public override Promise Update(Creature attacker, Creature target)
        {
            if (DateTime.UtcNow > walkCooldown)
            {
                Tile toTile = GetNext(Context.Current.Server, attacker, target);

                if (toTile != null)
                {
                    walkCooldown = DateTime.UtcNow.AddMilliseconds(1000 * toTile.Ground.Metadata.Speed / attacker.Speed);

                    return Context.Current.AddCommand(new CreatureWalkCommand(attacker, toTile) );
                }

                walkCooldown = DateTime.UtcNow.AddMilliseconds(500);
            }

            return Promise.Completed;
        }

        private Tile GetNext(Server server, Creature attacker, Creature target)
        {
            MoveDirection[] moveDirections = server.Pathfinding.GetMoveDirections(attacker.Tile.Position, target.Tile.Position);

            if (moveDirections.Length != 0)
            {
                return server.Map.GetTile(attacker.Tile.Position.Offset(moveDirections[0] ) );
            }

            return null;
        }
    }
}