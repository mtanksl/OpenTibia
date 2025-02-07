using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreatureKillScriptingHandler : EventHandler<CreatureKillEventArgs>
    {
        public override Promise Handle(CreatureKillEventArgs e)
        {
            List<Promise> promises = new List<Promise>();

            foreach (var plugin in Context.Server.Plugins.GetCreatureKillPlugins() )
            {
                promises.Add(plugin.OnKill(e.Creature, e.Target) );
            }

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}