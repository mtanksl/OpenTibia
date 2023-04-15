using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Components;
using OpenTibia.Game.Strategies;

namespace OpenTibia.Game
{
    public class PlayerFactory
    {
        private Server server;

        public PlayerFactory(Server server)
        {
            this.server = server;
        }

        public Player Create(IConnection connection, Data.Models.Player databasePlayer)
        {
            Client client = new Client(server)
            {
                Connection = connection
            };

            Player player = new Player()
            {
                Client = client,

                DatabasePlayerId = databasePlayer.Id,

                Name = databasePlayer.Name,

                Health = (ushort)databasePlayer.Health,

                MaxHealth = (ushort)databasePlayer.MaxHealth,

                Direction = (Direction)databasePlayer.Direction,

                Outfit = databasePlayer.OutfitId == 0 ? new Outfit(databasePlayer.OutfitItemId) : new Outfit(databasePlayer.OutfitId, databasePlayer.OutfitHead, databasePlayer.OutfitBody, databasePlayer.OutfitLegs, databasePlayer.OutfitFeet, (Addon)databasePlayer.OutfitAddon),

                BaseSpeed = (ushort)databasePlayer.BaseSpeed,

                Speed = (ushort)databasePlayer.Speed,

                Skills = {

                    MagicLevel = (byte)databasePlayer.SkillMagicLevel,

                    MagicLevelPercent = (byte)databasePlayer.SkillMagicLevelPercent,

                    Fist = (byte)databasePlayer.SkillFist,

                    FistPercent = (byte)databasePlayer.SkillFistPercent,

                    Club = (byte)databasePlayer.SkillClub,

                    ClubPercent = (byte)databasePlayer.SkillClubPercent,

                    Sword = (byte)databasePlayer.SkillSword,

                    SwordPercent = (byte)databasePlayer.SkillSwordPercent,

                    Axe = (byte)databasePlayer.SkillAxe,

                    AxePercent = (byte)databasePlayer.SkillAxePercent,

                    Distance = (byte)databasePlayer.SkillDistance,

                    DistancePercent = (byte)databasePlayer.SkillDistancePercent,

                    Shield = (byte)databasePlayer.SkillShield,

                    ShieldPercent = (byte)databasePlayer.SkillShieldPercent,

                    Fish = (byte)databasePlayer.SkillFish,

                    FishPercent = (byte)databasePlayer.SkillFishPercent
                },

                Experience = (uint)databasePlayer.Experience,

                Level = (ushort)databasePlayer.Level,

                LevelPercent = (byte)databasePlayer.LevelPercent,

                Mana = (ushort)databasePlayer.Mana,

                MaxMana = (ushort)databasePlayer.MaxMana,

                Soul = (byte)databasePlayer.Soul,

                Capacity = (uint)databasePlayer.Capacity,

                Stamina = (ushort)databasePlayer.Stamina,

                Gender = (Gender)databasePlayer.Gender,

                Vocation = (Vocation)databasePlayer.Vocation
            };

            //TODO

            server.GameObjects.AddGameObject(player);

            server.Components.AddComponent(player, new CreatureCooldownBehaviour() );

            server.Components.AddComponent(player, new CreatureSpecialConditionBehaviour() );

            server.Components.AddComponent(player, new PlayerAttackAndFollowBehaviour(new CloseAttackStrategy(500, (attacker, target) => -server.Randomization.Take(0, 20) ), new FollowWalkStrategy() ) );

            server.Logger.WriteLine(player.Name + " login.", LogLevel.Information);

            return player;
        }

        public void Destroy(Player player)
        {
            player.Client.Player = null;

            //TODO

            server.GameObjects.RemoveGameObject(player);

            server.Components.ClearComponents(player);

            server.Logger.WriteLine(player.Name + " logout.", LogLevel.Information);
        }
    }
}