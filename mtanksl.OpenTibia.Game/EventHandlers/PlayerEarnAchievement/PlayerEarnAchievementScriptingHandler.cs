using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerEarnAchievementScriptingHandler : EventHandler<PlayerEarnAchievementEventArgs>
    {
        public override Promise Handle(PlayerEarnAchievementEventArgs e)
        {
            List<Promise> promises = new List<Promise>();

            foreach (var plugin in Context.Server.Plugins.GetPlayerEarnAchievementPlugins() )
            {
                promises.Add(plugin.OnEarnAchievement(e.Player, e.AchievementName) );
            }

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}