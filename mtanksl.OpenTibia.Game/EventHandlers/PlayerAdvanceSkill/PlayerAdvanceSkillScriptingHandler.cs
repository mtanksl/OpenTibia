using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerAdvanceSkillScriptingHandler : EventHandler<PlayerAdvanceSkillEventArgs>
    {
        public override Promise Handle(PlayerAdvanceSkillEventArgs e)
        {
            List<Promise> promises = new List<Promise>();

            foreach (var plugin in Context.Server.Plugins.GetPlayerAdvanceSkillPlugins() )
            {
                promises.Add(plugin.OnAdvanceSkill(e.Player, e.Skill, e.FromLevel, e.ToLevel) );
            }

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}