using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Components
{
    public class FollowWalkStrategy : IWalkStrategy
    {
        public FollowWalkStrategy()
        {
            
        }

        public Tile GetNext(Server server, Tile spawn, Creature attacker, Creature target)
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