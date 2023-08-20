using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Components
{
    public class FollowWalkStrategy : IWalkStrategy
    {
        public bool CanWalk(Creature attacker, Creature target, out Tile tile)
        {
            MoveDirection[] moveDirections = Context.Current.Server.Pathfinding.GetMoveDirections(attacker.Tile.Position, target.Tile.Position);

            if (moveDirections.Length != 0)
            {
                tile = Context.Current.Server.Map.GetTile(attacker.Tile.Position.Offset(moveDirections[0] ) );

                return true;
            }

            tile = null;

            return false;
        }
    }
}