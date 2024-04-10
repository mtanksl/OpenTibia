using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerLogoutScriptingHandler : EventHandler<PlayerLogoutEventArgs>
    {
        public override Promise Handle(PlayerLogoutEventArgs e)
        {
            List<Promise> promises = new List<Promise>();

            foreach (var plugin in Context.Server.Plugins.GetPlayerLogoutPlugins() )
            {
                promises.Add(plugin.OnLogout(e.Player, e.Tile) );
            }

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}