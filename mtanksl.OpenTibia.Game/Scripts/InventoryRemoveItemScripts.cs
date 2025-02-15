using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class InventoryRemoveItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<InventoryRemoveItemCommand>(new InventoryRemovingItemScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<InventoryRemoveItemCommand>(new InventoryRemoveItemNpcTradingUpdateStatsHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<InventoryRemoveItemCommand>(new InventoryRemoveItemUpdatePlayerCapacityHandler() );


            Context.Server.EventHandlers.Subscribe<InventoryRemoveItemEventArgs>(new InventoryRemoveItemScriptingHandler() );

            Context.Server.EventHandlers.Subscribe<InventoryRemoveItemEventArgs>(new RingDeEquipHandler() );

            Context.Server.EventHandlers.Subscribe<InventoryRemoveItemEventArgs>(new FeetDeEquipHandler() );

            Context.Server.EventHandlers.Subscribe<InventoryRemoveItemEventArgs>(new HelmetOfTheDeepDeEquipHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}