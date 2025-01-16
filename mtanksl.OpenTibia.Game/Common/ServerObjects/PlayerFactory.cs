using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.GameObjectScripts;
using System;
using System.Linq;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class PlayerFactory : IPlayerFactory
    {
        private IServer server;

        public PlayerFactory(IServer server)
        {
            this.server = server;
        }

        public Player Create(int databasePlayerId, int databaseAccountId, string name, Tile town, Tile spawn)
        {
            Player player = new Player()
            {
                DatabasePlayerId = databasePlayerId,

                DatabaseAccountId = databaseAccountId,

                Name = name,

                Town = town,

                Spawn = spawn
            };

            return player;
        }

        public void Attach(Player player)
        {
            player.IsDestroyed = false;

            server.GameObjects.AddGameObject(player);

            GameObjectScript<Player> gameObjectScript = server.GameObjectScripts.GetPlayerGameObjectScript(player.Name);

            if (gameObjectScript != null)
            {
                gameObjectScript.Start(player);
            }
        }

        public bool Detach(Player player)
        {
            if (server.GameObjects.RemoveGameObject(player) )
            {
                GameObjectScript<Player> gameObjectScript = server.GameObjectScripts.GetPlayerGameObjectScript(player.Name);

                if (gameObjectScript != null)
                {
                    gameObjectScript.Stop(player);
                }

                return true;
            }

            return false;
        }

        public void ClearComponentsAndEventHandlers(Player player)
        {
            server.GameObjectComponents.ClearComponents(player);

            server.GameObjectEventHandlers.ClearEventHandlers(player);
        }

        public void Load(DbPlayer dbPlayer, Player player)
        {
            LoadPlayer(Context.Current, dbPlayer, player);

            LoadLockers(Context.Current, dbPlayer, player);

            LoadInventory(Context.Current, dbPlayer, player);

            LoadStorages(Context.Current, dbPlayer, player);

            LoadSpells(Context.Current, dbPlayer, player);

            LoadBlesses(Context.Current, dbPlayer, player);

            LoadAchievements(Context.Current, dbPlayer, player);

            LoadOutfits(Context.Current, dbPlayer, player);

            LoadVips(Context.Current, dbPlayer, player);
        }

        private static void LoadPlayer(Context context, DbPlayer dbPlayer, Player player)
        {
            VocationConfig vocationConfig = context.Server.Vocations.GetVocationById( (byte)dbPlayer.Vocation);

            player.Health = (ushort)dbPlayer.Health;

            player.MaxHealth = (ushort)dbPlayer.MaxHealth;

            player.Direction = (Direction)dbPlayer.Direction;

            player.BaseOutfit = dbPlayer.BaseOutfitId == 0 ? new Outfit(dbPlayer.BaseOutfitItemId) : new Outfit(dbPlayer.BaseOutfitId, dbPlayer.BaseOutfitHead, dbPlayer.BaseOutfitBody, dbPlayer.BaseOutfitLegs, dbPlayer.BaseOutfitFeet, (Addon)dbPlayer.BaseOutfitAddon);

            player.Outfit = dbPlayer.OutfitId == 0 ? new Outfit(dbPlayer.OutfitItemId) : new Outfit(dbPlayer.OutfitId, dbPlayer.OutfitHead, dbPlayer.OutfitBody, dbPlayer.OutfitLegs, dbPlayer.OutfitFeet, (Addon)dbPlayer.OutfitAddon);

            player.BaseSpeed = (ushort)dbPlayer.BaseSpeed;

            player.Speed = (ushort)dbPlayer.Speed;

            player.Invisible = dbPlayer.Invisible;

            player.Skills.MagicLevel = (byte)dbPlayer.SkillMagicLevel;

            player.Skills.MagicLevelTries = (ulong)dbPlayer.SkillMagicLevelTries;

            player.Skills.MagicLevelPercent = (byte)Math.Ceiling(100.0 * player.Skills.MagicLevelTries / Formula.GetRequiredSkillTries(player.Skills.MagicLevel, Skill.MagicLevel, vocationConfig) );

            player.Skills.Fist = (byte)dbPlayer.SkillFist;

            player.Skills.FistTries = (ulong)dbPlayer.SkillFistTries;

            player.Skills.FistPercent = (byte)Math.Ceiling(100.0 * player.Skills.FistTries / Formula.GetRequiredSkillTries(player.Skills.Fist, Skill.Fist, vocationConfig) );

            player.Skills.Club = (byte)dbPlayer.SkillClub;

            player.Skills.ClubTries = (ulong)dbPlayer.SkillClubTries;

            player.Skills.ClubPercent = (byte)Math.Ceiling(100.0 * player.Skills.ClubTries / Formula.GetRequiredSkillTries(player.Skills.Club, Skill.Club, vocationConfig) );

            player.Skills.Sword = (byte)dbPlayer.SkillSword;

            player.Skills.SwordTries = (ulong)dbPlayer.SkillSwordTries;

            player.Skills.SwordPercent = (byte)Math.Ceiling(100.0 * player.Skills.SwordTries / Formula.GetRequiredSkillTries(player.Skills.Sword, Skill.Sword, vocationConfig) );

            player.Skills.Axe = (byte)dbPlayer.SkillAxe;

            player.Skills.AxeTries = (ulong)dbPlayer.SkillAxeTries;

            player.Skills.AxePercent = (byte)Math.Ceiling(100.0 * player.Skills.AxeTries / Formula.GetRequiredSkillTries(player.Skills.Axe, Skill.Axe, vocationConfig) );

            player.Skills.Distance = (byte)dbPlayer.SkillDistance;

            player.Skills.DistanceTries = (ulong)dbPlayer.SkillDistanceTries;

            player.Skills.DistancePercent = (byte)Math.Ceiling(100.0 * player.Skills.DistanceTries / Formula.GetRequiredSkillTries(player.Skills.Distance, Skill.Distance, vocationConfig) );

            player.Skills.Shield = (byte)dbPlayer.SkillShield;

            player.Skills.ShieldTries = (ulong)dbPlayer.SkillShieldTries;

            player.Skills.ShieldPercent = (byte)Math.Ceiling(100.0 * player.Skills.ShieldTries / Formula.GetRequiredSkillTries(player.Skills.Shield, Skill.Shield, vocationConfig) );

            player.Skills.Fish = (byte)dbPlayer.SkillFish;

            player.Skills.FishTries = (ulong)dbPlayer.SkillFishTries;

            player.Skills.FishPercent = (byte)Math.Ceiling(100.0 * player.Skills.FishTries / Formula.GetRequiredSkillTries(player.Skills.Fish, Skill.Fish, vocationConfig) );

            player.Experience = (ulong)dbPlayer.Experience;

            player.Level = (ushort)dbPlayer.Level;

            player.LevelPercent = (byte)Math.Ceiling(100.0 * player.Experience / Formula.GetRequiredExperience(player.Level) );

            player.Mana = (ushort)dbPlayer.Mana;

            player.MaxMana = (ushort)dbPlayer.MaxMana;

            player.Soul = (byte)dbPlayer.Soul;

            player.Capacity = (uint)dbPlayer.Capacity;

            player.Stamina = (ushort)dbPlayer.Stamina;

            player.Gender = (Gender)dbPlayer.Gender;

            player.Vocation = (Vocation)dbPlayer.Vocation;

            player.Rank = (Rank)dbPlayer.Rank;

            player.Premium = dbPlayer.Account.PremiumUntil != null && (dbPlayer.Account.PremiumUntil.Value - DateTime.UtcNow).TotalDays > 0;

            player.BankAccount = (ulong)dbPlayer.BankAccount;
        }

        private static void LoadLockers(Context context, DbPlayer dbPlayer, Player player)
        {
            void AddItems(Container parent, int sequenceId)
            {
                foreach (var playerDepotItem in dbPlayer.PlayerDepotItems.Where(i => i.ParentId == sequenceId) )
                {
                    Item item = context.Server.ItemFactory.Create( (ushort)playerDepotItem.OpenTibiaId, (byte)playerDepotItem.Count);

                    context.Server.ItemFactory.Attach(item);

                    if (item is Container container)
                    {
                        AddItems(container, playerDepotItem.SequenceId);
                    }

                    parent.AddContent(item);
                }
            }

            foreach (var playerDepotItem in dbPlayer.PlayerDepotItems.Where(i => i.ParentId >= 0 /* Town Id */ && i.ParentId <= 100 /* Town Id */ ) )
            {
                Locker locker = (Locker)context.Server.ItemFactory.Create(Constants.LockerOpenTibiaItemId, 1);

                locker.TownId = (ushort)playerDepotItem.ParentId;

                context.Server.ItemFactory.Attach(locker);

                AddItems(locker, playerDepotItem.SequenceId);

                player.Lockers.AddContent(locker, locker.TownId);
            }
        }

        private static void LoadInventory(Context context, DbPlayer dbPlayer, Player player)
        {
            void AddItems(Container parent, int sequenceId)
            {
                foreach (var dbPlayerItem in dbPlayer.PlayerItems.Where(i => i.ParentId == sequenceId) )
                {
                    Item item = context.Server.ItemFactory.Create( (ushort)dbPlayerItem.OpenTibiaId, (byte)dbPlayerItem.Count);

                    context.Server.ItemFactory.Attach(item);

                    if (item is Container container)
                    {
                        AddItems(container, dbPlayerItem.SequenceId);
                    }

                    parent.AddContent(item);
                }
            }

            foreach (var dbPlayerItem in dbPlayer.PlayerItems.Where(i => i.ParentId >= 1 /* Slot.Head */ && i.ParentId <= 10 /* Slot.Extra */ ) )
            {
                Item item = context.Server.ItemFactory.Create( (ushort)dbPlayerItem.OpenTibiaId, (byte)dbPlayerItem.Count);

                context.Server.ItemFactory.Attach(item);

                if (item is Container container)
                {
                    AddItems(container, dbPlayerItem.SequenceId);
                }

                player.Inventory.AddContent(item, (byte)dbPlayerItem.ParentId);
            }
        }

        private static void LoadStorages(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbPlayerStorage in dbPlayer.PlayerStorages)
            {
                player.Storages.SetValue(dbPlayerStorage.Key, dbPlayerStorage.Value);
            }
        }

        private static void LoadSpells(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbPlayerSpell in dbPlayer.PlayerSpells)
            {
                player.Spells.SetSpell(dbPlayerSpell.Name);
            }
        }

        private static void LoadBlesses(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbPlayerBless in dbPlayer.DbPlayerBlesses)
            {
                player.Blesses.SetBless(dbPlayerBless.Name);
            }
        }

        private static void LoadAchievements(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbPlayerAchievement in dbPlayer.PlayerAchievements)
            {
                player.Achievements.SetAchievement(dbPlayerAchievement.Name);
            }
        }

        private static void LoadOutfits(Context context, DbPlayer dbPlayer, Player player)
        {
            if (dbPlayer.PlayerOutfits.Count == 0)
            {
                switch (player.Gender)
                {
                    case Gender.Male:

                        player.Outfits.SetOutfit(Outfit.MaleCitizen.Id, Addon.None);

                        player.Outfits.SetOutfit(Outfit.MaleHunter.Id, Addon.None);

                        player.Outfits.SetOutfit(Outfit.MaleMage.Id, Addon.None);

                        player.Outfits.SetOutfit(Outfit.MaleKnight.Id, Addon.None);

                        break;

                    case Gender.Female:

                        player.Outfits.SetOutfit(Outfit.FemaleCitizen.Id, Addon.None);

                        player.Outfits.SetOutfit(Outfit.FemaleHunter.Id, Addon.None);

                        player.Outfits.SetOutfit(Outfit.FemaleMage.Id, Addon.None);

                        player.Outfits.SetOutfit(Outfit.FemaleKnight.Id, Addon.None);

                        break;

                    default:

                        throw new NotImplementedException();
                }
            }
            else
            {
                foreach (var dbPlayerOutfit in dbPlayer.PlayerOutfits)
                {
                    player.Outfits.SetOutfit( (ushort)dbPlayerOutfit.OutfitId, (Addon)dbPlayerOutfit.OutfitAddon);
                }
            }
        }

        private static void LoadVips(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbVip in dbPlayer.PlayerVips)
            {
                player.Vips.AddVip(dbVip.Vip.Id, dbVip.Vip.Name);
            }
        }

        public void Save(DbPlayer dbPlayer, Player player)
        {
            SavePlayer(Context.Current, dbPlayer, player);

            SaveLockers(Context.Current, dbPlayer, player);

            SaveInventory(Context.Current, dbPlayer, player);

            SaveStorages(Context.Current, dbPlayer, player);

            SaveSpells(Context.Current, dbPlayer, player);

            SaveBlesses(Context.Current, dbPlayer, player);

            SaveAchievements(Context.Current, dbPlayer, player);

            SaveOutfits(Context.Current, dbPlayer, player);

            SaveVips(Context.Current, dbPlayer, player);
        }

        private static void SavePlayer(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.Health = player.Health;

            dbPlayer.MaxHealth = player.MaxHealth;

            dbPlayer.Direction = (int)player.Direction;

            dbPlayer.BaseOutfitItemId = player.BaseOutfit.TibiaId;

            dbPlayer.BaseOutfitId = player.BaseOutfit.Id;

            dbPlayer.BaseOutfitHead = player.BaseOutfit.Head;

            dbPlayer.BaseOutfitBody = player.BaseOutfit.Body;

            dbPlayer.BaseOutfitLegs = player.BaseOutfit.Legs;

            dbPlayer.BaseOutfitFeet = player.BaseOutfit.Feet;

            dbPlayer.BaseOutfitAddon = (int)player.BaseOutfit.Addon;

            dbPlayer.OutfitItemId = player.Outfit.TibiaId;

            dbPlayer.OutfitId = player.Outfit.Id;

            dbPlayer.OutfitHead = player.Outfit.Head;

            dbPlayer.OutfitBody = player.Outfit.Body;

            dbPlayer.OutfitLegs = player.Outfit.Legs;

            dbPlayer.OutfitFeet = player.Outfit.Feet;

            dbPlayer.OutfitAddon = (int)player.Outfit.Addon;

            dbPlayer.BaseSpeed = player.BaseSpeed;

            dbPlayer.Speed = player.Speed;

            dbPlayer.Invisible = player.Invisible;

            dbPlayer.SkillMagicLevel = player.Skills.MagicLevel;

            dbPlayer.SkillMagicLevelTries = (long)player.Skills.MagicLevelTries;

            dbPlayer.SkillFist = player.Skills.Fist;

            dbPlayer.SkillFistTries = (long)player.Skills.FistTries;

            dbPlayer.SkillClub = player.Skills.Club;

            dbPlayer.SkillClubTries = (long)player.Skills.ClubTries;

            dbPlayer.SkillSword = player.Skills.Sword;

            dbPlayer.SkillSwordTries = (long)player.Skills.SwordTries;

            dbPlayer.SkillAxe = player.Skills.Axe;

            dbPlayer.SkillAxeTries = (long)player.Skills.AxeTries;

            dbPlayer.SkillDistance = player.Skills.Distance;

            dbPlayer.SkillDistanceTries = (long)player.Skills.DistanceTries;

            dbPlayer.SkillShield = player.Skills.Shield;

            dbPlayer.SkillShieldTries = (long)player.Skills.ShieldTries;

            dbPlayer.SkillFish = player.Skills.Fish;

            dbPlayer.SkillFishTries = (long)player.Skills.FishTries;

            dbPlayer.Experience = (long)player.Experience;

            dbPlayer.Level = player.Level;

            dbPlayer.Mana = player.Mana;

            dbPlayer.MaxMana = player.MaxMana;

            dbPlayer.Soul = player.Soul;

            dbPlayer.Capacity = (int)player.Capacity;

            dbPlayer.Stamina = player.Stamina;

            dbPlayer.Gender = (int)player.Gender;

            dbPlayer.Vocation = (int)player.Vocation;

            dbPlayer.Rank = (int)player.Rank;

            dbPlayer.SpawnX = player.Spawn.Position.X;

            dbPlayer.SpawnY = player.Spawn.Position.Y;

            dbPlayer.SpawnZ = player.Spawn.Position.Z;

            dbPlayer.BankAccount = (long)player.BankAccount;
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

            foreach (var pair in player.Lockers.GetIndexedContents() )
            {
                AddItems(pair.Key, (Item)pair.Value);
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

            foreach (var pair in player.Outfits.GetIndexed() )
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

            foreach (var pair in player.Storages.GetIndexed() )
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

            foreach (var name in player.Spells.GetSpells() )
            {
                dbPlayer.PlayerSpells.Add(new DbPlayerSpell()
                {
                    PlayerId = dbPlayer.Id,

                    Name = name
                } );
            }
        }

        private static void SaveBlesses(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.DbPlayerBlesses.Clear();

            foreach (var name in player.Blesses.GetBlesses() )
            {
                dbPlayer.DbPlayerBlesses.Add(new DbPlayerBless()
                {
                    PlayerId = dbPlayer.Id,

                    Name = name
                } );
            }
        }

        private static void SaveAchievements(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.PlayerAchievements.Clear();

            foreach (var name in player.Achievements.GetAchievements() )
            {
                dbPlayer.PlayerAchievements.Add(new DbPlayerAchievement()
                {
                    PlayerId = dbPlayer.Id,

                    Name = name
                } );
            }
        }

        private static void SaveVips(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.PlayerVips.Clear();

            foreach (var pair in player.Vips.GetIndexed() )
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