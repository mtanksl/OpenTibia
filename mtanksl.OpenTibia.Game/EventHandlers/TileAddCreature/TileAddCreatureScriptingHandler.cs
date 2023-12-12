using OpenTibia.Game.Plugins;
using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileAddCreatureScriptingHandler : EventHandler<TileAddCreatureEventArgs>
    {
        public override Promise Handle(TileAddCreatureEventArgs e)
        {
            if (e.Tile.Ground != null)
            {
                CreatureStepInPlugin plugin = Context.Server.Plugins.GetCreatureStepInPlugin(e.Tile.Ground.Metadata.OpenTibiaId);

                if (plugin != null)
                {
                    return plugin.OnStepIn(e.Creature, e.Tile);
                }
            }

            return Promise.Completed;
        }
    }
}