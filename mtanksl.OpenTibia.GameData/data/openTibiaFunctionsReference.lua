return {
	-- void print(params object[] parameters)
	print = {
		type = "function",
		description = "",
		args = "(params object[] parameters)",
		returns = "void",
	},
	-- string typeof(object obj)
	typeof = {
		type = "function",
		description = "",
		args = "(object obj)",
		returns = "string",
	},
	-- object cast(object obj, string typeName)
	cast = {
		type = "function",
		description = "",
		args = "(object obj, string typeName)",
		returns = "object",
	},
	-- object getconfig(string file, string key)
	getconfig = {
		type = "function",
		description = "",
		args = "(string file, string key)",
		returns = "object",
	},
	-- string getfullpath(string relativePath)
	getfullpath = {
		type = "function",
		description = "",
		args = "(string relativePath)",
		returns = "string",
	},
	-- void registerplugin(string type, LuaTable parameters)
	registerplugin = {
		type = "function",
		description = "",
		args = "(string type, LuaTable parameters)",
		returns = "void",
	},
	debugger = {
		type = "lib",
		description = "",
		childs = {
			--void debugger.start()
			start = {
				type = "function",
				description = "",
				args = "()",
				returns = "void" 
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
				args = "(int seconds [, Action callback])",
				returns = "string",
			},
			-- void command.delaygameobject(GameObject gameObject, int seconds)
			-- string command.delaygameobject(GameObject gameObject, int seconds, Action callback)
			delaygameobject = {
				type = "function",
				description = "",
				args = "(GameObject gameObject, int seconds [, Action callback])",
				returns = "string",
			},
			-- bool command.canceldelay(string key)
			canceldelay = {
				type = "function",
				description = "",
				args = "(string key)",
				returns = "bool",
			},
			-- void command.containeradditem(Container container, Item item)
			containeradditem = {
				type = "function",
				description = "",
				args = "(Container container, Item item)",
				returns = "void",
			},	
			-- Item command.containercreateitem(Container container, ushort openTibiaId, byte count)
			containercreateitem = {
				type = "function",
				description = "",
				args = "(Container container, ushort openTibiaId, byte count)",
				returns = "Item",
			},	
			-- void command.containerremoveitem(Container container, Item item)
			containerremoveitem = {
				type = "function",
				description = "",
				args = "(Container container, Item item)",
				returns = "void",
			},
			-- void command.containerreplaceitem(Container container, Item fromItem, Item toItem)
			containerreplaceitem = {
				type = "function",
				description = "",
				args = "(Container container, Item fromItem, Item toItem)",
				returns = "void",
			},
			-- void command.creatureaddcondition(Creature target, Condition condition)
			creatureaddcondition = {
				type = "function",
				description = "",
				args = "(Creature target, Condition condition)",
				returns = "void",
			},	
			-- void command.creatureattackarea(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Attack attack, Condition condition)
			creatureattackarea = {
				type = "function",
				description = "",
				args = "(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Attack attack, Condition condition)",
				returns = "void",
			},	
			-- void command.creatureattackcreature(Creature attacker, Creature target, Attack attack, Condition condition)
			creatureattackcreature = {
				type = "function",
				description = "",
				args = "(Creature attacker, Creature target, Attack attack, Condition condition)",
				returns = "void",
			},
			-- void command.creatureremovecondition(Creature creature, ConditionSpecialCondition conditionSpecialCondition)
			creatureremovecondition = {
				type = "function",
				description = "",
				args = "(Creature creature, ConditionSpecialCondition conditionSpecialCondition)",
				returns = "void",
			},	
			-- void command.creaturedestroy(Creature creature)
			creaturedestroy = {
				type = "function",
				description = "",
				args = "(Creature creature)",
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
			-- void command.creaturemove(Creature creature, Tile toTile)
			creaturemove = {
				type = "function",
				description = "",
				args = "(Creature creature, Tile toTile)",
				returns = "void",
			},
			-- void command.fluiditemupdatefluidtype(FluidItem fluidItem, FluidType fluidType)
			fluiditemupdatefluidtype = {
				type = "function",
				description = "",
				args = "(FluidItem fluidItem, FluidType fluidType)",
				returns = "void",
			},
			-- void command.inventoryadditem(Inventory inventory, byte slot, Item item)
			inventoryadditem = {
				type = "function",
				description = "",
				args = "(Inventory inventory, byte slot, Item item)",
				returns = "void",
			},
			-- Item command.inventorycreateitem(Inventory inventory, byte slot, ushort openTibiaId, byte count)
			inventorycreateitem = {
				type = "function",
				description = "",
				args = "(Inventory inventory, byte slot, ushort openTibiaId, byte count)",
				returns = "Item",
			},
			-- void command.inventoryremoveitem(Inventory inventory, Item item)
			inventoryremoveitem = {
				type = "function",
				description = "",
				args = "(Inventory inventory, Item item)",
				returns = "void",
			},
			-- void command.inventoryreplaceitem(Inventory inventory, Item fromItem, Item toItem)
			inventoryreplaceitem = {
				type = "function",
				description = "",
				args = "(Inventory inventory, Item fromItem, Item toItem)",
				returns = "void",
			},
			-- Item command.itemclone(Item item, bool deepClone)
			itemclone = {
				type = "function",
				description = "",
				args = "(Item item, bool deepClone)",
				returns = "Item",
			},
			-- void command.itemdecrement(Item item, byte count)
			itemdecrement = {
				type = "function",
				description = "",
				args = "(Item item, byte count)",
				returns = "void",
			},
			-- void command.itemdestroy(Item item)
			itemdestroy = {
				type = "function",
				description = "",
				args = "(Item item)",
				returns = "void",
			},
			-- void command.itemmove(Item item, IContainer toContainer, byte toIndex)
			itemmove = {
				type = "function",
				description = "",
				args = "(Item item, IContainer toContainer, byte toIndex)",
				returns = "void",
			},
			-- Item command.itemtransform(Item item, ushort openTibiaId, byte count)
			itemtransform = {
				type = "function",
				description = "",
				args = "(Item item, ushort openTibiaId, byte count)",
				returns = "Item",
			},
			-- void command.monstersay(Monster monster, string message)
			monstersay = {
				type = "function",
				description = "",
				args = "(Monster monster, string message)",
				returns = "void",
			},
			-- void command.npcsay(Npc npc, string message)
			npcsay = {
				type = "function",
				description = "",
				args = "(Npc npc, string message)",
				returns = "void",
			},
			-- void command.npcsaytoplayer(Npc npc, Player player, string message)
			npcsaytoplayer = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player, string message)",
				returns = "void",
			},
			-- void command.npctrade(Npc npc, Player player, object[] offers)
			npctrade = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player, object[] offers)",
				returns = "void",
			},
			-- void command.npcidle(Npc npc, Player player)
			npcidle = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player)",
				returns = "void",
			},
			-- void command.npcfarewell(Npc npc, Player player)
			npcfarewell = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player)",
				returns = "void",
			},
			-- void command.npcdisappear(Npc npc, Player player)
			npcdisappear = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player)",
				returns = "void",
			},
			-- void command.playercreatemoney(Player player, int price)
			playercreatemoney = {
				type = "function",
				description = "",
				args = "(Player player, int price)",
				returns = "void",
			},
			-- bool command.playerdestroymoney(Player player, int price)
			playerdestroymoney = {
				type = "function",
				description = "",
				args = "(Player player, int price)",
				returns = "bool",
			},
			-- int command.playercountmoney(Player player)
			playercountmoney = {
				type = "function",
				description = "",
				args = "(Player player)",
				returns = "int",
			},
			-- void command.playercreateitems(Player player, ushort openTibiaId, byte type, int count)
			playercreateitems = {
				type = "function",
				description = "",
				args = "(Player player, ushort openTibiaId, byte type, int count)",
				returns = "void",
			},
			-- bool command.playerdestroyitems(Player player, ushort openTibiaId, byte type, int count)
			playerdestroyitems = {
				type = "function",
				description = "",
				args = "(Player player, ushort openTibiaId, byte type, int count)",
				returns = "bool",
			},
			-- int command.playercountitems(Player player, ushort openTibiaId, byte type)
			playercountitems = {
				type = "function",
				description = "",
				args = "(Player player, ushort openTibiaId, byte type)",
				returns = "int",
			},
			-- void command.playerachievement(Player player, int incrementStorageKey, int requiredStorageValue, string achievementName)
			playerachievement = {
				type = "function",
				description = "",
				args = "(Player player, int incrementStorageKey, int requiredStorageValue, string achievementName)",
				returns = "void",
			},
			-- void command.playerupdateaxe(Player player, byte skill, byte skillPercent)
			playerupdateaxe = {
				type = "function",
				description = "",
				args = "(Player player, byte skill, byte skillPercent)",
				returns = "void",
			},
			-- void command.playerupdateclub(Player player, byte skill, byte skillPercent)
			playerupdateclub = {
				type = "function",
				description = "",
				args = "(Player player, byte skill, byte skillPercent)",
				returns = "void",
			},
			-- void command.playerupdatedistance(Player player, byte skill, byte skillPercent)
			playerupdatedistance = {
				type = "function",
				description = "",
				args = "(Player player, byte skill, byte skillPercent)",
				returns = "void",
			},
			-- void command.playerupdatefish(Player player, byte skill, byte skillPercent)
			playerupdatefish = {
				type = "function",
				description = "",
				args = "(Player player, byte skill, byte skillPercent)",
				returns = "void",
			},
			-- void command.playerupdatefist(Player player, byte skill, byte skillPercent)
			playerupdatefist = {
				type = "function",
				description = "",
				args = "(Player player, byte skill, byte skillPercent)",
				returns = "void",
			},
			-- void command.playerupdatemagiclevel(Player player, byte skill, byte skillPercent)
			playerupdatemagiclevel = {
				type = "function",
				description = "",
				args = "(Player player, byte skill, byte skillPercent)",
				returns = "void",
			},
			-- void command.playerupdateshield(Player player, byte skill, byte skillPercent)
			playerupdateshield = {
				type = "function",
				description = "",
				args = "(Player player, byte skill, byte skillPercent)",
				returns = "void",
			},
			-- void command.playerupdatesword(Player player, byte skill, byte skillPercent)
			playerupdatesword = {
				type = "function",
				description = "",
				args = "(Player player, byte skill, byte skillPercent)",
				returns = "void",
			},
			-- void command.playersay(Player player, string message)
			playersay = {
				type = "function",
				description = "",
				args = "(Player player, string message)",
				returns = "void",
			},
			-- void command.playerupdatecapacity(Player player, int capacity)
			playerupdatecapacity = {
				type = "function",
				description = "",
				args = "(Player player, int capacity)",
				returns = "void",
			},
			-- void command.playerupdateexperience(Player player, ulong experience, ushort level, byte levelPercent)
			playerupdateexperience = {
				type = "function",
				description = "",
				args = "(Player player, ulong experience, ushort level, byte levelPercent)",
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
			-- void command.playerwhisper(Player player, string message)
			playerwhisper = {
				type = "function",
				description = "",
				args = "(Player player, string message)",
				returns = "void",
			},
			-- void command.playeryell(Player player, string message)
			playeryell = {
				type = "function",
				description = "",
				args = "(Player player, string message)",
				returns = "void",
			},
			-- (bool, Addon) command.playergetoutfit(Player player, ushort outfitId)
			playergetoutfit = {
				type = "function",
				description = "",
				args = "(Player player, ushort outfitId)",
				returns = "(bool, Addon)",
			},
			-- void command.playersetoutfit(Player player, ushort outfitId, Addon addon)
			playersetoutfit = {
				type = "function",
				description = "",
				args = "(Player player, ushort outfitId, Addon addon)",
				returns = "void",
			},
			-- void command.playerremoveoutfit(Player player, ushort outfitId)
			playerremoveoutfit = {
				type = "function",
				description = "",
				args = "(Player player, ushort outfitId)",
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
			-- void command.playerremovestorage(Player player, int key)
			playerremovestorage = {
				type = "function",
				description = "",
				args = "(Player player, int key)",
				returns = "void",
			},
			-- string[] command.playergetspells(Player player)
			playergetspells = {
				type = "function",
				description = "",
				args = "(Player player)",
				returns = "string[]",
			},
			-- void command.playersetspell(Player player, string name)
			playersetspell = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "void",
			},
			-- void command.playerremovespell(Player player, string name)
			playerremovespell = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "void",
			},			
			-- void command.showanimatedtext(Position position, AnimatedTextColor animatedTextColor, string message)
			-- void command.showanimatedtext(IContent content, AnimatedTextColor animatedTextColor, string message)
			showanimatedtext = {
				type = "function",
				description = "",
				args = "(Position position, AnimatedTextColor animatedTextColor, string message)",
				returns = "void",
			},
			-- void command.showmagiceffect(Position position, MagicEffectType magicEffectType)
			-- void command.showmagiceffect(IContent content, MagicEffectType magicEffectType)
			showmagiceffect = {
				type = "function",
				description = "",
				args = "(Position position, MagicEffectType magicEffectType)",
				returns = "void",
			},
			-- void command.showprojectile(IContent fromContent, IContent toContent, ProjectileType projectileType)
			-- void command.showprojectile(IContent fromContent, Position toPosition, ProjectileType projectileType)
			-- void command.showprojectile(Position fromPosition, IContent toContent, ProjectileType projectileType)
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
			},
			-- void command.tileaddcreature(Tile tile, Creature creature)
			tileaddcreature = {
				type = "function",
				description = "",
				args = "(Tile tile, Creature creature)",
				returns = "void",
			},
			-- void command.tileadditem(Tile tile, Item item)
			tileadditem = {
				type = "function",
				description = "",
				args = "(Tile tile, Item item)",
				returns = "void",
			},
			-- Item command.tilecreateitem(Tile tile, ushort openTibiaId, byte count)
			tilecreateitem = {
				type = "function",
				description = "",
				args = "(Tile tile, ushort openTibiaId, byte count)",
				returns = "Item",
			},
			-- void command.tilecreateitemorincrement(Tile tile, ushort openTibiaId, byte count)
			tilecreateitemorincrement = {
				type = "function",
				description = "",
				args = "(Tile tile, ushort openTibiaId, byte count)",
				returns = "void",
			},
			-- Item command.tilecreatemonstercorpse(Tile tile, MonsterMetadata metadata)
			tilecreatemonstercorpse = {
				type = "function",
				description = "",
				args = "(Tile tile, MonsterMetadata metadata)",
				returns = "Item",
			},
			-- Item command.tilecreateplayercorpse(Tile tile, Player player)
			tilecreateplayercorpse = {
				type = "function",
				description = "",
				args = "(Tile tile, Player player)",
				returns = "Item",
			},
			-- Monster command.tilecreatemonster(Tile tile, string name)
			tilecreatemonster = {
				type = "function",
				description = "",
				args = "(Tile tile, string name)",
				returns = "Monster",
			},
			-- Npc command.tilecreatenpc(Tile tile, string name)
			tilecreatenpc = {
				type = "function",
				description = "",
				args = "(Tile tile, string name)",
				returns = "Npc",
			},
			-- void command.tileremovecreature(Tile tile, Creature creature)
			tileremovecreature = {
				type = "function",
				description = "",
				args = "(Tile tile, Creature creature)",
				returns = "void",
			},
			-- void command.tileremoveitem(Tile tile, Item item)
			tileremoveitem = {
				type = "function",
				description = "",
				args = "(Tile tile, Item item)",
				returns = "void",
			},
			-- void command.tilereplaceitem(Tile tile, Item fromItem, Item toItem)
			tilereplaceitem = {
				type = "function",
				description = "",
				args = "(Tile tile, Item fromItem, Item toItem)",
				returns = "void",
			}
		}
	}
}