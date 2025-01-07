using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class InventoryRemoveItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe<InventoryRemoveItemEventArgs>(new InventoryRemoveItemScriptingHandler() );

            Context.Server.EventHandlers.Subscribe<InventoryRemoveItemEventArgs>(new RingDeEquipHandler() );

            Context.Server.EventHandlers.Subscribe<InventoryRemoveItemEventArgs>(new HelmetOfTheDeepDeEquipHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<InventoryRemoveItemCommand>(new InventoryRemoveItemNpcTradingUpdateStatsHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}