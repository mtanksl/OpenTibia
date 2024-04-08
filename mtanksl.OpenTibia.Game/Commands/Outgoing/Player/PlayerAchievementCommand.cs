using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class PlayerAchievementCommand : Command
    {
        public PlayerAchievementCommand(Player player, int incrementStorageKey, int requiredStorageValue, string achievementName)
        {
            Player = player;

            IncrementStorageKey = incrementStorageKey;

            RequiredStorageValue = requiredStorageValue;

            AchievementName = achievementName;
        }

        public PlayerAchievementCommand(Player player, int incrementStorageKey, int[] requiredStorageKeys, string achievementName)
        {
            Player = player;

            IncrementStorageKey = incrementStorageKey;

            RequiredStorageKeys = requiredStorageKeys;

            AchievementName = achievementName;
        }

        public Player Player { get; set; }

        public int IncrementStorageKey { get; set; }

        public int RequiredStorageValue { get; set; }

        public int[] RequiredStorageKeys { get; set; }

        public string AchievementName { get; set; }

        public override Promise Execute()
        {
            int value;

            if ( !Player.Storages.TryGetValue(IncrementStorageKey, out value) )
            {
                value = 0;
            }

            Player.Storages.SetValue(IncrementStorageKey, ++value);

            if ( !Player.Achievements.HasAchievement(AchievementName) )
            {
                if (RequiredStorageKeys == null)
                {
                    if (value >= RequiredStorageValue)
                    {                    
                        Player.Achievements.SetAchievement(AchievementName);

                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"" + AchievementName + "\".") );
                    }
                }
                else
                {
                    if (RequiredStorageKeys.All(key => Player.Storages.TryGetValue(key, out _) ) )
                    {
                        Player.Achievements.SetAchievement(AchievementName);

                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"" + AchievementName + "\".") );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}
