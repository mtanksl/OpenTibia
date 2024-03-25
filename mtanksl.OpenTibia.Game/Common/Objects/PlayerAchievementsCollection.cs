using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerAchievementsCollection : IPlayerAchievementsCollection
    {
        private HashSet<string> achievements = new HashSet<string>();

        public bool HasAchievement(string name)
        {
            return achievements.Contains(name);
        }

        public void SetAchievement(string name)
        {
            achievements.Add(name);
        }

        public void RemoveAchievement(string name)
        {
            achievements.Remove(name);
        }

        public IEnumerable<string> GetAchievements()
        {
            return achievements;
        }
    }    
}