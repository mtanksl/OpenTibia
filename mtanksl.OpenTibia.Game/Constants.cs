using OpenTibia.Common.Objects;

namespace OpenTibia.Game
{
    public static class Constants
    {
        public static readonly string OnlyProtocol86Allowed = "Only protocol 8.6 allowed.";

        public static readonly string AccountNameOrPasswordIsNotCorrect = "Account name or password is not correct.";

        public static readonly string ThereIsNoWay = "There is no way.";

        public static readonly string SorryNotPossible = "Sorry, not possible.";

        public static readonly string YouCanNotMoveThisObject = "You cannot move this object.";

        public static readonly string YouCanNotTakeThisObject = "You cannot take this object.";

        public static readonly string YouCanNotThrowThere = "You cannot throw there.";

        public static readonly string ThisIsImpossible = "This is impossible.";

        public static string PlayerWalkSchedulerEvent(Player player)
        {
            return "Player_Walk_" + player.Id;
        }
    }
}