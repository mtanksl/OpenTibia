using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbPlayer
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

        public int BaseOutfitItemId { get; set; }

        public int BaseOutfitId { get; set; }

        public int BaseOutfitHead { get; set; }

        public int BaseOutfitBody { get; set; }

        public int BaseOutfitLegs { get; set; }

        public int BaseOutfitFeet { get; set; }

        public int BaseOutfitAddon { get; set; }

        public int OutfitItemId { get; set; }

        public int OutfitId { get; set; }

        public int OutfitHead { get; set; }

        public int OutfitBody { get; set; }

        public int OutfitLegs { get; set; }

        public int OutfitFeet { get; set; }

        public int OutfitAddon { get; set; }

        public int BaseSpeed { get; set; }

        public int Speed { get; set; }

        public bool Invisible { get; set; }

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

        public long Experience { get; set; }

        public int Level { get; set; }

        public int LevelPercent { get; set; }

        public int Mana { get; set; }

        public int MaxMana { get; set; }

        public int Soul { get; set; }

        public int Capacity { get; set; }

        public int Stamina { get; set; }

        public int Gender { get; set; }

        public int Vocation { get; set; }

        public int Rank { get; set; }

        public int SpawnX { get; set; }

        public int SpawnY { get; set; }

        public int SpawnZ { get; set; }

        public int TownX { get; set; }

        public int TownY { get; set; }

        public int TownZ { get; set; }


        public DbAccount Account { get; set; }

        public DbWorld World { get; set; }

        public ICollection<DbPlayerDepotItem> PlayerDepotItems { get; set; } = new List<DbPlayerDepotItem>();

        public ICollection<DbPlayerItem> PlayerItems { get; set; } = new List<DbPlayerItem>();

        public ICollection<DbPlayerStorage> PlayerStorages { get; set; } = new List<DbPlayerStorage>();

        public ICollection<DbPlayerSpell> PlayerSpells { get; set; } = new List<DbPlayerSpell>();

        public ICollection<DbPlayerAchievement> PlayerAchievements { get; set; } = new List<DbPlayerAchievement>();

        public ICollection<DbPlayerOutfit> PlayerOutfits { get; set; } = new List<DbPlayerOutfit>();

        public ICollection<DbPlayerVip> PlayerVips { get; set; } = new List<DbPlayerVip>();

        public ICollection<DbHouse> Houses { get; set; } = new List<DbHouse>();
    }
}