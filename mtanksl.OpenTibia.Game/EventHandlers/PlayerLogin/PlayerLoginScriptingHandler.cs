using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerLoginScriptingHandler : EventHandler<PlayerLoginEventArgs>
    {
        public override Promise Handle(PlayerLoginEventArgs e)
        {
            List<Promise> promises = new List<Promise>();

            foreach (var plugin in Context.Server.Plugins.GetPlayerLoginPlugins() )
            {
                promises.Add(plugin.OnLogin(e.Player, e.Tile) );
            }

            return Promise.WhenAll(promises.ToArray() );
        }
    }
}