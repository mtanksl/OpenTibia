using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemScriptingHandler : CommandHandler<PlayerUseItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            PlayerUseItemPlugin plugin = Context.Server.Plugins.GetPlayerUseItemPlugin(command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                return plugin.OnUseItem(command.Player, command.Item).Then( (result) =>
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