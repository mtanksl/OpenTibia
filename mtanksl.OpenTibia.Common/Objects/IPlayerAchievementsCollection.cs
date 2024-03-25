using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IPlayerAchievementsCollection
    {
        bool HasAchievement(string name);

        void SetAchievement(string name);

        void RemoveAchievement(string name);

        IEnumerable<string> GetAchievements();
    }    
}