using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class MoveItemScriptingHandler : CommandHandler<PlayerMoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, PlayerMoveItemCommand command)
        {
            PlayerMoveItemPlugin plugin = Context.Server.Plugins.GetPlayerMoveItemPlugin(command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                return plugin.OnMoveItem(command.Player, command.Item, command.ToContainer, command.ToIndex, command.Count).Then( (result) =>
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