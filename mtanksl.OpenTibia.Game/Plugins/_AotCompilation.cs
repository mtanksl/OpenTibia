#if AOT
using OpenTibia.GameData.Plugins.Ammunitions;
using OpenTibia.GameData.Plugins.Runes;
using OpenTibia.GameData.Plugins.Spells;
using OpenTibia.GameData.Plugins.Weapons;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Plugins
{
    public static class _AotCompilation
    {
        public static readonly Dictionary<string, Func<Spell, SpellPlugin> > SpellPlugins = new Dictionary<string, Func<Spell, SpellPlugin> >()
        {
            { "OpenTibia.GameData.Plugins.Spells.BerserkSpellPlugin", spell => new BerserkSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.ConjureItemSpellPlugin", spell => new ConjureItemSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.ConjureRuneSpellPlugin", spell => new ConjureRuneSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.CurePoisonSpellPlugin", spell => new CurePoisonSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.DeathStrikeSpellPlugin", spell => new DeathStrikeSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.DivineHealingSpellPlugin", spell => new DivineHealingSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.EnergyBeamSpellPlugin", spell => new EnergyBeamSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.EnergyStrikeSpellPlugin", spell => new EnergyStrikeSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.EnergyWaveSpellPlugin", spell => new EnergyWaveSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.EternalWinterSpellPlugin", spell => new EternalWinterSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.FierceBerserkSpellPlugin", spell => new FierceBerserkSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.FireWaveSpellPlugin", spell => new FireWaveSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.FlameStrikeSpellPlugin", spell => new FlameStrikeSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.FoodSpellPlugin", spell => new FoodSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.GreatEnergyBeamSpellPlugin", spell => new GreatEnergyBeamSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.GreatLightSpellPlugin", spell => new GreatLightSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.GroundshakerSpellPlugin", spell => new GroundshakerSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.HasteSpellPlugin", spell => new HasteSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.HealFriendSpellPlugin", spell => new HealFriendSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.HellsCoreSpellPlugin", spell => new HellsCoreSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.IntenseHealingSpellPlugin", spell => new IntenseHealingSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.InvisibleSpellPlugin", spell => new InvisibleSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.LevitateDownSpellPlugin", spell => new LevitateDownSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.LevitateUpSpellPlugin", spell => new LevitateUpSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.LightHealingSpellPlugin", spell => new LightHealingSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.LightSpellPlugin", spell => new LightSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.MagicRopeSpellPlugin", spell => new MagicRopeSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.MagicShieldSpellPlugin", spell => new MagicShieldSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.MassHealingSpellPlugin", spell => new MassHealingSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.RageOfTheSkiesSpellPlugin", spell => new RageOfTheSkiesSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.StrongHasteSpellPlugin", spell => new StrongHasteSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.UltimateHealingSpellPlugin", spell => new UltimateHealingSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.UltimateLightSpellPlugin", spell => new UltimateLightSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.WoundCleansingSpellPlugin", spell => new WoundCleansingSpellPlugin(spell) },
            { "OpenTibia.GameData.Plugins.Spells.WrathOfNatureSpellPlugin", spell => new WrathOfNatureSpellPlugin(spell) },
        };

        public static readonly Dictionary<string, Func<Rune, RunePlugin> > RunePlugins = new Dictionary<string, Func<Rune, RunePlugin> >()
        {
            { "OpenTibia.GameData.Plugins.Runes.AvalancheRunePlugin", rune => new AvalancheRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.CurePoisonRunePlugin", rune => new CurePoisonRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.DestroyFieldRunePlugin", rune => new DestroyFieldRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.EnergyBombRunePlugin", rune => new EnergyBombRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.EnergyFieldRunePlugin", rune => new EnergyFieldRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.EnergyWallRunePlugin", rune => new EnergyWallRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.ExplosionRunePlugin", rune => new ExplosionRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.FireballRunePlugin", rune => new FireballRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.FireBombRunePlugin", rune => new FireBombRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.FireFieldRunePlugin", rune => new FireFieldRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.FireWallRunePlugin", rune => new FireWallRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.GreatFireballRunePlugin", rune => new GreatFireballRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.HeavyMagicMissileRunePlugin", rune => new HeavyMagicMissileRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.IcicleRunePlugin", rune => new IcicleRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.IntenseHealingRunePlugin", rune => new IntenseHealingRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.LightMagicMissileRunePlugin", rune => new LightMagicMissileRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.MagicWallRunePlugin", rune => new MagicWallRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.PoisonBombRunePlugin", rune => new PoisonBombRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.PoisonFieldRunePlugin", rune => new PoisonFieldRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.PoisonWallRunePlugin", rune => new PoisonWallRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.StalagmiteRunePlugin", rune => new StalagmiteRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.StoneShowerRunePlugin", rune => new StoneShowerRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.SuddenDeathRunePlugin", rune => new SuddenDeathRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.ThunderstormRunePlugin", rune => new ThunderstormRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.UltimateHealingRunePlugin", rune => new UltimateHealingRunePlugin(rune) },
            { "OpenTibia.GameData.Plugins.Runes.WildGrowthRunePlugin", rune => new WildGrowthRunePlugin(rune) },
        };

        public static readonly Dictionary<string, Func<Weapon, WeaponPlugin> > WeaponPlugins = new Dictionary<string, Func<Weapon, WeaponPlugin> >()
        {
            { "OpenTibia.GameData.Plugins.Weapons.MoonlightRodWeaponPlugin", weapon => new MoonlightRodWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.QuagmireRodWeaponPlugin", weapon => new QuagmireRodWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.SnakebiteRodWeaponPlugin", weapon => new SnakebiteRodWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.TempestRodWeaponPlugin", weapon => new TempestRodWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.ViperStarWeaponPlugin", weapon => new ViperStarWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.VolcanicRodWeaponPlugin", weapon => new VolcanicRodWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.WandOfCosmicEnergyWeaponPlugin", weapon => new WandOfCosmicEnergyWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.WandOfDragonbreathWeaponPlugin", weapon => new WandOfDragonbreathWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.WandOfInfernoWeaponPlugin", weapon => new WandOfInfernoWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.WandOfPlagueWeaponPlugin", weapon => new WandOfPlagueWeaponPlugin(weapon) },
            { "OpenTibia.GameData.Plugins.Weapons.WandOfVortexWeaponPlugin", weapon => new WandOfVortexWeaponPlugin(weapon) },
        };

        public static readonly Dictionary<string, Func<Ammunition, AmmunitionPlugin> > AmmunitionPlugins = new Dictionary<string, Func<Ammunition, AmmunitionPlugin> >()
        {
            { "OpenTibia.GameData.Plugins.Ammunitions.BurstArrowAmmunitionPlugin", ammunition => new BurstArrowAmmunitionPlugin(ammunition) },
            { "OpenTibia.GameData.Plugins.Ammunitions.PoisonArrowAmmunitionPlugin", ammunition => new PoisonArrowAmmunitionPlugin(ammunition) },
        };

        public static readonly Dictionary<string, Func<Plugin> > OtherPlugins = new Dictionary<string, Func<Plugin> >()
        {
            
        };
    }
}
#endif