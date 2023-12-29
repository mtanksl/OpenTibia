using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.GameObjectScripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace OpenTibia.Game
{
    public class PlayerFactory
    {
        private Server server;

        public PlayerFactory(Server server)
        {
            this.server = server;
        }

        public void Start()
        {
            gameObjectScripts = new Dictionary<string, GameObjectScript<string, Player> >();
#if AOT
            foreach (var gameObjectScript in _AotCompilation.Players)
            {
                gameObjectScripts.Add(gameObjectScript.Key, gameObjectScript);
            }
#else
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes().Where(t => typeof(GameObjectScript<string, Player>).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract) )
            {
                GameObjectScript<string, Player> gameObjectScript = (GameObjectScript<string, Player>)Activator.CreateInstance(type);

                gameObjectScripts.Add(gameObjectScript.Key, gameObjectScript);
            }
#endif
        }

        private Dictionary<string, GameObjectScript<string, Player> > gameObjectScripts;

        public GameObjectScript<string, Player> GetPlayerGameObjectScript(string name)
        {
            GameObjectScript<string, Player> gameObjectScript;

            if (gameObjectScripts.TryGetValue(name, out gameObjectScript) )
            {
                return gameObjectScript;
            }
            
            if (gameObjectScripts.TryGetValue("", out gameObjectScript) )
            {
                return gameObjectScript;
            }

            return null;
        }

        public Player Create(int databasePlayerId, string name, Tile spawn)
        {
            Player player = new Player()
            {
                DatabasePlayerId = databasePlayerId,

                Name = name,

                Town = spawn,

                Spawn = spawn
            };

            return player;
        }

        public void Attach(Player player)
        {
            player.IsDestroyed = false;

            server.GameObjects.AddGameObject(player);

            GameObjectScript<string, Player> gameObjectScript = GetPlayerGameObjectScript(player.Name);

            if (gameObjectScript != null)
            {
                gameObjectScript.Start(player);
            }
        }

        public bool Detach(Player player)
        {
            if (server.GameObjects.RemoveGameObject(player) )
            {
                GameObjectScript<string, Player> gameObjectScript = GetPlayerGameObjectScript(player.Name);

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

            LoadOutfits(Context.Current, dbPlayer, player);

            LoadVips(Context.Current, dbPlayer, player);
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

            player.Skills.MagicLevel = (byte)dbPlayer.SkillMagicLevel;

            player.Skills.MagicLevelPercent = (byte)dbPlayer.SkillMagicLevelPercent;

            player.Skills.Fist = (byte)dbPlayer.SkillFist;

            player.Skills.FistPercent = (byte)dbPlayer.SkillFistPercent;

            player.Skills.Club = (byte)dbPlayer.SkillClub;

            player.Skills.ClubPercent = (byte)dbPlayer.SkillClubPercent;

            player.Skills.Sword = (byte)dbPlayer.SkillSword;

            player.Skills.SwordPercent = (byte)dbPlayer.SkillSwordPercent;

            player.Skills.Axe = (byte)dbPlayer.SkillAxe;

            player.Skills.AxePercent = (byte)dbPlayer.SkillAxePercent;

            player.Skills.Distance = (byte)dbPlayer.SkillDistance;

            player.Skills.DistancePercent = (byte)dbPlayer.SkillDistancePercent;

            player.Skills.Shield = (byte)dbPlayer.SkillShield;

            player.Skills.ShieldPercent = (byte)dbPlayer.SkillShieldPercent;

            player.Skills.Fish = (byte)dbPlayer.SkillFish;

            player.Skills.FishPercent = (byte)dbPlayer.SkillFishPercent;

            player.Experience = (uint)dbPlayer.Experience;

            player.Level = (ushort)dbPlayer.Level;

            player.LevelPercent = (byte)dbPlayer.LevelPercent;

            player.Mana = (ushort)dbPlayer.Mana;

            player.MaxMana = (ushort)dbPlayer.MaxMana;

            player.Soul = (byte)dbPlayer.Soul;

            player.Capacity = (uint)dbPlayer.Capacity;

            player.Stamina = (ushort)dbPlayer.Stamina;

            player.Gender = (Gender)dbPlayer.Gender;

            player.Vocation = (Vocation)dbPlayer.Vocation;

            player.Rank = (Rank)dbPlayer.Rank;
        }

        private static void LoadLockers(Context context, DbPlayer dbPlayer, Player player)
        {
            void AddItems(Container parent, int sequenceId)
            {
                foreach (var playerDepotItem in dbPlayer.PlayerDepotItems.Where(i => i.ParentId == sequenceId) )
                {
                    var item = context.Server.ItemFactory.Create((ushort)playerDepotItem.OpenTibiaId, (byte)playerDepotItem.Count);

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
                var container = (Container)context.Server.ItemFactory.Create(2591, 1);

                context.Server.ItemFactory.Attach(container);

                AddItems(container, playerDepotItem.SequenceId);

                context.Server.Lockers.AddLocker(dbPlayer.Id, (ushort)playerDepotItem.ParentId, container);
            }
        }

        private static void LoadInventory(Context context, DbPlayer dbPlayer, Player player)
        {
            void AddItems(Container parent, int sequenceId)
            {
                foreach (var dbPlayerItem in dbPlayer.PlayerItems.Where(i => i.ParentId == sequenceId) )
                {
                    var item = context.Server.ItemFactory.Create((ushort)dbPlayerItem.OpenTibiaId, (byte)dbPlayerItem.Count);

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
                var item = context.Server.ItemFactory.Create((ushort)dbPlayerItem.OpenTibiaId, (byte)dbPlayerItem.Count);

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
                player.Client.Storages.SetValue(dbPlayerStorage.Key, dbPlayerStorage.Value);
            }
        }

        private static void LoadSpells(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbPlayerStorage in dbPlayer.PlayerSpells)
            {
                player.Client.Spells.SetSpell(dbPlayerStorage.Name);
            }
        }

        private static void LoadOutfits(Context context, DbPlayer dbPlayer, Player player)
        {
            if (dbPlayer.PlayerOutfits.Count == 0)
            {
                switch (player.Gender)
                {
                    case Gender.Male:

                        player.Client.Outfits.SetOutfit(Outfit.MaleCitizen.Id, Addon.None);

                        player.Client.Outfits.SetOutfit(Outfit.MaleHunter.Id, Addon.None);

                        player.Client.Outfits.SetOutfit(Outfit.MaleMage.Id, Addon.None);

                        player.Client.Outfits.SetOutfit(Outfit.MaleKnight.Id, Addon.None);

                        break;

                    case Gender.Female:

                        player.Client.Outfits.SetOutfit(Outfit.FemaleCitizen.Id, Addon.None);

                        player.Client.Outfits.SetOutfit(Outfit.FemaleHunter.Id, Addon.None);

                        player.Client.Outfits.SetOutfit(Outfit.FemaleMage.Id, Addon.None);

                        player.Client.Outfits.SetOutfit(Outfit.FemaleKnight.Id, Addon.None);

                        break;

                    default:

                        throw new NotImplementedException();
                }
            }
            else
            {
                foreach (var dbPlayerOutfit in dbPlayer.PlayerOutfits)
                {
                    player.Client.Outfits.SetOutfit((ushort)dbPlayerOutfit.OutfitId, (Addon)dbPlayerOutfit.OutfitAddon);
                }
            }
        }

        private static void LoadVips(Context context, DbPlayer dbPlayer, Player player)
        {
            foreach (var dbVip in dbPlayer.PlayerVips)
            {
                player.Client.Vips.AddVip(dbVip.Vip.Id, dbVip.Vip.Name);
            }
        }

        public void Save(DbPlayer dbPlayer, Player player)
        {
            SavePlayer(Context.Current, dbPlayer, player);

            SaveLockers(Context.Current, dbPlayer, player);

            SaveInventory(Context.Current, dbPlayer, player);

            SaveStorages(Context.Current, dbPlayer, player);

            SaveSpells(Context.Current, dbPlayer, player);

            SaveOutfits(Context.Current, dbPlayer, player);

            SaveVips(Context.Current, dbPlayer, player);
        }

        private static void SavePlayer(Context context, DbPlayer dbPlayer, Player player)
        {
            dbPlayer.Health = player.Health;

            dbPlayer.MaxHealth = player.MaxHealth;

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

            dbPlayer.BaseSpeed = player.BaseSpeed;

            dbPlayer.Speed = player.Speed;

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