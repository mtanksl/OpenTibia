using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreatureDeathScriptingHandler : EventHandler<CreatureDeathEventArgs>
    {
        public override Promise Handle(CreatureDeathEventArgs e)
        {
            List<Promise> promises = new List<Promise>();

            foreach (var plugin in Context.Server.Plugins.GetCreatureDeathPlugins() )
            {
                promises.Add(plugin.OnDeath(e.Creature) );
            }

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}