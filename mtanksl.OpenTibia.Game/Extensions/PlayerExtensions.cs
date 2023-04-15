using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class PlayerExtensions
    {
        public static Promise Destroy(this Player player)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerDestroyCommand(player) );
        }

        public static Promise Say(this Player player, string message)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerSayCommand(player, message) );
        }

        public static Promise UpdateCapacity(this Player player, int capacity)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateCapacityCommand(player, capacity) );
        }

        public static Promise UpdateExperiente(this Player player, uint experience, ushort level, byte levelPercent)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateExperienteCommand(player, experience, level, levelPercent) );
        }

        public static Promise UpdateMana(this Player player, int mana, int maxMana)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateManaCommand(player, mana, maxMana) );
        }

        public static Promise UpdateSoul(this Player player, int soul)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateSoulCommand(player, soul) );
        }

        public static Promise UpdateStamina(this Player player, int stamina)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateStaminaCommand(player, stamina) );
        }

        public static Promise Whisper(this Player player, string message)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerWhisperCommand(player, message) );
        }

        public static Promise Yell(this Player player, string message)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerYellCommand(player, message) );
        }
    }
}