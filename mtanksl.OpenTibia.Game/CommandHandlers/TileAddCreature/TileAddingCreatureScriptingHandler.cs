using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class TileAddingCreatureScriptingHandler : CommandHandler<TileAddCreatureCommand>
    {
        public override Promise Handle(Func<Promise> next, TileAddCreatureCommand command)
        {
            if (command.ToTile.Ground != null)
            {
                CreatureStepInPlugin plugin = Context.Server.Plugins.GetCreatureStepInPlugin(command.ToTile.Ground.Metadata.OpenTibiaId);

                if (plugin != null)
                {
                    return plugin.OnSteppingIn(command.Creature, command.ToTile).Then( (result) =>
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