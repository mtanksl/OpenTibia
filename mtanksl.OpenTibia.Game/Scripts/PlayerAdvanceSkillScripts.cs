using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class PlayerAdvanceSkillScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe<PlayerAdvanceSkillEventArgs>(new PlayerAdvanceSkillScriptingHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}