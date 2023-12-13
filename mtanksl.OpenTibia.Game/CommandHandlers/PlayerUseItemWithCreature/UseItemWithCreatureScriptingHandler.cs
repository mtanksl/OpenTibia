using OpenTibia.Game.Commands;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class UseItemWithCreatureScriptingHandler : CommandHandler<PlayerUseItemWithCreatureCommand>
    {
        private bool allowFarUse;

        public UseItemWithCreatureScriptingHandler(bool allowFarUse)
        {
            this.allowFarUse = allowFarUse;
        }

        public override Promise Handle(Func<Promise> next, PlayerUseItemWithCreatureCommand command)
        {
            PlayerUseItemWithCreaturePlugin plugin = Context.Server.Plugins.GetPlayerUseItemWithCreaturePlugin(allowFarUse, command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                return plugin.OnUseItemWithCreature(command.Player, command.Item, command.ToCreature).Then(result =>
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