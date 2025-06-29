using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class WrapItemScriptingHandler : CommandHandler<PlayerWrapItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerWrapItemCommand command)
        {
            PlayerWrapItemPlugin plugin = Context.Server.Plugins.GetPlayerWrapItemPlugin(command.Item);

            if (plugin != null)
            {
                return plugin.OnWrapItem(command.Player, command.Item).Then( (result) =>
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