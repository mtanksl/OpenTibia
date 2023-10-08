plugins = {
	actions = {
		-- { type = "PlayerRotateItem", opentibiaid = 1740, filename = "rotate item.lua" },
		-- { type = "PlayerUseItem", opentibiaid = 1740, filename = "use item.lua" },
		-- { type = "PlayerUseItemWithItem", opentibiaid = 2580, allowfaruse = true, filename = "use item with item.lua" },
		-- { type = "PlayerUseItemWithCreature", opentibiaid = 2580, allowfaruse = true, filename = "use item with creature.lua" },
		-- { type = "PlayerMoveItem", opentibiaid = 1740, filename = "move item.lua" }
	},
	movements = {
		-- { type = "CreatureStepIn", opentibiaid = 446, filename = "step in.lua" },
		-- { type = "CreatureStepOut", opentibiaid = 446, filename = "step out.lua" }
	},
	talkactions = {
		-- { type = "PlayerSay", message = "/hello", filename = "say.lua" }
	},
	npcs = {
		{ type = "Dialogue", name = "Al Dee", filename = "al dee.lua" },
		{ type = "Dialogue", name = "Rachel", filename = "rachel.lua" },
		{ type = "Dialogue", name = "Cipfried", filename = "cipfried.lua" },
		{ type = "Dialogue", name = "Captain Bluebear", filename = "captain bluebear.lua" }
	},
	spells = {
	  { words = "exani tera", name = "Magic Rope", group = "Support", cooldown = 2, groupcooldown = 2, level = 9, mana = 20, premium = true, vocations = { vocation.knight, vocation.paladin, vocation.druid, vocation.sorcerer, vocation.eliteknight, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.MagicRopeSpellPlugin" },
		{ words = "exani hur up", name = "Levitate", group = "Support", cooldown = 2, groupcooldown = 2, level = 12, mana = 50, premium = true, vocations = { vocation.knight, vocation.paladin, vocation.druid, vocation.sorcerer, vocation.eliteknight, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.LevitateUpSpellPlugin" },
		{ words = "exani hur down", name = "Levitate", group = "Support", cooldown = 2, groupcooldown = 2, level = 12, mana = 50, premium = true, vocations = { vocation.knight, vocation.paladin, vocation.druid, vocation.sorcerer, vocation.eliteknight, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.LevitateDownSpellPlugin" },
		{ words = "utevo lux", name = "Light", group = "Support", cooldown = 2, groupcooldown = 2, level = 8, mana = 20, premium = false, vocations = { vocation.knight, vocation.paladin, vocation.druid, vocation.sorcerer, vocation.eliteknight, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.LightSpellPlugin" },
		{ words = "utevo gran lux", name = "Great Light", group = "Support", cooldown = 2, groupcooldown = 2, level = 13, mana = 60, premium = false, vocations = { vocation.knight, vocation.paladin, vocation.druid, vocation.sorcerer, vocation.eliteknight, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.GreatLightSpellPlugin" },
		{ words = "utevo vis lux", name = "Ultimate Light", group = "Support", cooldown = 2, groupcooldown = 2, level = 26, mana = 140, premium = true, vocations = { vocation.druid, vocation.sorcerer, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.UltimateLightSpellPlugin" },
		{ words = "utana vid", name = "Invisible", group = "Support", cooldown = 2, groupcooldown = 2, level = 35, mana = 440, premium = false, vocations = { vocation.druid, vocation.sorcerer, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.InvisibleSpellPlugin" },
		{ words = "utani hur", name = "Haste", group = "Support", cooldown = 2, groupcooldown = 2, level = 14, mana = 60, premium = true, vocations = { vocation.knight, vocation.paladin, vocation.druid, vocation.sorcerer, vocation.eliteknight, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.HasteSpellPlugin" },
		{ words = "utani gran hur", name = "Strong Haste", group = "Support", cooldown = 2, groupcooldown = 2, level = 20, mana = 100, premium = true, vocations = { vocation.druid, vocation.sorcerer, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.StrongHasteSpellPlugin" },
		{ words = "utamo vita", name = "Magic Shield", group = "Support", cooldown = 14, groupcooldown = 2, level = 14, mana = 50, premium = false, vocations = { vocation.druid, vocation.sorcerer, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.MagicShieldSpellPlugin" },
		{ words = "exana pox", name = "Cure Poison", group = "Healing", cooldown = 6, groupcooldown = 1, level = 10, mana = 30, premium = false, vocations = { vocation.knight, vocation.paladin, vocation.druid, vocation.sorcerer, vocation.eliteknight, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.CurePoisonSpellPlugin" },
		{ words = "exura", name = "Light Healing", group = "Healing", cooldown = 1, groupcooldown = 1, level = 8, mana = 20, premium = false, vocations = { vocation.paladin, vocation.druid, vocation.sorcerer, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.LightHealingSpellPlugin" },
		{ words = "exura gran", name = "Intense Healing", group = "Healing", cooldown = 1, groupcooldown = 1, level = 20, mana = 70, premium = false, vocations = { vocation.paladin, vocation.druid, vocation.sorcerer, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.IntenseHealingSpellPlugin" },
		{ words = "exura ico", name = "Wound Cleansing", group = "Healing", cooldown = 1, groupcooldown = 1, level = 8, mana = 40, premium = false, vocations = { vocation.knight, vocation.eliteknight }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.WoundCleansingSpellPlugin" },
		{ words = "exura san", name = "Divine Healing", group = "Healing", cooldown = 1, groupcooldown = 1, level = 35, mana = 160, premium = false, vocations = { vocation.paladin, vocation.royalpaladin }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.DivineHealingSpellPlugin" },
		{ words = "exura vita", name = "Ultimate Healing", group = "Healing", cooldown = 1, groupcooldown = 1, level = 30, mana = 160, premium = false, vocations = { vocation.druid, vocation.sorcerer, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.UltimateHealingSpellPlugin" },
		{ words = "exura gran mas res", name = "Mass Healing", group = "Healing", cooldown = 2, groupcooldown = 1, level = 36, mana = 150, premium = true, vocations = { vocation.druid, vocation.elderdruid }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.MassHealingSpellPlugin" },
		{ words = "exori mort", name = "Death Strike", group = "Attack", cooldown = 2, groupcooldown = 2, level = 16, mana = 20, premium = true, vocations = { vocation.sorcerer, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.DeathStrikeSpellPlugin" },
		{ words = "exori flam", name = "Flame Strike", group = "Attack", cooldown = 2, groupcooldown = 2, level = 14, mana = 20, premium = true, vocations = { vocation.druid, vocation.sorcerer, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.FlameStrikeSpellPlugin" },
		{ words = "exori vis", name = "Energy Strike", group = "Attack", cooldown = 2, groupcooldown = 2, level = 12, mana = 20, premium = true, vocations = { vocation.druid, vocation.sorcerer, vocation.elderdruid, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.EnergyStrikeSpellPlugin" },
		{ words = "exevo flam hur", name = "Fire Wave", group = "Attack", cooldown = 4, groupcooldown = 2, level = 18, mana = 25, premium = false, vocations = { vocation.sorcerer, vocation.mastersorcerer }, requirestarget = false, filename = "fire wave.lua" },
		{ words = "exevo vis lux", name = "Energy Beam", group = "Attack", cooldown = 4, groupcooldown = 2, level = 23, mana = 40, premium = false, vocations = { vocation.sorcerer, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.EnergyBeamSpellPlugin" },
		{ words = "exevo gran vis lux", name = "Great Energy Beam", group = "Attack", cooldown = 6, groupcooldown = 2, level = 29, mana = 110, premium = false, vocations = { vocation.sorcerer, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.GreatEnergyBeamSpellPlugin" },
		{ words = "exevo vis hur", name = "Energy Wave", group = "Attack", cooldown = 8, groupcooldown = 2, level = 38, mana = 170, premium = false, vocations = { vocation.sorcerer, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.EnergyWaveSpellPlugin" },
		{ words = "exevo gran mas vis", name = "Rage of the Skies", group = "Attack", cooldown = 40, groupcooldown = 4, level = 55, mana = 600, premium = true, vocations = { vocation.sorcerer, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.RageOfTheSkiesSpellPlugin" },
		{ words = "exevo gran mas flam", name = "Hell's Core", group = "Attack", cooldown = 40, groupcooldown = 4, level = 60, mana = 1100, premium = true, vocations = { vocation.sorcerer, vocation.mastersorcerer }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.HellsCoreSpellPlugin" },
		{ words = "exevo gran mas tera", name = "Wrath of Nature", group = "Attack", cooldown = 40, groupcooldown = 4, level = 55, mana = 700, premium = true, vocations = { vocation.druid, vocation.elderdruid }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.WrathOfNatureSpellPlugin" },
		{ words = "exevo gran mas frigo", name = "Eternal Winter", group = "Attack", cooldown = 40, groupcooldown = 4, level = 60, mana = 1050, premium = true, vocations = { vocation.druid, vocation.elderdruid }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.EternalWinterSpellPlugin" },
		{ words = "exori mas", name = "Groundshaker", group = "Attack", cooldown = 8, groupcooldown = 2, level = 33, mana = 160, premium = true, vocations = { vocation.knight, vocation.eliteknight }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.GroundshakerSpellPlugin" },
		{ words = "exori", name = "Berserk", group = "Attack", cooldown = 4, groupcooldown = 2, level = 35, mana = 115, premium = true, vocations = { vocation.knight, vocation.eliteknight }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.BerserkSpellPlugin" },
		{ words = "exori gran", name = "Fierce Berserk", group = "Attack", cooldown = 6, groupcooldown = 2, level = 90, mana = 340, premium = true, vocations = { vocation.knight, vocation.eliteknight }, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Spells.FierceBerserkSpellPlugin" }
	},
	runes = {
		{ opentibiaid = 2266, name = "Cure Poison Rune", group = "Healing", groupcooldown = 2, level = 15, magiclevel = 0, requirestarget = true, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.CurePoisonRunePlugin" },
		{ opentibiaid = 2265, name = "Intense Healing Rune", group = "Healing", groupcooldown = 2, level = 15, magiclevel = 1, requirestarget = true, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.IntenseHealingRunePlugin" },
		{ opentibiaid = 2273, name = "Ultimate Healing Rune", group = "Healing", groupcooldown = 2, level = 24, magiclevel = 4, requirestarget = true, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.UltimateHealingRunePlugin" },
		{ opentibiaid = 2287, name = "Light Magic Missile Rune", group = "Attack", groupcooldown = 2, level = 15, magiclevel = 0, requirestarget = true, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.LightMagicMissileRunePlugin" },
		{ opentibiaid = 2311, name = "Heavy Magic Missile Rune", group = "Attack", groupcooldown = 2, level = 25, magiclevel = 3, requirestarget = true, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.HeavyMagicMissileRunePlugin" },
		{ opentibiaid = 2268, name = "Sudden Death Rune", group = "Attack", groupcooldown = 2, level = 45, magiclevel = 15, requirestarget = true, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.SuddenDeathRunePlugin" },
		
		{ opentibiaid = 2285, name = "Poison Field Rune", group = "Attack", groupcooldown = 2, level = 14, magiclevel = 0, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.PoisonFieldRunePlugin" },
		{ opentibiaid = 2286, name = "Poison Bomb Rune", group = "Attack", groupcooldown = 2, level = 25, magiclevel = 4, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.PoisonBombRunePlugin" },
		{ opentibiaid = 2289, name = "Poison Wall Rune", group = "Attack", groupcooldown = 2, level = 29, magiclevel = 5, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.PoisonWallRunePlugin" },
		{ opentibiaid = 2301, name = "Fire Field Rune", group = "Attack", groupcooldown = 2, level = 15, magiclevel = 1, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.FireFieldRunePlugin" },
		{ opentibiaid = 2305, name = "Fire Bomb Rune", group = "Attack", groupcooldown = 2, level = 27, magiclevel = 5, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.FireBombRunePlugin" },
		{ opentibiaid = 2303, name = "Fire Wall Rune", group = "Attack", groupcooldown = 2, level = 33, magiclevel = 6, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.FireWallRunePlugin" },
		{ opentibiaid = 2277, name = "Energy Field Rune", group = "Attack", groupcooldown = 2, level = 18, magiclevel = 3, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.EnergyFieldRunePlugin" },
		{ opentibiaid = 2262, name = "Energy Bomb Rune", group = "Attack", groupcooldown = 2, level = 37, magiclevel = 10, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.EnergyBombRunePlugin" },
		{ opentibiaid = 2279, name = "Energy Wall Rune", group = "Attack", groupcooldown = 2, level = 41, magiclevel = 9, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.EnergyWallRunePlugin" },
		{ opentibiaid = 2302, name = "Fireball Rune", group = "Attack", groupcooldown = 2, level = 27, magiclevel = 4, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.FireballRunePlugin" },
		{ opentibiaid = 2304, name = "Great Fireball Rune", group = "Attack", groupcooldown = 2, level = 30, magiclevel = 4, requirestarget = false, filename = "great fireball rune.lua" },
		{ opentibiaid = 2313, name = "Explosion Rune", group = "Attack", groupcooldown = 2, level = 31, magiclevel = 6, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.ExplosionRunePlugin" },
		{ opentibiaid = 2293, name = "Magic Wall Rune", group = "Support", groupcooldown = 2, level = 27, magiclevel = 9, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.MagicWallRunePlugin" },
		{ opentibiaid = 2269, name = "Wild Growth Rune", group = "Support", groupcooldown = 2, level = 27, magiclevel = 8, requirestarget = false, filename = "mtanksl.OpenTibia.GameData.Plugins.Runes.WildGrowthRunePlugin" }
	},
	weapons = {
		{ opentibiaid = 2187, level = 33, mana = 13, vocations = { vocation.sorcerer, vocation.mastersorcerer }, filename = "wand of inferno.lua" },		
		{ opentibiaid = 2188, level = 19, mana = 5, vocations = { vocation.sorcerer, vocation.mastersorcerer }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.WandOfPlagueWeaponPlugin" },
		{ opentibiaid = 2189, level = 26, mana = 8, vocations = { vocation.sorcerer, vocation.mastersorcerer }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.WandOfCosmicEnergyWeaponPlugin" },
		{ opentibiaid = 2190, level = 7, mana = 2, vocations = { vocation.sorcerer, vocation.mastersorcerer }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.WandOfVortexWeaponPlugin" },
		{ opentibiaid = 2191, level = 13, mana = 3, vocations = { vocation.sorcerer, vocation.mastersorcerer }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.WandOfDragonbreathWeaponPlugin" },
		{ opentibiaid = 2181, level = 26, mana = 8, vocations = { vocation.druid, vocation.elderdruid }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.QuagmireRodWeaponPlugin" },
		{ opentibiaid = 2182, level = 6, mana = 2, vocations = { vocation.druid, vocation.elderdruid }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.SnakebiteRodWeaponPlugin" },
		{ opentibiaid = 2183, level = 33, mana = 13, vocations = { vocation.druid, vocation.elderdruid }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.TempestRodWeaponPlugin" },
		{ opentibiaid = 2185, level = 19, mana = 5, vocations = { vocation.druid, vocation.elderdruid }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.VolcanicRodWeaponPlugin" },
		{ opentibiaid = 2186, level = 13, mana = 3, vocations = { vocation.druid, vocation.elderdruid }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.MoonlightRodWeaponPlugin" },
		{ opentibiaid = 7366, level = 0, mana = 0, vocations = { vocation.knight, vocation.paladin, vocation.druid, vocation.sorcerer, vocation.eliteknight, vocation.royalpaladin, vocation.elderdruid, vocation.mastersorcerer }, filename = "mtanksl.OpenTibia.GameData.Plugins.Weapons.ViperStarWeaponPlugin" }		
	},
	ammunitions = {
		{ opentibiaid = 2545, filename = "mtanksl.OpenTibia.GameData.Plugins.Ammunitions.PoisonArrowAmmunitionPlugin" },
		{ opentibiaid = 2546, filename = "burst arrow.lua" }
	}
}