using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveCreatureScriptingHandler : CommandHandler<PlayerMoveCreatureCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveCreatureCommand command)
        {
            PlayerMoveCreaturePlugin plugin = Context.Server.Plugins.GetPlayerMoveCreaturePlugin(command.Creature.Name);

            if (plugin != null)
            {
                return plugin.OnMoveCreature(command.Player, command.Creature, command.ToTile).Then( (result) =>
                {
                    if (result)
                    {
                        return Promise.Completed;
                    }

                    return next();
                } );
            }

            return next();
        }
    }
}