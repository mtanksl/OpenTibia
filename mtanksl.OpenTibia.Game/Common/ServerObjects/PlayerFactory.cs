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

        public Player Create(Data.Models.Player databasePlayer)
        {
            Player player = new Player();

            player.DatabasePlayerId = databasePlayer.Id;

            player.Name = databasePlayer.Name;

            player.Health = (ushort)databasePlayer.Health;

            player.MaxHealth = (ushort)databasePlayer.MaxHealth;

            player.Direction = (Direction)databasePlayer.Direction;

            if (databasePlayer.OutfitId == 0)
            {
                player.Outfit = new Outfit(databasePlayer.OutfitItemId);
            }
            else
            {
                player.Outfit = new Outfit(databasePlayer.OutfitId, databasePlayer.OutfitHead, databasePlayer.OutfitBody, databasePlayer.OutfitLegs, databasePlayer.OutfitFeet, (Addon)databasePlayer.OutfitAddon);
            }

            player.BaseSpeed = (ushort)databasePlayer.BaseSpeed;

            player.Speed = (ushort)databasePlayer.Speed;

            player.Skills.MagicLevel = (byte)databasePlayer.SkillMagicLevel;

            player.Skills.MagicLevelPercent = (byte)databasePlayer.SkillMagicLevelPercent;

            player.Skills.Fist = (byte)databasePlayer.SkillFist;

            player.Skills.FistPercent = (byte)databasePlayer.SkillFistPercent;

            player.Skills.Club = (byte)databasePlayer.SkillClub;

            player.Skills.ClubPercent = (byte)databasePlayer.SkillClubPercent;

            player.Skills.Sword = (byte)databasePlayer.SkillSword;

            player.Skills.SwordPercent = (byte)databasePlayer.SkillSwordPercent;

            player.Skills.Axe = (byte)databasePlayer.SkillAxe;

            player.Skills.AxePercent = (byte)databasePlayer.SkillAxePercent;

            player.Skills.Distance = (byte)databasePlayer.SkillDistance;

            player.Skills.DistancePercent = (byte)databasePlayer.SkillDistancePercent;

            player.Skills.Shield = (byte)databasePlayer.SkillShield;

            player.Skills.ShieldPercent = (byte)databasePlayer.SkillShieldPercent;

            player.Skills.Fish = (byte)databasePlayer.SkillFish;

            player.Skills.FishPercent = (byte)databasePlayer.SkillFishPercent;

            player.Experience = (uint)databasePlayer.Experience;

            player.Level = (ushort)databasePlayer.Level;

            player.LevelPercent = (byte)databasePlayer.LevelPercent;

            player.Mana = (ushort)databasePlayer.Mana;

            player.MaxMana = (ushort)databasePlayer.MaxMana;

            player.Soul = (byte)databasePlayer.Soul;

            player.Capacity = (uint)databasePlayer.Capacity;

            player.Stamina = (ushort)databasePlayer.Stamina;

            player.Gender = (Gender)databasePlayer.Gender;

            player.Vocation = (Vocation)databasePlayer.Vocation;

            server.GameObjects.AddGameObject(player);

            server.Components.AddComponent(player, new SpecialConditionBehaviour() );

            server.Components.AddComponent(player, new CooldownBehaviour() );

            server.Components.AddComponent(player, new AttackAndFollowBehaviour(new CloseAttackStrategy(500, (attacker, target) => -server.Randomization.Take(0, 20) ), new FollowWalkStrategy() ) );

            return player;
        }

        public void Destroy(Player player)
        {
            server.CancelQueueForExecution(Constants.PlayerWalkSchedulerEvent(player) );

            server.CancelQueueForExecution(Constants.PlayerAutomationSchedulerEvent(player) );

            server.GameObjects.RemoveGameObject(player);

            server.Components.ClearComponents(player);
        }
    }
}