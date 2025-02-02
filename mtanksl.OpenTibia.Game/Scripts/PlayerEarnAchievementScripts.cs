using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class PlayerEarnAchievementScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe<PlayerEarnAchievementEventArgs>(new PlayerEarnAchievementScriptingHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}