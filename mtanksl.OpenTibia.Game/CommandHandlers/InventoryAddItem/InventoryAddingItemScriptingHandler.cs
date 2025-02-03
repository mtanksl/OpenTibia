using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryAddingItemScriptingHandler : CommandHandler<InventoryAddItemCommand>
    {
        public override Promise Handle(Func<Promise> next, InventoryAddItemCommand command)
        {
            InventoryEquipPlugin plugin = Context.Server.Plugins.GetInventoryEquipPlugin(command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                return plugin.OnEquipping(command.Inventory, command.Item, command.Slot).Then( (result) =>
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