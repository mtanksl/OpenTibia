using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;

namespace OpenTibia.Game.Extensions
{
    public static class CreatureExtensions
    {
        public static Promise UpdateDirection(this Creature creature, Direction direction)
        {
            Context context = Context.Current;

            return context.AddCommand(new CreatureUpdateDirectionCommand(creature, direction) );
        }

        public static Promise UpdateHealth(this Creature creature, ushort health, ushort maxHealth)
        {
            Context context = Context.Current;

            return context.AddCommand(new CreatureUpdateHealthCommand(creature, health, maxHealth) );
        }

        public static Promise UpdateLight(this Creature creature, Light light)
        {
            Context context = Context.Current;

            return context.AddCommand(new CreatureUpdateLightCommand(creature, light) );
        }

        public static Promise UpdateOutfit(this Creature creature, Outfit outfit)
        {
            Context context = Context.Current;

            return context.AddCommand(new CreatureUpdateOutfit(creature, outfit) );
        }

        public static Promise UpdateParent(this Creature creature, Tile toTile)
        {
            Context context = Context.Current;

            return context.AddCommand(new CreatureUpdateParentCommand(creature, toTile) );
        }

        public static Promise UpdatePartyIcon(this Creature creature, PartyIcon partyIcon)
        {
            Context context = Context.Current;

            return context.AddCommand(new CreatureUpdatePartyIconCommand(creature, partyIcon) );
        }

        public static Promise UpdateSkullIcon(this Creature creature, SkullIcon skullIcon)
        {
            Context context = Context.Current;

            return context.AddCommand(new CreatureUpdateSkullIconCommand(creature, skullIcon) );
        }

        public static Promise UpdateSpeed(this Creature creature, ushort speed)
        {
            Context context = Context.Current;

            return context.AddCommand(new CreatureUpdateSpeedCommand(creature, speed) );
        }
    }
}