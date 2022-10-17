using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCommand : CommandResult<Player>
    {
        public TileCreatePlayerCommand(Tile tile, Data.Models.Player databasePlayer)
        {
            Tile = tile;

            DatabasePlayer = databasePlayer;
        }

        public Tile Tile { get; set; }

        public Data.Models.Player DatabasePlayer { get; set; }

        public override PromiseResult<Player> Execute(Context context)
        {
            return PromiseResult<Player>.Run(resolve =>
            {
                Player player = context.Server.PlayerFactory.Create();

                player.DatabasePlayerId = DatabasePlayer.Id;

                player.Name = DatabasePlayer.Name;

                player.Health = (ushort)DatabasePlayer.Health;

                player.MaxHealth = (ushort)DatabasePlayer.MaxHealth;

                player.Outfit = DatabasePlayer.OutfitItemId != null ? 
                
                    new Outfit(DatabasePlayer.OutfitItemId.Value) : 
                    
                    new Outfit(DatabasePlayer.OutfitId.Value, DatabasePlayer.OutfitHead.Value, DatabasePlayer.OutfitBody.Value, DatabasePlayer.OutfitLegs.Value, DatabasePlayer.OutfitFeet.Value, (Addon)DatabasePlayer.OutfitAddon.Value);
               
                player.BaseSpeed = (ushort)DatabasePlayer.BaseSpeed;

                player.Speed = (ushort)DatabasePlayer.Speed;

                player.Skills.MagicLevel = (byte)DatabasePlayer.SkillMagicLevel;

                player.Skills.MagicLevelPercent = (byte)DatabasePlayer.SkillMagicLevelPercent;

                player.Skills.Fist = (byte)DatabasePlayer.SkillFist;

                player.Skills.FistPercent = (byte)DatabasePlayer.SkillFistPercent;

                player.Skills.Club = (byte)DatabasePlayer.SkillClub;

                player.Skills.ClubPercent = (byte)DatabasePlayer.SkillClubPercent;

                player.Skills.Sword = (byte)DatabasePlayer.SkillSword;

                player.Skills.SwordPercent = (byte)DatabasePlayer.SkillSwordPercent;

                player.Skills.Axe = (byte)DatabasePlayer.SkillAxe;

                player.Skills.AxePercent = (byte)DatabasePlayer.SkillAxePercent;

                player.Skills.Distance = (byte)DatabasePlayer.SkillDistance;

                player.Skills.DistancePercent = (byte)DatabasePlayer.SkillDistancePercent;

                player.Skills.Shield = (byte)DatabasePlayer.SkillShield;

                player.Skills.ShieldPercent = (byte)DatabasePlayer.SkillShieldPercent;

                player.Skills.Fish = (byte)DatabasePlayer.SkillFish;

                player.Skills.FishPercent = (byte)DatabasePlayer.SkillFishPercent;

                player.Experience = (uint)DatabasePlayer.Experience;

                player.Level = (ushort)DatabasePlayer.Level;

                player.LevelPercent = (byte)DatabasePlayer.LevelPercent;

                player.Mana = (ushort)DatabasePlayer.Mana;

                player.MaxMana = (ushort)DatabasePlayer.MaxMana;

                player.Soul = (byte)DatabasePlayer.Soul;

                player.Capacity = (uint)DatabasePlayer.Capacity;

                player.Stamina = (ushort)DatabasePlayer.Stamina;

                foreach (var databaseItem in DatabasePlayer.PlayerItems.Where(i => i.ParentId >= 1 /* Slot.Head */ && i.ParentId <= 10 /* Slot.Extra */ ) )
                {
                    var item = context.Server.ItemFactory.Create( (ushort)databaseItem.OpenTibiaId, (byte)databaseItem.Count);

                    if (item is Container container)
                    {
                        AddItems(context, DatabasePlayer.PlayerItems, container, databaseItem.SequenceId);
                    }

                    player.Inventory.AddContent(item, (byte)databaseItem.ParentId);
                }

                foreach (var databaseItem in DatabasePlayer.PlayerDepotItems.Where(i => i.ParentId >= 0 /* Town Id */ && i.ParentId <= 100 /* Town Id */ ) )
                {
                    var container = context.Server.Lockers.GetLocker(context, DatabasePlayer.Id, (ushort)databaseItem.ParentId);

                    if (container.Count == 0)
                    {
                        AddItems(context, DatabasePlayer.PlayerDepotItems, container, databaseItem.SequenceId);
                    }
                }

                context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( (ctx, index) =>
                {
                    resolve(ctx, player);
                } );
            } );
        }

        private void AddItems(Context context, ICollection<Data.Models.PlayerItem> databaseItems, Container container, int sequenceId)
        {
            foreach (var databaseItem in databaseItems.Where(i => i.ParentId == sequenceId) )
            {
                var item = context.Server.ItemFactory.Create( (ushort)databaseItem.OpenTibiaId, (byte)databaseItem.Count);

                if (item is Container container2)
                {
                    AddItems(context, DatabasePlayer.PlayerItems, container2, databaseItem.SequenceId);
                }

                container.AddContent(item);
            }
        }

        private void AddItems(Context context, ICollection<Data.Models.PlayerDepotItem> databaseItems, Container container, int sequenceId)
        {
            foreach (var databaseItem in databaseItems.Where(i => i.ParentId == sequenceId) )
            {
                var item = context.Server.ItemFactory.Create( (ushort)databaseItem.OpenTibiaId, (byte)databaseItem.Count);

                if (item is Container container2)
                {
                    AddItems(context, DatabasePlayer.PlayerItems, container2, databaseItem.SequenceId);
                }

                container.AddContent(item);
            }
        }
    }
}