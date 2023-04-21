using OpenTibia.Common.Objects;
using OpenTibia.Game.Events;
using OpenTibia.Network.Packets.Outgoing;

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
                    Detach(child);
                }

                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.PlayerFactory.Destroy(Player);

                    foreach (var child in Player.Inventory.GetItems() )
                    {
                        Destroy(child);
                    }

                    return Context.AddCommand(new TileRemoveCreatureCommand(Player.Tile, Player) ).Then( () =>
                    {
                        Context.AddEvent(new PlayerLogoutEventArgs(Player) );

                        if (Player.Health == 0)
                        {
                            Context.AddPacket(Player.Client.Connection, new OpenYouAreDeathDialogOutgoingPacket() );

                            Context.AddEvent(new PlayerDeathEventArgs(Player) );
                        }
                        else
                        {
                            Context.Disconnect(Player.Client.Connection);
                        }

                        return Promise.Completed;
                    } );
                } );
            }

            return Promise.Completed;
        }
        
        private void Detach(Item parent)
        {
            Context.Server.ItemFactory.Detach(parent);

            if (parent is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    Detach(child);
		        }
	        }
        }

        private void Destroy(Item parent)
        {
            Context.Server.ItemFactory.Destroy(parent);

            if (parent is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    Destroy(child);
		        }
	        }
        }
    }
}