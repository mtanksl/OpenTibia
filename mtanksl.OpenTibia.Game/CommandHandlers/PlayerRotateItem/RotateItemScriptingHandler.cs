using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class RotateItemScriptingHandler : CommandHandler<PlayerRotateItemCommand>
    {
        private Dictionary<ushort, PlayerRotateItemPlugin> plugins;

        public RotateItemScriptingHandler(Dictionary<ushort, PlayerRotateItemPlugin> plugins)
        {
            this.plugins = plugins;
        }

        public override Promise Handle(Func<Promise> next, PlayerRotateItemCommand command)
        {
            PlayerRotateItemPlugin plugin;

            if (plugins.TryGetValue(command.Item.Metadata.OpenTibiaId, out plugin))
            {
                return plugin.OnRotateItem(command.Player, command.Item).Then(result =>
                {
                    if (result)
                    {
                        return Promise.Completed;
                    }

                    return next();
                });
            }

            return next();
        }
    }
}