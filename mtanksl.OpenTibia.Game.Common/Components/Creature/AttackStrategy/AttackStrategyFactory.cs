using OpenTibia.Common.Structures;
using OpenTibia.Game.Commands;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public enum AttackType
    {
        Arrows,
        Bolts,
        BoulderThrow,
        BurstArrows,
        Knives,
        Haste,
        LifeDrain,
        ManaDrain,
        Melee,
        SelfHealing,
        SmallStones,
        Spears,
        Stars,

        // Runes
        Avalanche,
        EnergyBomb,
        EnergyField,
        Explosion,
        FireBomb,
        FireField,
        Fireball,
        GreatFireball,
        HeavyMagicMissile,
        HolyMissile,
        Icicle,
        LightMagicMissile,
        Paralyse,
        PoisonBomb,
        PoisonField,
        SoulFire,
        Stalagmite,
        StoneShower,
        SuddenDeath,
        Thunderstorm,

        // Spells
        Berserk,
        DeathStrike,
        DivineCaldera,
        DivineMissile,
        EnergyBeam,
        EnergyStrike,
        EnergyWave,
        EternalWinter,
        EtherealSpear,
        FierceBerserk,
        FireWave,
        FlameStrike,
        Groundshaker,
        GreatEnergyBeam,
        HellsCore,
        IceStrike,
        IceWave,
        RageOfTheSkies,
        TerraStrike,
        TerraWave,
        WhirlwindThrow,
        WrathOfNature
    }

    public static class AttackStrategyFactory
    {
        private static List<(string Name, AttackType Type, Func<int, int, IAttackStrategy> AttackStrategy)> items = new List<(string Name, AttackType Type, Func<int, int, IAttackStrategy> AttackStrategy)>()
        {
            ("arrows", AttackType.Arrows, (min, max) => new DistanceAttackStrategy(ProjectileType.Arrow, null, DamageType.Physical, min, max) ),
            ("bolts", AttackType.Bolts, (min, max) => new DistanceAttackStrategy(ProjectileType.Bolt, null, DamageType.Physical, min, max) ),
            ("boulder throw", AttackType.BoulderThrow, (min, max) => new DistanceAttackStrategy(ProjectileType.BigStone, null, DamageType.Physical, min, max) ),
            ("burst arrows", AttackType.BurstArrows, (min, max) => new RuneAreaAttackStrategy(Offset.Square3, ProjectileType.Fire, MagicEffectType.FireArea, DamageType.Fire, min, max) ),
            ("haste", AttackType.Haste, (min, max) => new SpellHasteAttackStrategy() ),
            ("knives", AttackType.Knives, (min, max) => new DistanceAttackStrategy(ProjectileType.ThrowingKnife, null, DamageType.Physical, min, max) ),
            ("life drain", AttackType.LifeDrain, (min, max) => new RuneTargetSimpleAttackStrategy(null, null, DamageType.LifeDrain, min, max) ),
            ("mana drain", AttackType.ManaDrain, (min, max) => new RuneTargetSimpleAttackStrategy(null, null, DamageType.ManaDrain, min, max) ),
            ("melee", AttackType.Melee, (min, max) => new MeleeAttackStrategy(null, DamageType.Physical, min, max) ),
            ("self healing", AttackType.SelfHealing, (min, max) => new SpellHealingAttackStrategy(min, max) ),
            ("small stones", AttackType.SmallStones, (min, max) => new DistanceAttackStrategy(ProjectileType.SmallStone, null, DamageType.Physical, min, max) ),
            ("spears", AttackType.Spears, (min, max) => new DistanceAttackStrategy(ProjectileType.Spear, null, DamageType.Physical, min, max) ),
            ("stars", AttackType.Stars, (min, max) => new DistanceAttackStrategy(ProjectileType.ThrowingStar, null, DamageType.Physical, min, max) ),

            // Runes
            ("avalanche", AttackType.Avalanche, (min, max) => new RuneAreaAttackStrategy(Offset.Circle7, ProjectileType.Ice, MagicEffectType.IceArea, DamageType.Ice, min, max) ),
            ("energy bomb", AttackType.EnergyBomb, (min, max) => new RuneAreaAttackStrategy(Offset.Square3, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, DamageType.Energy, 30, 30, new DamageCondition(SpecialCondition.Electrified, null, DamageType.Energy, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) ) ),
            ("energy field", AttackType.EnergyField, (min, max) => new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Energy, MagicEffectType.EnergyDamage, 1495, 1, DamageType.Energy, 30, 30, new DamageCondition(SpecialCondition.Electrified, null, DamageType.Energy, new[] { 25, 25 }, TimeSpan.FromSeconds(4) ) ) ),
            ("explosion", AttackType.Explosion, (min, max) => new RuneAreaAttackStrategy(Offset.Circle3, ProjectileType.Explosion, MagicEffectType.ExplosionArea, DamageType.Physical, min, max) ),
            ("fire bomb", AttackType.FireBomb, (min, max) => new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Fire, MagicEffectType.FireDamage, 1492, 1, DamageType.Fire, 20, 20, new DamageCondition(SpecialCondition.Burning, null, DamageType.Fire, new[] { 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(4) ) ) ),
            ("fire field", AttackType.FireField, (min, max) => new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Fire, MagicEffectType.FireDamage, 1492, 1, DamageType.Fire, 20, 20, new DamageCondition(SpecialCondition.Burning, null, DamageType.Fire, new[] { 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(4) ) ) ),
            ("fireball", AttackType.Fireball, (min, max) => new RuneAreaAttackStrategy(Offset.Circle5, ProjectileType.Fire, MagicEffectType.FireArea, DamageType.Fire, min, max) ),
            ("great fireball", AttackType.GreatFireball, (min, max) => new RuneAreaAttackStrategy(Offset.Circle7, ProjectileType.Fire, MagicEffectType.FireArea, DamageType.Fire, min, max) ),
            ("heavy magic missile", AttackType.HeavyMagicMissile, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.EnergySmall, null, DamageType.Energy, min, max) ),
            ("holy missile", AttackType.HolyMissile, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.Holy, null, DamageType.Holy, min, max) ),
            ("icicle", AttackType.Icicle, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.Ice, MagicEffectType.IceArea, DamageType.Ice, min, max) ),
            ("light magic missile", AttackType.LightMagicMissile, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.EnergySmall, null, DamageType.Energy, min, max) ),
            ("paralyse", AttackType.Paralyse, (min, max) => new RuneParalyseAttackStrategy() ),
            ("poison bomb", AttackType.PoisonBomb, (min, max) => new RuneAreaAttackStrategy(Offset.Square3, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, DamageType.Earth, 5, 5, new DamageCondition(SpecialCondition.Poisoned, null, DamageType.Earth, new[] { 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) ) ),
            ("poison field", AttackType.PoisonField, (min, max) => new RuneAreaAttackStrategy(Offset.Square1, ProjectileType.Poison, MagicEffectType.GreenRings, 1496, 1, DamageType.Earth, 5, 5, new DamageCondition(SpecialCondition.Poisoned, null, DamageType.Earth, new[] { 5, 5, 5, 5, 4, 4, 4, 4, 4, 3, 3, 3, 3, 3, 3, 3, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1 }, TimeSpan.FromSeconds(4) ) ) ),
            ("soulfire", AttackType.SoulFire, (min, max) => new RuneTargetSimpleAttackStrategy(null, null, DamageType.Fire, 10, 10, new DamageCondition(SpecialCondition.Burning, null, DamageType.Fire, new[] { 10, 10, 10, 10, 10, 10, 10, 10, 10, 10 }, TimeSpan.FromSeconds(9) ) ) ),
            ("stalagmite", AttackType.Stalagmite, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.Poison, null, DamageType.Earth, min, max) ),
            ("stone shower", AttackType.StoneShower, (min, max) => new RuneAreaAttackStrategy(Offset.Circle7, ProjectileType.SmallStone, MagicEffectType.Stones, DamageType.Earth, min, max) ),
            ("sudden death", AttackType.SuddenDeath, (min, max) =>  new RuneTargetSimpleAttackStrategy(ProjectileType.SuddenDeath, MagicEffectType.MortArea, DamageType.Death, min, max) ),
            ("thunderstorm", AttackType.Thunderstorm, (min, max) => new RuneAreaAttackStrategy(Offset.Circle7, ProjectileType.Energy, MagicEffectType.EnergyDamage, DamageType.Energy, min, max) ),

            // Spells
            ("berserk", AttackType.Berserk, (min, max) => new SpellAreaAttackStrategy(Offset.Square3, MagicEffectType.BlackSpark, DamageType.Physical, min, max) ),
            ("death strike", AttackType.DeathStrike, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.SuddenDeath, MagicEffectType.MortArea, DamageType.Death, min, max) ),
            ("divine caldera", AttackType.DivineCaldera, (min, max) => new SpellAreaAttackStrategy(Offset.Circle7, MagicEffectType.HolyDamage, DamageType.Holy, min, max) ),
            ("divine missile", AttackType.DivineMissile, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.HolySmall, MagicEffectType.HolyDamage, DamageType.Holy, min, max) ),
            ("energy beam", AttackType.EnergyBeam, (min, max) => new SpellBeamAttackStrategy(Offset.Beam5, MagicEffectType.EnergyArea, DamageType.Energy, min, max) ),
            ("energy strike", AttackType.EnergyStrike, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.EnergySmall, MagicEffectType.EnergyArea, DamageType.Energy, min, max) ),
            ("energy wave", AttackType.EnergyWave, (min, max) => new SpellBeamAttackStrategy(Offset.Wave11333, MagicEffectType.EnergyArea, DamageType.Energy, min, max) ),
            ("eternal winter", AttackType.EternalWinter, (min, max) => new SpellAreaAttackStrategy(Offset.Circle11, MagicEffectType.IceTornado, DamageType.Ice, min, max) ),
            ("ethereal spear", AttackType.EtherealSpear, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.EthernalSpear, MagicEffectType.GroundShaker, DamageType.Physical, min, max) ),
            ("fierce berserk", AttackType.FierceBerserk, (min, max) => new SpellAreaAttackStrategy(Offset.Square3, MagicEffectType.BlackSpark, DamageType.Physical, min, max) ),
            ("fire wave", AttackType.FireWave, (min, max) => new SpellBeamAttackStrategy(Offset.Wave11333, MagicEffectType.FireArea, DamageType.Fire, min, max) ),
            ("flame strike", AttackType.FlameStrike, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.Fire, MagicEffectType.FireDamage, DamageType.Fire, min, max) ),
            ("groundshaker", AttackType.Groundshaker, (min, max) => new SpellAreaAttackStrategy(Offset.Circle7, MagicEffectType.GroundShaker, DamageType.Physical, min, max) ),
            ("great energy beam", AttackType.GreatEnergyBeam, (min, max) => new SpellBeamAttackStrategy(Offset.Beam7, MagicEffectType.EnergyArea, DamageType.Energy, min, max) ),
            ("hells core", AttackType.HellsCore, (min, max) => new SpellAreaAttackStrategy(Offset.Circle11, MagicEffectType.FireArea, DamageType.Fire, min, max) ),
            ("ice strike", AttackType.IceStrike, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.IceSmall, MagicEffectType.IceDamage, DamageType.Ice, min, max) ),
            ("ice wave", AttackType.IceWave, (min, max) => new SpellBeamAttackStrategy(Offset.Wave1335, MagicEffectType.EnergyArea, DamageType.Energy, min, max) ),
            ("rage of the skies", AttackType.RageOfTheSkies, (min, max) => new SpellAreaAttackStrategy(Offset.Circle11, MagicEffectType.BigClouds, DamageType.Energy, min, max) ),
            ("terra strike", AttackType.TerraStrike, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.Poison, MagicEffectType.Carniphilia, DamageType.Earth, min, max) ),
            ("terra wave", AttackType.TerraWave, (min, max) => new SpellBeamAttackStrategy(Offset.Wave11333, MagicEffectType.PlantAttack, DamageType.Earth, min, max) ),
            ("whirlwind throw", AttackType.WhirlwindThrow, (min, max) => new RuneTargetSimpleAttackStrategy(ProjectileType.WhirlWindSword, MagicEffectType.GroundShaker, DamageType.Physical, min, max) ),
            ("wrath of nature", AttackType.WrathOfNature, (min, max) => new SpellAreaAttackStrategy(Offset.Circle11, MagicEffectType.PlantAttack, DamageType.Earth, min, max) )
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