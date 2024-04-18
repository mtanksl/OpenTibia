using OpenTibia.Common.Structures;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface IPathfinding
    {
        bool CanThrow(Position fromPosition, Position toPosition);

        MoveDirection[] GetMoveDirections(Position fromPosition, Position toPosition, bool allowProtectionZone);
    }
}