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

        public static readonly string YouMayNotAttackThisPlayer = "You may not attack this player.";

        public static readonly string TargetLost = "Target lost.";

        public static string PlayerWalkSchedulerEvent(Player player)
        {
            return "Player_Walk_" + player.Id;
        }

        public static string PlayerAttackSchedulerEvent(Player player)
        {
            return "Player_Attack_" + player.Id;
        }

        public static readonly string GlobalLightSchedulerEvent = "Global_Light";

        public static readonly string GlobalCreaturesSchedulerEvent = "Global_Creatures";
    }
}