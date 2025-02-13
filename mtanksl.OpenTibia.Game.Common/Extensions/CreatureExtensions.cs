using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
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

        public static Promise AttackArea(this Creature creature, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Attack attack, Condition condition)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureAttackAreaCommand(creature, beam, center, area, projectileType, magicEffectType, attack, condition) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AttackArea(this Creature creature, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureAttackAreaCommand(creature, beam, center, area, projectileType, magicEffectType, openTibiaId, count) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AttackArea(this Creature creature, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, Attack attack)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureAttackAreaCommand(creature, beam, center, area, projectileType, magicEffectType, openTibiaId, count, attack) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise AttackArea(this Creature creature, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte count, Attack attack, Condition condition)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureAttackAreaCommand(creature, beam, center, area, projectileType, magicEffectType, openTibiaId, count, attack, condition) );
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

        public static Promise AttackCreature(this Creature creature, Creature target, Attack attack, Condition condition)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureAttackCreatureCommand(creature, target, attack, condition) );
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

        public static Promise Destroy(this Creature creature)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureDestroyCommand(creature) );
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

        public static Promise UpdateHealth(this Creature creature, int health)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateHealthCommand(creature, health) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateLight(this Creature creature, Light conditionLight)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateLightCommand(creature, conditionLight) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateOutfit(this Creature creature, Outfit baseOutfit, Outfit conditionOutfit, bool swimming, bool stealth)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateOutfitCommand(creature, baseOutfit, conditionOutfit, swimming, stealth) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise UpdateSpeed(this Creature creature, int? conditionSpeed)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureUpdateSpeedCommand(creature, conditionSpeed) );
        }

        /// <exception cref="InvalidOperationException"></exception>

        public static Promise Move(this Creature creature, Tile toTile)
        {
            Context context = Context.Current;

            if (context == null)
            {
                throw new InvalidOperationException("Context not found.");
            }

            return context.AddCommand(new CreatureMoveCommand(creature, toTile) );
        }
    }
}