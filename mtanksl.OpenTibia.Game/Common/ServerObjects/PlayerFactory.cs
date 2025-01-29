using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.GameObjectScripts;
using System;
using System.Linq;
using static OpenTibia.Common.Objects.PlayerCombatCollection;

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

            server.PositionalEventHandlers.ClearEventHandlers(player);
        }

        // TODO: Optimize, we don't need to load and save everything

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

            LoadKills(Context.Current, dbPlayer, player);

            LoadDeaths(Context.Current, dbPlayer, player);
        }

        private static void LoadPlayer(Context context, DbPlayer dbPlayer, Player player)
        {
            player.Health = (ushort)dbPlayer.Health;

            player.MaxHealth = (ushort)dbPlayer.MaxHealth;

            player.Direction = (Direction)dbPlayer.Direction;

            player.BaseOutfit = dbPlayer.BaseOutfitId == 0 ? new Outfit(dbPlayer.BaseOutfitItemId) : new Outfit(dbPlayer.BaseOutfitId, dbPlayer.BaseOutfitHead, dbPlayer.BaseOutfitBody, dbPlayer.BaseOutfitLegs, dbPlayer.BaseOutfitFeet, (Addon)dbPlayer.BaseOutfitAddon);

            player.Outfit = dbPlayer.OutfitId == 0 ? new Outfit(dbPlayer.OutfitItemId) : new Outfit(dbPlayer.OutfitId, dbPlayer.OutfitHead, dbPlayer.OutfitBody, dbPlayer.OutfitLegs, dbPlayer.OutfitFeet, (Addon)dbPlayer.OutfitAddon);

            player.BaseSpeed = (ushort)dbPlayer.BaseSpeed;

            player.Speed = (ushort)dbPlayer.Speed;

            player.Invisible = dbPlayer.Invisible;

            player.Level = (ushort)dbPlayer.Level;

            player.Experience = (ulong)dbPlayer.Experience;

            player.Experience = Formula.FixRequiredExperience(player.Level, player.Experience);

            player.LevelPercent = Formula.GetLevelPercent(player.Level, player.Experience);

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

            player.Skills.SetSkillLevel(Skill.MagicLevel, (byte)dbPlayer.SkillMagicLevel);

            player.Skills.SetSkillPoints(Skill.MagicLevel, (ulong)dbPlayer.SkillMagicLevelPoints);

            player.Skills.SetSkillLevel(Skill.MagicLevel, Formula.FixRequiredSkillLevel(player, Skill.MagicLevel) );

            player.Skills.SetSkillPoints(Skill.MagicLevel, Formula.FixRequiredSkillPoints(player, Skill.MagicLevel) );

            player.Skills.SetSkillPercent(Skill.MagicLevel, Formula.GetSkillPercent(player, Skill.MagicLevel) );

            player.Skills.SetSkillLevel(Skill.Fist, (byte)dbPlayer.SkillFist);

            player.Skills.SetSkillPoints(Skill.Fist, (ulong)dbPlayer.SkillFistPoints);

            player.Skills.SetSkillLevel(Skill.Fist, Formula.FixRequiredSkillLevel(player, Skill.Fist) );

            player.Skills.SetSkillPoints(Skill.Fist, Formula.FixRequiredSkillPoints(player, Skill.Fist) );

            player.Skills.SetSkillPercent(Skill.Fist, Formula.GetSkillPercent(player, Skill.Fist) );

            player.Skills.SetSkillLevel(Skill.Club, (byte)dbPlayer.SkillClub);

            player.Skills.SetSkillPoints(Skill.Club, (ulong)dbPlayer.SkillClubPoints);

            player.Skills.SetSkillLevel(Skill.Club, Formula.FixRequiredSkillLevel(player, Skill.Club) );

            player.Skills.SetSkillPoints(Skill.Club, Formula.FixRequiredSkillPoints(player, Skill.Club) );

            player.Skills.SetSkillPercent(Skill.Club, Formula.GetSkillPercent(player, Skill.Club) );

            player.Skills.SetSkillLevel(Skill.Sword, (byte)dbPlayer.SkillSword);

            player.Skills.SetSkillPoints(Skill.Sword, (ulong)dbPlayer.SkillSwordPoints);

            player.Skills.SetSkillLevel(Skill.Sword, Formula.FixRequiredSkillLevel(player, Skill.Sword) );

            player.Skills.SetSkillPoints(Skill.Sword, Formula.FixRequiredSkillPoints(player, Skill.Sword) );

            player.Skills.SetSkillPercent(Skill.Sword, Formula.GetSkillPercent(player, Skill.Sword) );

            player.Skills.SetSkillLevel(Skill.Axe, (byte)dbPlayer.SkillAxe);

            player.Skills.SetSkillPoints(Skill.Axe, (ulong)dbPlayer.SkillAxePoints);

            player.Skills.SetSkillLevel(Skill.Axe, Formula.FixRequiredSkillLevel(player, Skill.Axe) );

            player.Skills.SetSkillPoints(Skill.Axe, Formula.FixRequiredSkillPoints(player, Skill.Axe) );

            player.Skills.SetSkillPercent(Skill.Axe, Formula.GetSkillPercent(player, Skill.Axe) );

            player.Skills.SetSkillLevel(Skill.Distance, (byte)dbPlayer.SkillDistance);

            player.Skills.SetSkillPoints(Skill.Distance, (ulong)dbPlayer.SkillDistancePoints);

            player.Skills.SetSkillLevel(Skill.Distance, Formula.FixRequiredSkillLevel(player, Skill.Distance) );

            player.Skills.SetSkillPoints(Skill.Distance, Formula.FixRequiredSkillPoints(player, Skill.Distance) );

            player.Skills.SetSkillPercent(Skill.Distance, Formula.GetSkillPercent(player, Skill.Distance) );

            player.Skills.SetSkillLevel(Skill.Shield, (byte)dbPlayer.SkillShield);

            player.Skills.SetSkillPoints(Skill.Shield, (ulong)dbPlayer.SkillShieldPoints);

            player.Skills.SetSkillLevel(Skill.Shield, Formula.FixRequiredSkillLevel(player, Skill.Shield) );

            player.Skills.SetSkillPoints(Skill.Shield, Formula.FixRequiredSkillPoints(player, Skill.Shield) );

            player.Skills.SetSkillPercent(Skill.Shield, Formula.GetSkillPercent(player, Skill.Shield) );

            player.Skills.SetSkillLevel(Skill.Fish, (byte)dbPlayer.SkillFish);

            player.Skills.SetSkillPoints(Skill.Fish, (ulong)dbPlayer.SkillFishPoints);

            player.Skills.SetSkillLevel(Skill.Fish, Formula.FixRequiredSkillLevel(player, Skill.Fish) );

            player.Skills.SetSkillPoints(Skill.Fish, Formula.FixRequiredSkillPoints(player, Skill.Fish) );

            player.Skills.SetSkillPercent(Skill.Fish, Formula.GetSkillPercent(player, Skill.Fish) );
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
            foreach (var dbPlayerBless in dbPlayer.PlayerBlesses)
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
            foreach (var dbPlayerOutfit in dbPlayer.PlayerOutfits)
            {
                player.Outfits.SetOutfit( (ushort)dbPlayerOutfit.OutfitId, (Addon)dbPlayerOutfit.OutfitAddon);
            }
        }

        private static void LoadVips(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbPlayerVip in dbPlayer.PlayerVips)
            {
                player.Vips.AddVip(dbPlayerVip.Vip.Id, dbPlayerVip.Vip.Name);
            }
        }

        private static void LoadKills(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbPlayerKill in dbPlayer.PlayerKills)
            {
                player.Combat.AddKill(dbPlayerKill.Id, dbPlayerKill.TargetId, dbPlayerKill.Unjustified, dbPlayerKill.CreationDate);
            }
        }

        private static void LoadDeaths(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbPlayerDeath in dbPlayer.PlayerDeaths)
            {
                player.Combat.AddDeath(dbPlayerDeath.Id, dbPlayerDeath.AttackerId, dbPlayerDeath.Name, dbPlayerDeath.Level, dbPlayerDeath.Unjustified, dbPlayerDeath.CreationDate);
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

            SaveKills(Context.Current, dbPlayer, player);

            SaveDeaths(Context.Current, dbPlayer, player);
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

            dbPlayer.SkillMagicLevel = player.Skills.GetSkillLevel(Skill.MagicLevel);

            dbPlayer.SkillMagicLevelPoints = (long)player.Skills.GetSkillPoints(Skill.MagicLevel);

            dbPlayer.SkillFist = player.Skills.GetSkillLevel(Skill.Fist);

            dbPlayer.SkillFistPoints = (long)player.Skills.GetSkillPoints(Skill.Fist);

            dbPlayer.SkillClub = player.Skills.GetSkillLevel(Skill.Club);

            dbPlayer.SkillClubPoints = (long)player.Skills.GetSkillPoints(Skill.Club);

            dbPlayer.SkillSword = player.Skills.GetSkillLevel(Skill.Sword);

            dbPlayer.SkillSwordPoints = (long)player.Skills.GetSkillPoints(Skill.Sword);

            dbPlayer.SkillAxe = player.Skills.GetSkillLevel(Skill.Axe);

            dbPlayer.SkillAxePoints = (long)player.Skills.GetSkillPoints(Skill.Axe);

            dbPlayer.SkillDistance = player.Skills.GetSkillLevel(Skill.Distance);

            dbPlayer.SkillDistancePoints = (long)player.Skills.GetSkillPoints(Skill.Distance);

            dbPlayer.SkillShield = player.Skills.GetSkillLevel(Skill.Shield);

            dbPlayer.SkillShieldPoints = (long)player.Skills.GetSkillPoints(Skill.Shield);

            dbPlayer.SkillFish = player.Skills.GetSkillLevel(Skill.Fish);

            dbPlayer.SkillFishPoints = (long)player.Skills.GetSkillPoints(Skill.Fish);

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
            dbPlayer.PlayerBlesses.Clear();

            foreach (var name in player.Blesses.GetBlesses() )
            {
                dbPlayer.PlayerBlesses.Add(new DbPlayerBless()
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

        private static void SaveKills(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.PlayerKills.Clear();

            foreach (var kill in player.Combat.GetKills() )
            {
                dbPlayer.PlayerKills.Add(new DbPlayerKill()
                {
                    Id = kill.Id,

                    TargetId = kill.TargetId,

                    Unjustified = kill.Unjustified,

                    CreationDate = kill.CreationDate
                } );
            }
        }

        private static void SaveDeaths(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.PlayerDeaths.Clear();

            foreach (var death in player.Combat.GetDeaths() )
            {
                dbPlayer.PlayerDeaths.Add(new DbPlayerDeath()
                {
                    Id = death.Id,

                    AttackerId = death.AttackerId,

                    Name = death.Name,

                    Level = death.Level,

                    Unjustified = death.Unjustified,

                    CreationDate = death.CreationDate
                } );
            }
        }
    }
}