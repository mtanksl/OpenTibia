using OpenTibia.Common.Objects;
using OpenTibia.Data.Models;
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
                    Detach(Context, child);
                }

                foreach (var pair in Context.Server.Lockers.GetIndexedLockers(Player.DatabasePlayerId) )
                {
                    Detach(Context, pair.Value);
                }

                DbPlayer dbPlayer = Context.Database.PlayerRepository.GetPlayerById(Player.DatabasePlayerId);

                    SavePlayer(Context, dbPlayer, Player);

                    SaveLockers(Context, dbPlayer, Player);

                    SaveInventory(Context, dbPlayer, Player);

                    SaveStorages(Context, dbPlayer, Player);

                    SaveSpells(Context, dbPlayer, Player);

                    SaveOutfits(Context, dbPlayer, Player);

                    SaveVips(Context, dbPlayer, Player);

                Context.Database.Commit();

                Context.Server.QueueForExecution( () =>
                {
                    Context.Server.PlayerFactory.Destroy(Player);

                    foreach (var child in Player.Inventory.GetItems() )
                    {
                        Destroy(Context, child);
                    }

                    foreach (var pair in Context.Server.Lockers.GetIndexedLockers(Player.DatabasePlayerId) )
                    {
                        Destroy(Context, pair.Value);
                    }

                    Context.Server.Lockers.ClearLocker(Player.DatabasePlayerId);

                    Tile fromTile = Player.Tile;

                    return Context.AddCommand(new TileRemoveCreatureCommand(fromTile, Player) ).Then( () =>
                    {
                        return Context.AddCommand(new PlayerLogoutCommand(Player, fromTile) );
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

        private static void Destroy(Context context, Item item)
        {
            context.Server.ItemFactory.Destroy(item);

            if (item is Container container)
	        {
		        foreach (var child in container.GetItems() )
		        {
                    Destroy(context, child);
		        }
	        }
        }

        private static void SavePlayer(Context context, DbPlayer dbPlayer, Player player)
        {
            // dbPlayer.Health = player.Health;

            // dbPlayer.MaxHealth = player.MaxHealth;

            dbPlayer.Direction = (int)player.Direction;

            dbPlayer.BaseOutfitItemId = player.BaseOutfit.ItemId;

            dbPlayer.BaseOutfitId = player.BaseOutfit.Id;

            dbPlayer.BaseOutfitHead = player.BaseOutfit.Head;

            dbPlayer.BaseOutfitBody = player.BaseOutfit.Body;

            dbPlayer.BaseOutfitLegs = player.BaseOutfit.Legs;

            dbPlayer.BaseOutfitFeet = player.BaseOutfit.Feet;

            dbPlayer.BaseOutfitAddon = (int)player.BaseOutfit.Addon;

            dbPlayer.OutfitItemId = player.Outfit.ItemId;

            dbPlayer.OutfitId = player.Outfit.Id;

            dbPlayer.OutfitHead = player.Outfit.Head;

            dbPlayer.OutfitBody = player.Outfit.Body;

            dbPlayer.OutfitLegs = player.Outfit.Legs;

            dbPlayer.OutfitFeet = player.Outfit.Feet;

            dbPlayer.OutfitAddon = (int)player.Outfit.Addon;

            // dbPlayer.BaseSpeed = player.BaseSpeed;

            // dbPlayer.Speed = player.Speed;

            dbPlayer.Invisible = player.Invisible;

            dbPlayer.SkillMagicLevel = player.Skills.MagicLevel;

            dbPlayer.SkillMagicLevelPercent = player.Skills.MagicLevelPercent;

            dbPlayer.SkillFist = player.Skills.Fist;

            dbPlayer.SkillFistPercent = player.Skills.FistPercent;

            dbPlayer.SkillClub = player.Skills.Club;

            dbPlayer.SkillClubPercent = player.Skills.ClubPercent;

            dbPlayer.SkillSword = player.Skills.Sword;

            dbPlayer.SkillSwordPercent = player.Skills.SwordPercent;

            dbPlayer.SkillAxe = player.Skills.Axe;

            dbPlayer.SkillAxePercent = player.Skills.AxePercent;

            dbPlayer.SkillDistance = player.Skills.Distance;

            dbPlayer.SkillDistancePercent = player.Skills.DistancePercent;

            dbPlayer.SkillShield = player.Skills.Shield;

            dbPlayer.SkillShieldPercent = player.Skills.ShieldPercent;

            dbPlayer.SkillFish = player.Skills.Fish;

            dbPlayer.SkillFishPercent = player.Skills.FishPercent;

            dbPlayer.Experience = (int)player.Experience;

            dbPlayer.Level = player.Level;

            dbPlayer.LevelPercent = player.LevelPercent;

            // dbPlayer.Mana = player.Mana;

            // dbPlayer.MaxMana = player.MaxMana;

            dbPlayer.Soul = player.Soul;

            dbPlayer.Capacity = (int)player.Capacity;

            dbPlayer.Stamina = player.Stamina;

            dbPlayer.Gender = (int)player.Gender;

            dbPlayer.Vocation = (int)player.Vocation;

            dbPlayer.Rank = (int)player.Rank;

            dbPlayer.SpawnX = player.Tile.Position.X;

            dbPlayer.SpawnY = player.Tile.Position.Y;

            dbPlayer.SpawnZ = player.Tile.Position.Z;
        }

        private static void SaveLockers(Context context, DbPlayer dbPlayer, Player player)
        {
            int sequenceId = 101;

            void AddItems(int parentId, Item item)
            {
                DbPlayerDepotItem dbPlayerDepotItem = new DbPlayerDepotItem()
                {
                    PlayerId = dbPlayer.Id,

                    SequenceId = sequenceId++,

                    ParentId = parentId,

                    OpenTibiaId = item.Metadata.OpenTibiaId,

                    Count = item is StackableItem stackableItem ? stackableItem.Count :

                            item is FluidItem fluidItem ? (int)fluidItem.FluidType :

                            item is SplashItem splashItem ? (int)splashItem.FluidType : 1
                };

                dbPlayer.PlayerDepotItems.Add(dbPlayerDepotItem);

                if (item is Container container)
                {
                    foreach (var child in container.GetItems().Reverse() )
                    {
                        AddItems(dbPlayerDepotItem.SequenceId, child);
                    }
                }
            }

            dbPlayer.PlayerDepotItems.Clear();

            foreach (var pair in context.Server.Lockers.GetIndexedLockers(dbPlayer.Id) )
            {
                AddItems(pair.Key, pair.Value);
            }
        }

        private static void SaveInventory(Context context, DbPlayer dbPlayer, Player player)
        {
            int sequenceId = 101;

            void AddItems(int parentId, Item item)
            {
                DbPlayerItem dbPlayerItem = new DbPlayerItem()
                {
                    PlayerId = dbPlayer.Id,

                    SequenceId = sequenceId++,

                    ParentId = parentId,

                    OpenTibiaId = item.Metadata.OpenTibiaId,

                    Count = item is StackableItem stackableItem ? stackableItem.Count :

                            item is FluidItem fluidItem ? (int)fluidItem.FluidType :

                            item is SplashItem splashItem ? (int)splashItem.FluidType : 1
                };

                dbPlayer.PlayerItems.Add(dbPlayerItem);

                if (item is Container container)
                {
                    foreach (var child in container.GetItems().Reverse() )
                    {
                        AddItems(dbPlayerItem.SequenceId, child);
                    }
                }
            }

            dbPlayer.PlayerItems.Clear();

            foreach (var pair in player.Inventory.GetIndexedContents() )
            {
                AddItems(pair.Key, (Item)pair.Value);
            }
        }

        private static void SaveOutfits(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.PlayerOutfits.Clear();

            foreach (var pair in player.Client.Outfits.GetIndexed() )
            {
                dbPlayer.PlayerOutfits.Add(new DbPlayerOutfit()
                {
                    PlayerId = dbPlayer.Id,

                    OutfitId = (int)pair.Key,

                    OutfitAddon = (int)pair.Value
                } );
            }            
        }

        private static void SaveStorages(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.PlayerStorages.Clear();

            foreach (var pair in player.Client.Storages.GetIndexed() )
            {
                dbPlayer.PlayerStorages.Add(new DbPlayerStorage()
                {
                    PlayerId = dbPlayer.Id,

                    Key = pair.Key,

                    Value = pair.Value
                } );
            }            
        }

        private static void SaveSpells(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.PlayerSpells.Clear();

            foreach (var name in player.Client.Spells.GetSpells() )
            {
                dbPlayer.PlayerSpells.Add(new DbPlayerSpell()
                {
                    PlayerId = dbPlayer.Id,

                    Name = name
                } );
            }
        }

        private static void SaveVips(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.PlayerVips.Clear();

            foreach (var pair in player.Client.Vips.GetIndexed() )
            {
                dbPlayer.PlayerVips.Add(new DbPlayerVip()
                {
                    PlayerId = dbPlayer.Id,

                    VipId = pair.Key
                } );
            }
        }
    }
}