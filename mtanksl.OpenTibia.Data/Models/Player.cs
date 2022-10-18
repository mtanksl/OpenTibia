using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class Player
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int WorldId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        public int Health { get; set; }

        public int MaxHealth { get; set; }

        public int Direction { get; set; }

        public int OutfitItemId { get; set; }

        public int OutfitId { get; set; }

        public int OutfitHead { get; set; }

        public int OutfitBody { get; set; }

        public int OutfitLegs { get; set; }

        public int OutfitFeet { get; set; }

        public int OutfitAddon { get; set; }

        public int BaseSpeed { get; set; }

        public int Speed { get; set; }

        public int SkillMagicLevel { get; set; }

        public int SkillMagicLevelPercent { get; set; }

        public int SkillFist { get; set; }

        public int SkillFistPercent { get; set; }

        public int SkillClub { get; set; }

        public int SkillClubPercent { get; set; }

        public int SkillSword { get; set; }

        public int SkillSwordPercent { get; set; }

        public int SkillAxe { get; set; }

        public int SkillAxePercent { get; set; }

        public int SkillDistance { get; set; }

        public int SkillDistancePercent { get; set; }

        public int SkillShield { get; set; }

        public int SkillShieldPercent { get; set; }

        public int SkillFish { get; set; }

        public int SkillFishPercent { get; set; }

        public int Experience { get; set; }

        public int Level { get; set; }

        public int LevelPercent { get; set; }

        public int Mana { get; set; }

        public int MaxMana { get; set; }

        public int Soul { get; set; }

        public int Capacity { get; set; }

        public int Stamina { get; set; }

        public int CoordinateX { get; set; }

        public int CoordinateY { get; set; }

        public int CoordinateZ { get; set; }


        public Account Account { get; set; }

        public World World { get; set; }

        public ICollection<PlayerItem> PlayerItems { get; set; }

        public ICollection<PlayerDepotItem> PlayerDepotItems { get; set; }
    }
}