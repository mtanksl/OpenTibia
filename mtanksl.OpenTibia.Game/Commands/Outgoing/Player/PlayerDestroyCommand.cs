using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
using OpenTibia.Network.Packets.Outgoing;
using System.Linq;

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

                #region TODO: Review

                DbPlayer databasePlayer = SavePlayer(Context, Player, Player.Tile);

                SaveInventory(Context, Player, databasePlayer);

                SaveLocker(Context, Player, databasePlayer);

                SaveVip(Context, Player, databasePlayer);

                Context.DatabaseContext.Commit();

                #endregion

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

        private static DbPlayer SavePlayer(Context context, Player player, Tile fromTile)
        {
            DbPlayer databasePlayer = context.DatabaseContext.PlayerRepository.GetPlayerById(player.DatabasePlayerId);

            databasePlayer.Direction = (int)player.Direction;

            databasePlayer.BaseOutfitItemId = player.BaseOutfit.ItemId;

            databasePlayer.BaseOutfitId = player.BaseOutfit.Id;

            databasePlayer.BaseOutfitHead = player.BaseOutfit.Head;

            databasePlayer.BaseOutfitBody = player.BaseOutfit.Body;

            databasePlayer.BaseOutfitLegs = player.BaseOutfit.Legs;

            databasePlayer.BaseOutfitFeet = player.BaseOutfit.Feet;

            databasePlayer.BaseOutfitAddon = (int)player.BaseOutfit.Addon;

            databasePlayer.OutfitItemId = player.Outfit.ItemId;

            databasePlayer.OutfitId = player.Outfit.Id;

            databasePlayer.OutfitHead = player.Outfit.Head;

            databasePlayer.OutfitBody = player.Outfit.Body;

            databasePlayer.OutfitLegs = player.Outfit.Legs;

            databasePlayer.OutfitFeet = player.Outfit.Feet;

            databasePlayer.OutfitAddon = (int)player.Outfit.Addon;

            databasePlayer.Invisible = player.Invisible;

            databasePlayer.CoordinateX = fromTile.Position.X;

            databasePlayer.CoordinateY = fromTile.Position.Y;

            databasePlayer.CoordinateZ = fromTile.Position.Z;

            context.DatabaseContext.PlayerRepository.UpdatePlayer(databasePlayer);

            return databasePlayer;
        }

        private static void SaveInventory(Context context, Player player, DbPlayer databasePlayer)
        {
            void AddItems(ref int sequence, int index, Item item)
            {
                DbPlayerItem playerItem = new DbPlayerItem()
                {
                    PlayerId = player.DatabasePlayerId,

                    SequenceId = sequence++,

                    ParentId = index,

                    OpenTibiaId = item.Metadata.OpenTibiaId,

                    Count = item is StackableItem stackableItem ? stackableItem.Count :

                            item is FluidItem fluidItem ? (int)fluidItem.FluidType :

                            item is SplashItem splashItem ? (int)splashItem.FluidType : 1,
                };

                context.DatabaseContext.PlayerRepository.AddPlayerItem(playerItem);

                if (item is Container container)
                {
                    foreach (var item2 in container.GetItems().Reverse() )
                    {
                        AddItems(ref sequence, playerItem.SequenceId, item2);
                    }
                }
            }

            foreach (var playerItem in databasePlayer.PlayerItems.ToList() )
            {
                context.DatabaseContext.PlayerRepository.RemovePlayerItem(playerItem);
            }

            int sequenceId = 101;

            foreach (var pair in player.Inventory.GetIndexedContents() )
            {
                AddItems(ref sequenceId, pair.Key, (Item)pair.Value);
            }
        }

        private static void SaveLocker(Context context, Player player, DbPlayer databasePlayer)
        {
            void AddItems(ref int sequence, int index, Item item)
            {
                DbPlayerDepotItem playerDepotItem = new DbPlayerDepotItem()
                {
                    PlayerId = player.DatabasePlayerId,

                    SequenceId = sequence++,

                    ParentId = index,

                    OpenTibiaId = item.Metadata.OpenTibiaId,

                    Count = item is StackableItem stackableItem ? stackableItem.Count :

                            item is FluidItem fluidItem ? (int)fluidItem.FluidType :

                            item is SplashItem splashItem ? (int)splashItem.FluidType : 1,
                };

                context.DatabaseContext.PlayerRepository.AddPlayerDepotItem(playerDepotItem);

                if (item is Container container)
                {
                    foreach (var item2 in container.GetItems().Reverse() )
                    {
                        AddItems(ref sequence, playerDepotItem.SequenceId, item2);
                    }
                }
            }

            foreach (var playerDepotItem in databasePlayer.PlayerDepotItems.ToList() )
            {
                context.DatabaseContext.PlayerRepository.RemovePlayerDepotItem(playerDepotItem);
            }

            int sequenceId = 101;

            foreach (var pair in context.Server.Lockers.GetIndexedLockers(player.DatabasePlayerId) )
            {
                AddItems(ref sequenceId, pair.Key, pair.Value);
            }
        }

        private static void SaveVip(Context context, Player player, DbPlayer databasePlayer)
        {
            foreach (var playerVip in databasePlayer.PlayerVips.ToList() )
            {
                context.DatabaseContext.PlayerRepository.RemovePlayerVip(playerVip);
            }

            int sequenceId = 1;

            foreach (var vip in player.Client.VipCollection.GetVips() )
            {
                var playerVip = new DbPlayerVip()
                {
                    PlayerId = player.DatabasePlayerId,

                    SequenceId = sequenceId++,

                    VipId = context.DatabaseContext.PlayerRepository.GetPlayerByName(vip.Name).Id
                };

                context.DatabaseContext.PlayerRepository.AddPlayerVip(playerVip);
            }
        }
    }
}