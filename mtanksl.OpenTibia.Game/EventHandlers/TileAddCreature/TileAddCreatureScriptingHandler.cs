using OpenTibia.Game.Common;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileAddCreatureScriptingHandler : EventHandler<TileAddCreatureEventArgs>
    {
        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.ToTile.Ground != null)
            {
                CreatureStepInPlugin plugin = Context.Server.Plugins.GetCreatureStepInPlugin(e.ToTile.Ground.Metadata.OpenTibiaId);

                if (plugin != null)
                {
                    return plugin.OnStepIn(e.Creature, e.FromTile, e.ToTile);
                }
            }

            return Promise.Completed;
        }
    }
}