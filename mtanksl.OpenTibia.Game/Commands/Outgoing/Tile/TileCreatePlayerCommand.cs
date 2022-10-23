using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Commands
{
    public class TileCreatePlayerCommand : CommandResult<Player>
    {
        public TileCreatePlayerCommand(Tile tile, IConnection connection, Data.Models.Player databasePlayer)
        {
            Tile = tile;

            Connection = connection;

            DatabasePlayer = databasePlayer;
        }

        public Tile Tile { get; set; }

        public IConnection Connection { get; set; }

        public Data.Models.Player DatabasePlayer { get; set; }

        public override PromiseResult<Player> Execute(Context context)
        {
            return PromiseResult<Player>.Run(resolve =>
            {
                #region Connect

                Client client = new Client(context.Server);

                Connection.Client = client;

                #endregion

                #region Create

                Player player = context.Server.PlayerFactory.Create();

                client.Player = player;

                #endregion

                #region Load player from database

                player.DatabasePlayerId = DatabasePlayer.Id;

                player.Name = DatabasePlayer.Name;

                player.Health = (ushort)DatabasePlayer.Health;

                player.MaxHealth = (ushort)DatabasePlayer.MaxHealth;

                player.Direction = (Direction)DatabasePlayer.Direction;

                if (DatabasePlayer.OutfitId == 0)
                {
                    player.Outfit = new Outfit(DatabasePlayer.OutfitItemId);
                }
                else
                {
                    player.Outfit = new Outfit(DatabasePlayer.OutfitId, DatabasePlayer.OutfitHead, DatabasePlayer.OutfitBody, DatabasePlayer.OutfitLegs, DatabasePlayer.OutfitFeet, (Addon)DatabasePlayer.OutfitAddon);
                }

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

                player.Gender = (Gender)DatabasePlayer.Gender;

                player.Vocation = (Vocation)DatabasePlayer.Vocation;

                #endregion

                #region Load player items from database

                foreach (var databasePlayerItem in DatabasePlayer.PlayerItems.Where(i => i.ParentId >= 1 /* Slot.Head */ && i.ParentId <= 10 /* Slot.Extra */ ) )
                {
                    var item = context.Server.ItemFactory.Create( (ushort)databasePlayerItem.OpenTibiaId, (byte)databasePlayerItem.Count);

                    if (item is Container container)
                    {
                        AddItems(context, DatabasePlayer.PlayerItems, container, databasePlayerItem.SequenceId);
                    }

                    player.Inventory.AddContent(item, (byte)databasePlayerItem.ParentId);
                }

                #endregion

                #region Load player depot items from database

                foreach (var databaseplayerDepotItem in DatabasePlayer.PlayerDepotItems.Where(i => i.ParentId >= 0 /* Town Id */ && i.ParentId <= 100 /* Town Id */ ) )
                {
                    var container = (Container)context.Server.ItemFactory.Create(2591, 1);

                    context.Server.Lockers.AddLocker(DatabasePlayer.Id, (ushort)databaseplayerDepotItem.ParentId, container);

                    AddItems(context, DatabasePlayer.PlayerDepotItems, container, databaseplayerDepotItem.SequenceId);
                }

                #endregion

                context.AddCommand(new TileAddCreatureCommand(Tile, player) ).Then( (ctx, index) =>
                {
                    resolve(ctx, player);
                } );
            } );
        }

        private void AddItems(Context context, ICollection<Data.Models.PlayerItem> databasePlayerItems, Container container, int sequenceId)
        {
            foreach (var databasePlayerItem in databasePlayerItems.Where(i => i.ParentId == sequenceId) )
            {
                var item = context.Server.ItemFactory.Create( (ushort)databasePlayerItem.OpenTibiaId, (byte)databasePlayerItem.Count);

                if (item is Container container2)
                {
                    AddItems(context, DatabasePlayer.PlayerItems, container2, databasePlayerItem.SequenceId);
                }

                container.AddContent(item);
            }
        }

        private void AddItems(Context context, ICollection<Data.Models.PlayerDepotItem> databasePlayerDepotItems, Container container, int sequenceId)
        {
            foreach (var databasePlayerDepotItem in databasePlayerDepotItems.Where(i => i.ParentId == sequenceId) )
            {
                var item = context.Server.ItemFactory.Create( (ushort)databasePlayerDepotItem.OpenTibiaId, (byte)databasePlayerDepotItem.Count);

                if (item is Container container2)
                {
                    AddItems(context, DatabasePlayer.PlayerDepotItems, container2, databasePlayerDepotItem.SequenceId);
                }

                container.AddContent(item);
            }
        }
    }
}