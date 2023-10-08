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
		-- { words = "exevo flam hur", name = "Fire Wave", group = "Attack", cooldown = 4, groupcooldown = 2, level = 18, mana = 25, premium = false, vocations = { vocation.sorcerer, vocation.mastersorcerer }, requirestarget = false, filename = "fire wave.lua" }
	},
	runes = {
		-- { opentibiaid = 2304, name = "Great Fireball Rune", group = "Attack", groupcooldown = 2, level = 30, magiclevel = 4, requirestarget = false, filename = "great fireball rune.lua" }
	},
	weapons = {
		-- { opentibiaid = 2187, level = 33, mana = 13, vocations = { vocation.sorcerer, vocation.mastersorcerer }, filename = "wand of inferno.lua" }
	},
	ammunitions = {
		-- { opentibiaid = 2546, filename = "burst arrow.lua" }
	}
}