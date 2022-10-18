using OpenTibia.Common.Objects;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class PlayerExtensions
    {
        public static Promise UpdateCapacity(this Player player, uint capacity)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateCapacityCommand(player, capacity) );
        }

        public static Promise UpdateExperiente(this Player player, uint experience, ushort level, byte levelPercent)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateExperienteCommand(player, experience, level, levelPercent) );
        }

        public static Promise UpdateMana(this Player player, ushort mana, ushort maxMana)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateManaCommand(player, mana, maxMana) );
        }

        public static Promise UpdateSoul(this Player player, byte soul)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateSoulCommand(player, soul) );
        }

        public static Promise UpdateStamina(this Player player, ushort stamina)
        {
            Context context = Context.Current;

            return context.AddCommand(new PlayerUpdateStaminaCommand(player, stamina) );
        }
    }
}