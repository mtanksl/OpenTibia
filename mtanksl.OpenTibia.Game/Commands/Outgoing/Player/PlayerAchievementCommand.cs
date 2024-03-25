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
            int count;

            if ( !Player.Client.Storages.TryGetValue(IncrementStorageKey, out count) )
            {
                count = 0;
            }

            Player.Client.Storages.SetValue(IncrementStorageKey, ++count);

            if ( !Player.Client.Achievements.HasAchievement(AchievementName) )
            {
                if (RequiredStorageKeys == null)
                {
                    if (count >= RequiredStorageValue)
                    {                    
                        Player.Client.Achievements.SetAchievement(AchievementName);

                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"" + AchievementName + "\".") );
                    }
                }
                else
                {
                    if (RequiredStorageKeys.All(key => Player.Client.Storages.TryGetValue(key, out _) ) )
                    {
                        Player.Client.Achievements.SetAchievement(AchievementName);

                        Context.AddPacket(Player, new ShowWindowTextOutgoingPacket(TextColor.WhiteCenterGameWindowAndServerLog, "Congratulations! You earned the achievement \"" + AchievementName + "\".") );
                    }
                }
            }

            return Promise.Completed;
        }
    }
}
