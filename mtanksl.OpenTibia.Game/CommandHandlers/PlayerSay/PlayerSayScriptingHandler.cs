using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerSayScriptingHandler : CommandHandler<PlayerSayCommand>
    {
        private Dictionary<string, PlayerSayPlugin> plugins;

        public PlayerSayScriptingHandler(Dictionary<string, PlayerSayPlugin> plugins)
        {
            this.plugins = plugins;
        }

        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            PlayerSayPlugin plugin;

            if (plugins.TryGetValue(command.Message, out plugin) )
            {
                return plugin.OnSay(command.Player, command.Message).Then(result =>
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