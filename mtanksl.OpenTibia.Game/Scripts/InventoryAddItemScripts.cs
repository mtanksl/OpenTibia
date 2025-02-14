using OpenTibia.Game.CommandHandlers;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;

namespace OpenTibia.Game.Scripts
{
    public class InventoryAddItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.CommandHandlers.AddCommandHandler<InventoryAddItemCommand>(new InventoryAddingItemScriptingHandler() );

            Context.Server.CommandHandlers.AddCommandHandler<InventoryAddItemCommand>(new InventoryAddItemNpcTradingUpdateStatsHandler() );


            Context.Server.EventHandlers.Subscribe<InventoryAddItemEventArgs>(new InventoryAddItemScriptingHandler() );

            Context.Server.EventHandlers.Subscribe<InventoryAddItemEventArgs>(new RingEquipHandler() );

            Context.Server.EventHandlers.Subscribe<InventoryAddItemEventArgs>(new FeetEquipHandler() );

            Context.Server.EventHandlers.Subscribe<InventoryAddItemEventArgs>(new HelmetOfTheDeepEquipHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}