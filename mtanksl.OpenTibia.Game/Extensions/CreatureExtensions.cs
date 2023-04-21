using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Extensions
{
    public static class CreatureExtensions
    {
        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AddCondition(this Creature creature, Condition condition)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureAddConditionCommand(creature, condition) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AttackArea(this Creature creature, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Attack attack)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureAttackAreaCommand(creature, beam, center, area, projectileType, magicEffectType, attack) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AttackCreature(this Creature creature, Creature target, Attack attack)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureAttackCreatureCommand(creature, target, attack) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise RemoveCondition(this Creature creature, ConditionSpecialCondition conditionSpecialCondition)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureRemoveConditionCommand(creature, conditionSpecialCondition) );
        }

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

        public static Promise UpdateOutfit(this Creature creature, Outfit baseOutfit, Outfit outfit)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateOutfitCommand(creature, baseOutfit, outfit) );
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

        public static Promise UpdateSpeed(this Creature creature, ushort baseSpeed, ushort speed)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateSpeedCommand(creature, baseSpeed, speed) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateTile(this Creature creature, Tile toTile)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateTileCommand(creature, toTile) );
        }
    }
}