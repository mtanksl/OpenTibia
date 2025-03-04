using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileRemoveCreatureScriptingHandler : EventHandler<TileRemoveCreatureEventArgs>
    {
        public override Promise Handle(TileRemoveCreatureEventArgs e)
        {
            if (e.FromTile.Ground != null)
            {
                CreatureStepOutPlugin plugin = Context.Server.Plugins.GetCreatureStepOutPlugin(e.FromTile.Ground);

                if (plugin != null)
                {
                    return plugin.OnStepOut(e.Creature, e.FromTile, e.ToTile);
                }
            }

            return Promise.Completed;
        }
    }
}