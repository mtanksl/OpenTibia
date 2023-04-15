using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class CreatureExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateDirection(this Creature creature, Direction direction)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateDirectionCommand(creature, direction) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateHealth(this Creature creature, int health, int maxHealth)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateHealthCommand(creature, health, maxHealth) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateLight(this Creature creature, Light light)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateLightCommand(creature, light) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateOutfit(this Creature creature, Outfit outfit)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateOutfit(creature, outfit) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateParent(this Creature creature, Tile toTile)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateParentCommand(creature, toTile) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdatePartyIcon(this Creature creature, PartyIcon partyIcon)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdatePartyIconCommand(creature, partyIcon) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateSkullIcon(this Creature creature, SkullIcon skullIcon)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateSkullIconCommand(creature, skullIcon) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateSpeed(this Creature creature, ushort speed)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateSpeedCommand(creature, speed) );
        }
    }
}