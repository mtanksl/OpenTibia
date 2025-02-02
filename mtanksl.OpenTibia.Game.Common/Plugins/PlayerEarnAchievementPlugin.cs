using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;

namespace OpenTibia.Game.Plugins
{
    public abstract class PlayerEarnAchievementPlugin : Plugin
    {
        public abstract Promise OnEarnAchievement(Player player, string achievementName);
    }
}