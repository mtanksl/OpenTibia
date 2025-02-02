using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class PlayerEarnAchievementEventArgs : GameEventArgs
    {
        public PlayerEarnAchievementEventArgs(Player player, string achievementName)
        {
            Player = player;

            AchievementName = achievementName;
        }

        public Player Player { get; }

        public string AchievementName { get; }
    }
}