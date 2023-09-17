return {
	debugger = {
		type = "lib",
		description = "",
		childs = {
			start = {
				type = "function",
				description = "",
				args = "",
				returns = "" 
			}
		}
	},
	command = {
		type = "lib",
		description = "",
		childs = {
			-- void command.delay(int seconds)
			-- string command.delay(int seconds, Action callback)
			delay = {
				type = "function",
				description = "",
				args = "(int seconds | int seconds, Action callback)",
				returns = "void | string",
			},
			-- void command.delaygameobject(GameObject gameObject, int seconds)
			-- string command.delaygameobject(GameObject gameObject, int seconds, Action callback)
			delaygameobject = {
				type = "function",
				description = "",
				args = "(GameObject gameObject, int seconds | GameObject gameObject, int seconds, Action callback)",
				returns = "void | string",
			},
			-- bool command.canceldelay(string key)
			canceldelay = {
				type = "function",
				description = "",
				args = "(string key)",
				returns = "bool",
			},
			-- void command.creaturewalk(Creature creature, Tile tile)
			creaturewalk = {
				type = "function",
				description = "",
				args = "(Creature creature, Tile tile)",
				returns = "void",
			},
			-- void command.creatureupdatedirection(Creature creature, Direction direction)
			creatureupdatedirection = {
				type = "function",
				description = "",
				args = "(Creature creature, Direction direction)",
				returns = "void",
			},
			-- void command.creatureupdatehealth(Creature creature, int health)
			creatureupdatehealth = {
				type = "function",
				description = "",
				args = "(Creature creature, int health)",
				returns = "void",
			},
			-- void command.creatureupdateinvisible(Creature creature, bool invisible)
			creatureupdateinvisible = {
				type = "function",
				description = "",
				args = "(Creature creature, bool invisible)",
				returns = "void",
			},
			-- void command.creatureupdatelight(Creature creature, Light light)
			creatureupdatelight = {
				type = "function",
				description = "",
				args = "(Creature creature, Light light)",
				returns = "void",
			},
			-- void command.creatureupdateoutfit(Creature creature, Outfit baseOutfit, Outfit outfit)
			creatureupdateoutfit = {
				type = "function",
				description = "",
				args = "(Creature creature, Outfit baseOutfit, Outfit outfit)",
				returns = "void",
			},
			-- void command.creatureupdatepartyicon(Creature creature, PartyIcon partyIcon)
			creatureupdatepartyicon = {
				type = "function",
				description = "",
				args = "(Creature creature, PartyIcon partyIcon)",
				returns = "void",
			},
			-- void command.creatureupdateskullicon(Creature creature, SkullIcon skullIcon)
			creatureupdateskullicon = {
				type = "function",
				description = "",
				args = "(Creature creature, SkullIcon skullIcon)",
				returns = "void",
			},
			-- void command.creatureupdatespeed(Creature creature, ushort baseSpeed, ushort speed)
			creatureupdatespeed = {
				type = "function",
				description = "",
				args = "(Creature creature, ushort baseSpeed, ushort speed)",
				returns = "void",
			},
			-- void command.showanimatedtext(Position position, AnimatedTextColor animatedTextColor, string message)
			showanimatedtext = {
				type = "function",
				description = "",
				args = "(Position position, AnimatedTextColor animatedTextColor, string message)",
				returns = "void",
			},
			-- void command.showmagiceffect(Position position, MagicEffectType magicEffectType)
			showmagiceffect = {
				type = "function",
				description = "",
				args = "(Position position, MagicEffectType magicEffectType)",
				returns = "void",
			},
			-- void command.showprojectile(Position fromPosition, Position toPosition, ProjectileType projectileType)
			showprojectile = {
				type = "function",
				description = "",
				args = "(Position fromPosition, Position toPosition, ProjectileType projectileType)",
				returns = "void",
			},
			-- void command.showtext(Creature creature, TalkType talkType, string message)
			showtext = {
				type = "function",
				description = "",
				args = "(Creature creature, TalkType talkType, string message)",
				returns = "void",
			},
			-- void command.showwindowtext(Player player, TextColor textColor, string message)
			showwindowtext = {
				type = "function",
				description = "",
				args = "(Player player, TextColor textColor, string message)",
				returns = "void",
			},
			-- void command.fluiditemupdatefluidtype(FluidItem fluidItem, FluidType fluidType)
			fluiditemupdatefluidtype = {
				type = "function",
				description = "",
				args = "(FluidItem fluidItem, FluidType fluidType)",
				returns = "void",
			},
			-- void command.itemdestroy(Item item)
			itemdestroy = {
				type = "function",
				description = "",
				args = "(Item item)",
				returns = "void",
			},
			-- Item command.itemtransform(Item item, ushort openTibiaId, byte count)
			itemtransform = {
				type = "function",
				description = "",
				args = "(Item item, ushort openTibiaId, byte count)",
				returns = "Item",
			},
			-- void command.monsterdestroy(Monster monster)
			monsterdestroy = {
				type = "function",
				description = "",
				args = "(Monster monster)",
				returns = "void",
			},
			-- void command.monstersay(Monster monster, string message)
			monstersay = {
				type = "function",
				description = "",
				args = "(Monster monster, string message)",
				returns = "void",
			},
			-- void command.npcdestroy(Npc npc)
			npcdestroy = {
				type = "function",
				description = "",
				args = "(Npc npc)",
				returns = "void",
			},
			-- void command.npcsay(Npc npc, string message)
			npcsay = {
				type = "function",
				description = "",
				args = "(Npc npc, string message)",
				returns = "void",
			},
			-- void command.npcaddmoney(Player player, int price)
			npcaddmoney = {
				type = "function",
				description = "",
				args = "(Player player, int price)",
				returns = "void",
			},
			-- void command.npcdeletemoney(Player player, int price)
			npcdeletemoney = {
				type = "function",
				description = "",
				args = "(Player player, int price)",
				returns = "void",
			},
			-- int command.npccountmoney(Player player)
			npccountmoney = {
				type = "function",
				description = "",
				args = "(Player player)",
				returns = "int",
			},
			-- void command.npcadditem(Player player, ushort openTibiaId, byte type, int count)
			npcadditem = {
				type = "function",
				description = "",
				args = "(Player player, ushort openTibiaId, byte type, int count)",
				returns = "void",
			},
			-- void command.npcremoveitem(Player player, ushort openTibiaId, byte type, int count)
			npcremoveitem = {
				type = "function",
				description = "",
				args = "(Player player, ushort openTibiaId, byte type, int count)",
				returns = "void",
			},
			-- int command.npccountitem(Player player, ushort openTibiaId, byte type)
			npccountitem = {
				type = "function",
				description = "",
				args = "(Player player, ushort openTibiaId, byte type)",
				returns = "int",
			},
			-- void command.playerdestroy(Player player)
			playerdestroy = {
				type = "function",
				description = "",
				args = "(Player player)",
				returns = "void",
			},
			-- void command.playerupdatecapacity(Player player, int capacity)
			playerupdatecapacity = {
				type = "function",
				description = "",
				args = "(Player player, int capacity)",
				returns = "void",
			},
			-- void command.playerupdateexperience(Player player, int experience, ushort level, byte levelPercent)
			playerupdateexperience = {
				type = "function",
				description = "",
				args = "(Player player, int experience, ushort level, byte levelPercent)",
				returns = "void",
			},
			-- void command.playerupdatemana(Player player, int mana)
			playerupdatemana = {
				type = "function",
				description = "",
				args = "(Player player, int mana)",
				returns = "void",
			},
			-- void command.playerupdatesoul(Player player, int soul)
			playerupdatesoul = {
				type = "function",
				description = "",
				args = "(Player player, int soul)",
				returns = "void",
			},
			-- void command.playerupdatestamina(Player player, int stamina)
			playerupdatestamina = {
				type = "function",
				description = "",
				args = "(Player player, int stamina)",
				returns = "void",
			},
			-- (bool, int) command.playergetstorage(Player player, int key)
			playergetstorage = {
				type = "function",
				description = "",
				args = "(Player player, int key)",
				returns = "(bool, int)",
			},
			-- void command.playersetstorage(Player player, int key, int value)
			playersetstorage = {
				type = "function",
				description = "",
				args = "(Player player, int key, int value)",
				returns = "void",
			},
			-- void command.splashitemupdatefluidtype(SplashItem splashItem, FluidType fluidType)
			splashitemupdatefluidtype = {
				type = "function",
				description = "",
				args = "(SplashItem splashItem, FluidType fluidType)",
				returns = "void",
			},
			-- void command.stackableitemupdatecount(StackableItem stackableItem, byte count)
			stackableitemupdatecount = {
				type = "function",
				description = "",
				args = "(StackableItem stackableItem, byte count)",
				returns = "void",
			}
		}
	}
}