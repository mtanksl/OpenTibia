﻿using System.Collections.Generic;
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

        public int BaseOutfitMount { get; set; }

        public int BaseSpeed { get; set; }

        public bool Invisible { get; set; }

        public int SkillMagicLevel { get; set; }

        public long SkillMagicLevelPoints { get; set; }

        public int SkillFist { get; set; }

        public long SkillFistPoints { get; set; }

        public int SkillClub { get; set; }

        public long SkillClubPoints { get; set; }

        public int SkillSword { get; set; }

        public long SkillSwordPoints { get; set; }

        public int SkillAxe { get; set; }

        public long SkillAxePoints { get; set; }

        public int SkillDistance { get; set; }

        public long SkillDistancePoints { get; set; }

        public int SkillShield { get; set; }

        public long SkillShieldPoints { get; set; }

        public int SkillFish { get; set; }

        public long SkillFishPoints { get; set; }

        public long Experience { get; set; }

        public int Level { get; set; }

        public int Mana { get; set; }

        public int MaxMana { get; set; }

        public int Soul { get; set; }

        public int Capacity { get; set; }

        public int MaxCapacity { get; set; }

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

        public long BankAccount { get; set; }


        public DbAccount Account { get; set; }

        public DbWorld World { get; set; }

        public ICollection<DbPlayerDepotItem> PlayerDepotItems { get; set; } = new List<DbPlayerDepotItem>();

        public ICollection<DbPlayerItem> PlayerItems { get; set; } = new List<DbPlayerItem>();

        public ICollection<DbPlayerStorage> PlayerStorages { get; set; } = new List<DbPlayerStorage>();

        public ICollection<DbPlayerSpell> PlayerSpells { get; set; } = new List<DbPlayerSpell>();

        public ICollection<DbPlayerBless> PlayerBlesses { get; set; } = new List<DbPlayerBless>();

        public ICollection<DbPlayerAchievement> PlayerAchievements { get; set; } = new List<DbPlayerAchievement>();

        public ICollection<DbPlayerOutfit> PlayerOutfits { get; set; } = new List<DbPlayerOutfit>();

        public ICollection<DbPlayerVip> PlayerVips { get; set; } = new List<DbPlayerVip>();

        public ICollection<DbPlayerKill> PlayerKills { get; set; } = new List<DbPlayerKill>();

        public ICollection<DbPlayerDeath> PlayerDeaths { get; set; } = new List<DbPlayerDeath>();

        public ICollection<DbHouse> Houses { get; set; } = new List<DbHouse>();
    }
}