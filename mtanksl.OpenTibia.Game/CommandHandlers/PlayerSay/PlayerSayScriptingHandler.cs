using mtanksl.OpenTibia.Game.Plugins;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerSayScriptingHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            PlayerSayPlugin plugin = Context.Server.Plugins.GetPlayerSayPlugin(command.Message);

            if (plugin != null)
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