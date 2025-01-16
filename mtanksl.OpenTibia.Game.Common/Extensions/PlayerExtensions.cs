using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class PlayerExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise CreateMoney(this Player player, int price)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerCreateMoneyCommand(player, price) );
        }

        /// <exception cref="InvalidOperationException"></exception>
    
        public static PromiseResult<bool> DestroyMoney(this Player player, int price)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerDestroyMoneyCommand(player, price) );
        }

        /// <exception cref="InvalidOperationException"></exception>
   
        public static PromiseResult<int> CountMoney(this Player player)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerCountMoneyCommand(player) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise CreateItems(this Player player, ushort openTibiaId, byte type, int count)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerCreateItemsCommand(player, openTibiaId, type, count) );
        }

        /// <exception cref="InvalidOperationException"></exception>
        
        public static PromiseResult<bool> DestroyItems(this Player player, ushort openTibiaId, byte type, int count)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerDestroyItemsCommand(player, openTibiaId, type, count) );
        }

        /// <exception cref="InvalidOperationException"></exception>
       
        public static PromiseResult<int> CountItems(this Player player, ushort openTibiaId, byte type)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerCountItemsCommand(player, openTibiaId, type) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Achievement(this Player player, int incrementStorageKey, int requiredStorageValue, string achievementName)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerAchievementCommand(player, incrementStorageKey, requiredStorageValue, achievementName) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Achievement(this Player player, int incrementStorageKey, int[] requiredStorageKeys, string achievementName)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerAchievementCommand(player, incrementStorageKey, requiredStorageKeys, achievementName) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AddExperience(this Player player, ulong experience)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerAddExperienceCommand(player, experience) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AddSkillPoints(this Player player, Skill skill, ulong skillPoints)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerAddSkillPointsCommand(player, skill, skillPoints) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Bless(this Player player, string message, string blessName)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerBlessCommand(player, message, blessName) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateAxe(this Player player, ulong axePoints, byte axe, byte axePercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateAxeCommand(player, axePoints, axe, axePercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateClub(this Player player, ulong clubPoints, byte club, byte clubPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateClubCommand(player, clubPoints, club, clubPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateDistance(this Player player, ulong distancePoints, byte distance, byte distancePercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateDistanceCommand(player, distancePoints, distance, distancePercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateFish(this Player player, ulong fishPoints, byte fish, byte fishPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateFishCommand(player, fishPoints, fish, fishPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateFist(this Player player, ulong fistPoints, byte fist, byte fistPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateFistCommand(player, fistPoints, fist, fistPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateMagicLevel(this Player player, ulong magicLevelPoints, byte magicLevel, byte magicLevelPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateMagicLevelCommand(player, magicLevelPoints, magicLevel, magicLevelPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateShield(this Player player, ulong shieldPoints, byte shield, byte shieldPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateShieldCommand(player, shieldPoints, shield, shieldPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateSword(this Player player, ulong swordPoints, byte sword, byte swordPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateSwordCommand(player, swordPoints, sword, swordPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Say(this Player player, string message)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerSayCommand(player, message) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateCapacity(this Player player, uint capacity)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateCapacityCommand(player, capacity) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateExperience(this Player player, ulong experience, ushort level, byte levelPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateExperienceCommand(player, experience, level, levelPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateMana(this Player player, int mana, int maxMana)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateManaCommand(player, mana, maxMana) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateSoul(this Player player, int soul)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateSoulCommand(player, soul) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateStamina(this Player player, int stamina)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateStaminaCommand(player, stamina) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Whisper(this Player player, string message)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerWhisperCommand(player, message) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Yell(this Player player, string message)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerYellCommand(player, message) );
        }
    }
}