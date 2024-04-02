using OpenTibia.Game.CommandHandlers;

namespace OpenTibia.Game.Scripts
{
    public class InventoryRemoveItemScripts : Script
    {
        public override void Start()
        {
            Context.Server.EventHandlers.Subscribe(new InventoryRemoveItemScriptingHandler() );

            Context.Server.EventHandlers.Subscribe(new RingDeEquipHandler() );

            Context.Server.EventHandlers.Subscribe(new HelmetOfTheDeepDeEquipHandler() );

            Context.Server.CommandHandlers.AddCommandHandler(new InventoryRemoveItemNpcTradingUpdateStatsHandler() );
        }

        public override void Stop()
        {
            
        }
    }
}