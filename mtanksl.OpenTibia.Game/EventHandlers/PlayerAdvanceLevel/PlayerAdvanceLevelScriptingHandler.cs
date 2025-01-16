using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerAdvanceLevelScriptingHandler : EventHandler<PlayerAdvanceLevelEventArgs>
    {
        public override Promise Handle(PlayerAdvanceLevelEventArgs e)
        {
            List<Promise> promises = new List<Promise>();

            foreach (var plugin in Context.Server.Plugins.GetPlayerAdvanceLevelPlugins() )
            {
                promises.Add(plugin.OnAdvanceLevel(e.Player, e.FromLevel, e.ToLevel) );
            }

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}