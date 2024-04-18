using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class PlayerSayScriptingHandler : CommandHandler<PlayerSayCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerSayCommand command)
        {
            if (command.Message.StartsWith("/") )
            {
                int index = command.Message.IndexOf(" ");

                string message;

                if (index == -1)
                {
                    message = command.Message;
                }
                else
                {
                    message = command.Message.Substring(0, index);
                }

                PlayerSayPlugin plugin = Context.Server.Plugins.GetPlayerSayPlugin(message);

                if (plugin != null)
                {
                    return plugin.OnSay(command.Player, command.Message).Then( (result) =>
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