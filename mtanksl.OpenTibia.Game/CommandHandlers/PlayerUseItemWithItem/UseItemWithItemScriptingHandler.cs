using OpenTibia.Game.Plugins;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithItemScriptingHandler : CommandHandler<PlayerUseItemWithItemCommand>
    {
        private bool allowFarUse;

        public UseItemWithItemScriptingHandler(bool allowFarUse)
        {
            this.allowFarUse = allowFarUse;
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithItemCommand command)
        {
            PlayerUseItemWithItemPlugin plugin = Context.Server.Plugins.GetPlayerUseItemWithItemPlugin(allowFarUse, command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                return plugin.OnUseItemWithItem(command.Player, command.Item, command.ToItem).Then(result =>
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