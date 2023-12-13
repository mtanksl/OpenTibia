using OpenTibia.Game.Commands;
using OpenTibia.Game.EventHandlers;
using OpenTibia.Game.Events;
using OpenTibia.Game.Plugins;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileRemoveCreatureScriptingHandler : EventHandler<TileRemoveCreatureEventArgs>
    {
        public override Promise Handle(TileRemoveCreatureEventArgs e)
        {
            if (e.Tile.Ground != null)
            {
                CreatureStepOutPlugin plugin = Context.Server.Plugins.GetCreatureStepOutPlugin(e.Tile.Ground.Metadata.OpenTibiaId);

                if (plugin != null)
                {
                    return plugin.OnStepOut(e.Creature, e.Tile);
                }
            }

            return Promise.Completed;
        }
    }
}