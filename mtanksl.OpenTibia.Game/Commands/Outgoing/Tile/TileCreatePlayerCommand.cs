using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using System;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCommand : CommandResult<Player>
    {
        public TileCreatePlayerCommand(Tile tile, Client client, DbPlayer dbPlayer)
        {
            Tile = tile;

            Client = client;

            DbPlayer = dbPlayer;
        }

        public Tile Tile { get; set; }

        public Client Client { get; set; }

        public DbPlayer DbPlayer { get; set; }

        public override PromiseResult<Player> Execute()
        {
            Player player = Context.Server.PlayerFactory.Create(DbPlayer.Name, Tile);

            if (player != null)
            {
                Context.Server.PlayerFactory.Attach(player);

                player.Client = Client;

                LoadPlayer(Context, DbPlayer, player);

                LoadLockers(Context, DbPlayer, player);

                LoadInventory(Context, DbPlayer, player);

                LoadStorages(Context, DbPlayer, player);

                LoadOutfits(Context, DbPlayer, player);

                LoadVips(Context, DbPlayer, player);

                return Context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( () =>
                {
                    return Context.AddCommand(new PlayerLoginCommand(player) );     
                    
                } ).Then( () =>
                {
                    return Promise.FromResult(player); 
                } );
            }

            return Promise.FromResult(player);
        }

        private static void LoadPlayer(Context context, DbPlayer dbPlayer, Player player)
        {
            player.DatabasePlayerId = dbPlayer.Id;

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
                    var item = context.Server.ItemFactory.Create( (ushort)playerDepotItem.OpenTibiaId, (byte)playerDepotItem.Count);

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
                    var item = context.Server.ItemFactory.Create( (ushort)dbPlayerItem.OpenTibiaId, (byte)dbPlayerItem.Count);

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
                var item = context.Server.ItemFactory.Create( (ushort)dbPlayerItem.OpenTibiaId, (byte)dbPlayerItem.Count);

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

        private static void LoadOutfits(Context context, DbPlayer dbPlayer, Player player)
        {
            if (dbPlayer.PlayerOutfits.Count == 0)
            {
                switch (player.Gender)
                {
                    case Gender.Male:

                        player.Client.Outfits.SetValue(Outfit.MaleCitizen.Id, Addon.None);

                        player.Client.Outfits.SetValue(Outfit.MaleHunter.Id, Addon.None);

                        player.Client.Outfits.SetValue(Outfit.MaleMage.Id, Addon.None);

                        player.Client.Outfits.SetValue(Outfit.MaleKnight.Id, Addon.None);

                        break;

                    case Gender.Female:

                        player.Client.Outfits.SetValue(Outfit.FemaleCitizen.Id, Addon.None);

                        player.Client.Outfits.SetValue(Outfit.FemaleHunter.Id, Addon.None);

                        player.Client.Outfits.SetValue(Outfit.FemaleMage.Id, Addon.None);

                        player.Client.Outfits.SetValue(Outfit.FemaleKnight.Id, Addon.None);

                        break;

                    default:

                        throw new NotImplementedException();
                }
            }
            else
            {
                foreach (var dbPlayerOutfit in dbPlayer.PlayerOutfits)
                {
                    player.Client.Outfits.SetValue( (ushort)dbPlayerOutfit.OutfitId, (Addon)dbPlayerOutfit.OutfitAddon);
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
    }
}