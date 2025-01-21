using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;

namespace OpenTibia.Game.Components
{
    public enum AttackType
    {
        EnergyField,

        FireField,

        PoisonField
    }

    public enum MinMaxAttackType
    {
        Bolts,

        BoulderThrow,

        EneryBeam,

        FireWave,

        GreatFireball,

        ManaDrain,

        Melee,

        SelfHealing,

        Stalagmite,

        ThrowsKnives,

        ThrowsSpears
    }

    public static class AttackStrategyFactory
    {
        public static IAttackStrategy Create(AttackType type)
        {
            switch (type)
            {
                case AttackType.EnergyField:

                    return new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1504, 1, DamageType.Energy, 20, 20, 
                        
                        new DamageCondition(SpecialCondition.Electrified, null, DamageType.Energy, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) );

                case AttackType.FireField:

                    return new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Fire, MagicEffectType.FireDamage, 1492, 1, DamageType.Fire, 20, 20, 
                        
                        new DamageCondition(SpecialCondition.Burning, null, DamageType.Fire, new[] { 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(4) ) );

                case AttackType.PoisonField:

                    return new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Poison, MagicEffectType.GreenRings, 1503, 1, DamageType.Earth, 5, 5, 
                        
                        new DamageCondition(SpecialCondition.Poisoned, null, DamageType.Earth, new[] { 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) );
            }

            throw new NotImplementedException();
        }

        public static IAttackStrategy Create(MinMaxAttackType type, int min, int max)
        {
            switch (type)
            {
                case MinMaxAttackType.Bolts:

                    return new DistanceAttackStrategy(ProjectileType.Bolt, null, DamageType.Physical, min, max);

                case MinMaxAttackType.BoulderThrow:

                    return new DistanceAttackStrategy(ProjectileType.BigStone, null, DamageType.Physical, min, max);

                case MinMaxAttackType.EneryBeam:

                    return new SpellBeamAttackStrategy(Offset.Beam7, MagicEffectType.EnergyArea, DamageType.Energy, min, max);                    

                case MinMaxAttackType.FireWave:

                    return new SpellBeamAttackStrategy(Offset.Wave1133355, MagicEffectType.FireArea, DamageType.Fire, min, max);

                case MinMaxAttackType.GreatFireball:

                    return new RuneAreaAttackStrategy(Offset.Circle5, ProjectileType.Fire, MagicEffectType.FireArea, DamageType.Fire, min, max);

                case MinMaxAttackType.ManaDrain:

                    return new RuneTargetSimpleAttackStrategy(null, null, DamageType.ManaDrain, min, max);

                case MinMaxAttackType.Melee:

                    return new MeleeAttackStrategy(null, DamageType.Physical, min, max);

                case MinMaxAttackType.SelfHealing:

                    return new SpellHealingAttackStrategy(min, max);

                case MinMaxAttackType.Stalagmite:

                    return new RuneTargetSimpleAttackStrategy(ProjectileType.Poison, null, DamageType.Earth, min, max);

                case MinMaxAttackType.ThrowsKnives:

                    return new DistanceAttackStrategy(ProjectileType.ThrowingKnife, null, DamageType.Physical, min, max);

                case MinMaxAttackType.ThrowsSpears:

                    return new DistanceAttackStrategy(ProjectileType.Spear, null, DamageType.Physical, min, max);
            }

            throw new NotImplementedException();
        }      
    }
}