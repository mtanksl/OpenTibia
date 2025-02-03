using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Game.Plugins;
using System;

namespace OpenTibia.Game.CommandHandlers
{
    public class InventoryRemovingItemScriptingHandler : CommandHandler<InventoryRemoveItemCommand>
    {
        public override Promise Handle(Func<Promise> next, InventoryRemoveItemCommand command)
        {
            InventoryDeEquipPlugin plugin = Context.Server.Plugins.GetInventoryDeEquipPlugin(command.Item.Metadata.OpenTibiaId);

            if (plugin != null)
            {
                return plugin.OnDeEquipping(command.Inventory, command.Item, (byte)command.Inventory.GetIndex(command.Item) ).Then( (result) =>
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