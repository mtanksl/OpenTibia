using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Components
{
    public class FollowWalkStrategy : IWalkStrategy
    {
        public static readonly FollowWalkStrategy Instance = new FollowWalkStrategy();

        public bool CanWalk(Creature attacker, Creature target, out Tile tile)
        {
            MoveDirection[] moveDirections = Context.Current.Server.Pathfinding.GetMoveDirections(attacker.Tile.Position, target.Tile.Position, false);

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