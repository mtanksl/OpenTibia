using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class CreatureMoveTileRemovingScriptingHandler : CommandHandler<CreatureMoveCommand>
    {
        public override Promise Handle(Func<Promise> next, CreatureMoveCommand command)
        {
            if (command.Creature.Tile.Ground != null)
            {
                CreatureStepOutPlugin plugin = Context.Server.Plugins.GetCreatureStepOutPlugin(command.Creature.Tile.Ground);

                if (plugin != null)
                {
                    return plugin.OnSteppingOut(command.Creature, command.Creature.Tile).Then( (result) =>
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