using OpenTibia.Common.Objects;
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
            Tile tile = Player.Tile;

            return Context.AddCommand(new TileRemoveCreatureCommand(tile, Player) ).Then( () =>
            {
                #region Save player to database

                var databasePlayer = Context.DatabaseContext.PlayerRepository.GetPlayerById(Player.DatabasePlayerId);

                databasePlayer.Direction = (int)Player.Direction;

                databasePlayer.OutfitItemId = Player.Outfit.ItemId;

                databasePlayer.OutfitId = Player.Outfit.Id;

                databasePlayer.OutfitHead = Player.Outfit.Head;

                databasePlayer.OutfitBody = Player.Outfit.Body;

                databasePlayer.OutfitLegs = Player.Outfit.Legs;

                databasePlayer.OutfitFeet = Player.Outfit.Feet;

                databasePlayer.OutfitAddon = (int)Player.Outfit.Addon;

                databasePlayer.CoordinateX = tile.Position.X;

                databasePlayer.CoordinateY = tile.Position.Y;

                databasePlayer.CoordinateZ = tile.Position.Z;

                Context.DatabaseContext.PlayerRepository.UpdatePlayer(databasePlayer);

                #endregion

                #region Save player items to database

                foreach (var playerItem in databasePlayer.PlayerItems.ToList() )
                {
                    Context.DatabaseContext.PlayerRepository.RemovePlayerItem(playerItem);
                }

                int sequenceId = 101;

                foreach (var pair in Player.Inventory.GetIndexedContents() )
                {
                    AddPlayerItems(Context, ref sequenceId, pair.Key, (Item)pair.Value);
                }

                #endregion

                #region Save player depot items to database

                foreach (var playerDepotItem in databasePlayer.PlayerDepotItems.ToList() )
                {
                    Context.DatabaseContext.PlayerRepository.RemovePlayerDepotItem(playerDepotItem);
                }

                sequenceId = 101;

                foreach (var pair in Context.Server.Lockers.GetIndexedLockers(Player.DatabasePlayerId) )
                {
                    AddPlayerDepotItems(Context, ref sequenceId, pair.Key, pair.Value);
                }

                #endregion

                #region Save player vip to database

                foreach (var playerVip in databasePlayer.PlayerVips.ToList() )
                {
                    Context.DatabaseContext.PlayerRepository.RemovePlayerVip(playerVip);
                }

                sequenceId = 1;

                foreach (var vip in Player.Client.VipCollection.GetVips() )
                {
                    var playerVip = new Data.Models.PlayerVip()
                    {
                        PlayerId = Player.DatabasePlayerId,

                        SequenceId = sequenceId++,

                        VipId = Context.DatabaseContext.PlayerRepository.GetPlayerByName(vip.Name).Id
                    };

                    Context.DatabaseContext.PlayerRepository.AddPlayerVip(playerVip);
                }

                #endregion

                Context.DatabaseContext.Commit();

                #region Destroy

                foreach (var item in Player.Inventory.GetItems() )
                {
                    Context.AddCommand(new ItemDestroyCommand(item) );
                }

                foreach (var pair in Context.Server.Lockers.GetIndexedLockers(Player.DatabasePlayerId).ToList() )
                {
                    Context.Server.Lockers.RemoveLocker(Player.DatabasePlayerId, pair.Key);

                    Context.AddCommand(new ItemDestroyCommand(pair.Value) );
                }

                foreach (var pair in Player.Client.ContainerCollection.GetIndexedContainers() )
                {
                    Player.Client.ContainerCollection.CloseContainer(pair.Key);
                }

                foreach (var pair in Player.Client.WindowCollection.GetIndexedWindows() )
                {
                    Player.Client.WindowCollection.CloseWindow(pair.Key);
                }

                /*
                foreach (var channel in context.Server.Channels.GetChannels().ToList() )
                {
                    if (channel.ContainsPlayer(Player) )
                    {
                        channel.RemovePlayer(Player);
                    }

                    if (channel is PrivateChannel privateChannel)
                    {
                        if (privateChannel.Owner == Player)
                        {
                            privateChannel.Owner = null;
                        }

                        if (privateChannel.ContainsInvitation(Player) )
                        {
                            privateChannel.RemoveInvitation(Player);
                        }
                    }
                }
                */

                /*
                foreach (var ruleViolation in context.Server.RuleViolations.GetRuleViolations().ToList() )
                {
                    if (ruleViolation.Reporter == Player)
                    {
                        ruleViolation.Reporter = null;
                    }

                    if (ruleViolation.Assignee == Player)
                    {
                        ruleViolation.Assignee = null;
                    }
                }
                */

                Context.Server.PlayerFactory.Destroy(Player);

                #endregion
            } );
        }

        private void AddPlayerItems(Context context, ref int sequence, int index, Item item)
        {
            var playerItem = new Data.Models.PlayerItem()
            {
                PlayerId = Player.DatabasePlayerId,

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
                    AddPlayerItems(context, ref sequence, playerItem.SequenceId, item2);
                }
            }
        }

        private void AddPlayerDepotItems(Context context, ref int sequence, int index, Item item)
        {
            var playerDepotItem = new Data.Models.PlayerDepotItem()
            {
                PlayerId = Player.DatabasePlayerId,

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
                    AddPlayerDepotItems(context, ref sequence, playerDepotItem.SequenceId, item2);
                }
            }
        }
    }
}