using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
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

                DbPlayer dbPlayer = Context.Database.PlayerRepository.GetPlayerById(Player.DatabasePlayerId);

                dbPlayer.PlayerStorages.Clear();

                foreach (var pair in Player.Client.Storages.GetIndexed() )
                {
                    dbPlayer.PlayerStorages.Add(new DbPlayerStorage()
                    {
                        PlayerId = dbPlayer.Id,

                        Key = pair.Key,

                        Value = pair.Value
                    } );
                }

                dbPlayer.PlayerVips.Clear();

                foreach (var pair in Player.Client.Vips.GetIndexed() )
                {
                    dbPlayer.PlayerVips.Add(new DbPlayerVip()
                    {
                        PlayerId = dbPlayer.Id,

                        VipId = pair.Key
                    } );
                }

                Context.Database.Commit();

                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.PlayerFactory.Destroy(Player);

                    foreach (var child in Player.Inventory.GetItems() )
                    {
                        Destroy(child);
                    }

                    return Context.AddCommand(new TileRemoveCreatureCommand(Player.Tile, Player) ).Then( () =>
                    {
                        if (Player.Health == 0)
                        {
                            Context.AddPacket(Player.Client.Connection, new OpenYouAreDeathDialogOutgoingPacket() );
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
        
        private bool Detach(Item item)
        {
            if (Context.Server.ItemFactory.Detach(item) )
            {
                if (item is Container container)
                {
                    foreach (var child in container.GetItems() )
                    {
                        Detach(child);
                    }
                }

                return true;
            }

            return false;
        }

        private void Destroy(Item item)
        {
            Context.Server.ItemFactory.Destroy(item);

            if (item is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    Destroy(child);
		        }
	        }
        }
    }
}