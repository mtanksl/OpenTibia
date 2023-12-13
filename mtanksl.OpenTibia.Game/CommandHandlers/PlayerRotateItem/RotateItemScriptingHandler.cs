using OpenTibia.Game.Commands;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class RotateItemScriptingHandler : CommandHandler<PlayerRotateItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerRotateItemCommand command)
        {
            PlayerRotateItemPlugin plugin = Context.Server.Plugins.GetPlayerRotateItemPlugin(command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                return plugin.OnRotateItem(command.Player, command.Item).Then(result =>
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