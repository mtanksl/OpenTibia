using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemScriptingHandler : CommandHandler<PlayerUseItemCommand>
    {
        private Dictionary<ushort, PlayerUseItemPlugin> plugins;

        public UseItemScriptingHandler(Dictionary<ushort, PlayerUseItemPlugin> plugins)
        {
            this.plugins = plugins;
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemCommand command)
        {
            PlayerUseItemPlugin plugin;

            if (plugins.TryGetValue(command.Item.Metadata.OpenTibiaId, out plugin) )
            {
                return plugin.OnUseItem(command.Player, command.Item).Then(result =>
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