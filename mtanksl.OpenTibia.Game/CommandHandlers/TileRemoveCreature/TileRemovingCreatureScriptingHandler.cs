using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileRemovingCreatureScriptingHandler : CommandHandler<TileRemoveCreatureCommand>
    {
        public override Promise Handle(Func<Promise> next, TileRemoveCreatureCommand command)
        {
            if (command.FromTile.Ground != null)
            {
                CreatureStepOutPlugin plugin = Context.Server.Plugins.GetCreatureStepOutPlugin(command.FromTile.Ground);

                if (plugin != null)
                {
                    return plugin.OnSteppingOut(command.Creature, command.FromTile).Then( (result) =>
                    {
                        if (result)
                        {
                            return Promise.Completed;
                        }

                        return next();
                    } );
                }
            }

            return next();
        }
    }
}