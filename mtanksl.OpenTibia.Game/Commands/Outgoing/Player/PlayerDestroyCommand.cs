using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Commands
{
    public class PlayerDestroyCommand : Command
    {
        public PlayerDestroyCommand(Player player)
        {
            Player = player;
        }

        public Player Player { get; set; }

        public override Promise Execute()
        {
            if (Context.Server.PlayerFactory.Detach(Player) )
            {
                foreach (var child in Player.Inventory.GetItems() )
                {
                    Detach(Context, child);
                }

                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.PlayerFactory.ClearComponentsAndEventHandlers(Player);

                    foreach (var child in Player.Inventory.GetItems() )
                    {
                        ClearComponentsAndEventHandlers(Context, child);
                    }

                    return Context.AddCommand(new PlayerLogoutCommand(Player) ).Then( () =>
                    {
                        return Context.AddCommand(new TileRemoveCreatureCommand(Player.Tile, Player) );
                    } );
                } );
            }

            return Promise.Completed;
        }

        private static bool Detach(Context context, Item item)
        {
            if (context.Server.ItemFactory.Detach(item) )
            {
                if (item is Container container)
                {
                    foreach (var child in container.GetItems() )
                    {
                        Detach(context, child);
                    }
                }

                return true;
            }

            return false;
        }

        private static void ClearComponentsAndEventHandlers(Context context, Item item)
        {
            context.Server.ItemFactory.ClearComponentsAndEventHandlers(item);

            if (item is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    ClearComponentsAndEventHandlers(context, child);
		        }
	        }
        }
    }
}