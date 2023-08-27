using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class PlayerExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateAxe(this Player player, byte axe, byte axePercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateAxeCommand(player, axe, axePercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateClub(this Player player, byte club, byte clubPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateClubCommand(player, club, clubPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateDistance(this Player player, byte distance, byte distancePercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateDistanceCommand(player, distance, distancePercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateFish(this Player player, byte fish, byte fishPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateFishCommand(player, fish, fishPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateFist(this Player player, byte fist, byte fistPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateFistCommand(player, fist, fistPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateMagicLevel(this Player player, byte magicLevel, byte magicLevelPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateMagicLevelCommand(player, magicLevel, magicLevelPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateShield(this Player player, byte shield, byte shieldPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateShieldCommand(player, shield, shieldPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateSword(this Player player, byte sword, byte swordPercent)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateSwordCommand(player, sword, swordPercent) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Destroy(this Player player)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerDestroyCommand(player) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise InventoryContainerTileCreateItem(this Player player, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerInventoryContainerTileCreateItemCommand(player, openTibiaId, count) );
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

        public static Promise UpdateCapacity(this Player player, int capacity)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new PlayerUpdateCapacityCommand(player, capacity) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateExperiente(this Player player, uint experience, ushort level, byte levelPercent)
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