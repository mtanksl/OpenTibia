using OpenTibia.Common.Objects;

namespace OpenTibia.Game
{
    public static class Constants
    {
        public static readonly string OnlyProtocol86Allowed = "Only protocol 8.6 allowed.";

        public static readonly string AccountNameOrPasswordIsNotCorrect = "Account name or password is not correct.";

        public static readonly string SorryNotPossible = "Sorry, not possible.";

        public static string PlayerWalkSchedulerEvent(Player player)
        {
            return "Player_Walk_" + player.Id;
        }

        public static string PlayerPingSchedulerEvent(Player player)
        {
            return "Player_Ping_" + player.Id;
        }
    }
}