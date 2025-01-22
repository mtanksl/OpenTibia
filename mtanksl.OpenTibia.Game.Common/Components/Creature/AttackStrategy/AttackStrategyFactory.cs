using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public enum AttackType
    {
        EnergyField,

        FireField,

        PoisonField,


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
        private static List<(string Name, AttackType Type, Func<int, int, IAttackStrategy> AttackStrategy)> items = new List<(string Name, AttackType Type, Func<int, int, IAttackStrategy> AttackStrategy)>()
        {
            ("EnergyField", AttackType.EnergyField, (min, max) => new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1504, 1, DamageType.Energy, 20, 20, new DamageCondition(SpecialCondition.Electrified, null, DamageType.Energy, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) ) ),

            ("FireField", AttackType.FireField, (min, max) => new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Fire, MagicEffectType.FireDamage, 1492, 1, DamageType.Fire, 20, 20, new DamageCondition(SpecialCondition.Burning, null, DamageType.Fire, new[] { 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(4) ) ) ),

            ("PoisonField", AttackType.PoisonField, (min, max) => new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Poison, MagicEffectType.GreenRings, 1503, 1, DamageType.Earth, 5, 5, new DamageCondition(SpecialCondition.Poisoned, null, DamageType.Earth, new[] { 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) ) ),


            ("Bolts", AttackType.Bolts, (min, max) => new DistanceAttackStrategy(ProjectileType.Bolt, null, DamageType.Physical, min, max) ),

            ("BoulderThrow", AttackType.BoulderThrow, (min, max) => new DistanceAttackStrategy(ProjectileType.BigStone, null, DamageType.Physical, min, max) ),

            ("EneryBeam", AttackType.EneryBeam, (min, max) => new SpellBeamAttackStrategy(Offset.Beam7, MagicEffectType.EnergyArea, DamageType.Energy, min, max) ),

            ("FireWave", AttackType.FireWave, (min, max) => new SpellBeamAttackStrategy(Offset.Wave1133355, MagicEffectType.FireArea, DamageType.Fire, min, max) ),

            ("GreatFireball", AttackType.GreatFireball, (min, max) => new RuneAreaAttackStrategy(Offset.Circle5, ProjectileType.Fire, MagicEffectType.FireArea, DamageType.Fire, min, max) ),

            ("ManaDrain", AttackType.ManaDrain, (min, max) => new RuneTargetSimpleAttackStrategy(null, null, DamageType.ManaDrain, min, max) ),

            ("Melee", AttackType.Melee, (min, max) => new MeleeAttackStrategy(null, DamageType.Physical, min, max) ),

            ("SelfHealing", AttackType.SelfHealing, (min, max) => new SpellHealingAttackStrategy(min, max) ),

            ("Stalagmite", AttackType.Stalagmite, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.Poison, null, DamageType.Earth, min, max) ),

            ("ThrowsKnives", AttackType.ThrowsKnives, (min, max) => new DistanceAttackStrategy(ProjectileType.ThrowingKnife, null, DamageType.Physical, min, max) ),

            ("ThrowsSpears", AttackType.ThrowsSpears, (min, max) => new DistanceAttackStrategy(ProjectileType.Spear, null, DamageType.Physical, min, max) )
        };

        public static IAttackStrategy Create(string name, int min = 0, int max = 0)
        {
            foreach (var item in items)
            {
                if (item.Name == name)
                {
                    return item.AttackStrategy(min, max);
                }
            }

            return null;
        }

        public static IAttackStrategy Create(AttackType type, int min = 0, int max = 0)
        {
            foreach (var item in items)
            {
                if (item.Type == type)
                {
                    return item.AttackStrategy(min, max);
                }
            }

            return null;
        }
    }
}