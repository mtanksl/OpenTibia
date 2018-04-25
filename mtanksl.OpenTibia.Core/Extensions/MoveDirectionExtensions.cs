namespace OpenTibia
{
    public static class MoveDirectionExtensions
    {
        public static bool IsDiagonal(this MoveDirection moveDirection)
        {
            return moveDirection == MoveDirection.NorthEast || moveDirection == MoveDirection.NorthWest || moveDirection == MoveDirection.SouthEast || moveDirection == MoveDirection.SouthWest;
        }
    }
}