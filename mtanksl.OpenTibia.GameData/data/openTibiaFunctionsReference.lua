return {
	-- void print(params object[] parameters)
	print = {
		type = "function",
		description = "",
		args = "(params object[] parameters)",
		returns = "void"
	},
	-- string typeof(object obj)
	typeof = {
		type = "function",
		description = "",
		args = "(object obj)",
		returns = "string"
	},
	-- object cast(object obj, string typeName)
	cast = {
		type = "function",
		description = "",
		args = "(object obj, string typeName)",
		returns = "object"
	},
	-- object getconfig(string file, string key)
	getconfig = {
		type = "function",
		description = "",
		args = "(string file, string key)",
		returns = "object"
	},
	-- string getfullpath(string relativePath)
	getfullpath = {
		type = "function",
		description = "",
		args = "(string relativePath)",
		returns = "string"
	},
	-- void registerplugin(string nodeType, LuaTable parameters)
	registerplugin = {
		type = "function",
		description = "",
		args = "(string nodeType, LuaTable parameters)",
		returns = "void"
	},
	-- void registeractionsplayerrotateitem(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Player, Item, bool> onrotateitem)
	registeractionsplayerrotateitem = {
		type = "function",
		description = "",
		args = "(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Player, Item, bool> onrotateitem)",
		returns = "void"
	},
	-- void registeractionsplayeruseitem(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Player, Item, bool> onuseitem)
	registeractionsplayeruseitem = {
		type = "function",
		description = "",
		args = "(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Player, Item, bool> onuseitem)",
		returns = "void"
	},
	-- void registeractionsplayeruseitemwithitem(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, allowfaruse, Func<Player, Item, Item, bool> onuseitemwithitem)
	registeractionsplayeruseitemwithitem = {
		type = "function",
		description = "",
		args = "(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, allowfaruse, Func<Player, Item, Item, bool> onuseitemwithitem)",
		returns = "void"
	},
	-- void registeractionsplayeruseitemwithcreature(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, allowfaruse, Func<Player, Item, Creature, bool> onuseitemwithcreature)
	registeractionsplayeruseitemwithcreature = {
		type = "function",
		description = "",
		args = "(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, allowfaruse, Func<Player, Item, Creature, bool> onuseitemwithcreature)",
		returns = "void"
	},
	-- void registeractionsplayermoveitem(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Player, Item, IContainer, byte, byte, bool> onmoveitem)
	registeractionsplayermoveitem = {
		type = "function",
		description = "",
		args = "(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Player, Item, IContainer, byte, byte, bool> onmoveitem)",
		returns = "void"
	},
	-- void registeractionsplayermovecreature(string name, Func<Player, Creature, Tile, bool> onmovecreature)
	registeractionsplayermovecreature = {
		type = "function",
		description = "",
		args = "(string name, Func<Player, Creature, Tile, bool> onmovecreature)",
		returns = "void"
	},
	-- void registermovementscreaturestepin(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Creature, Tile, bool> onsteppingin, Action<Creature, Tile, Tile> onstepin)
	registermovementscreaturestepin = {
		type = "function",
		description = "",
		args = "(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Creature, Tile, bool> onsteppingin, Action<Creature, Tile, Tile> onstepin)",
		returns = "void"
	},
	-- void registermovementscreaturestepout(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Creature, Tile, bool> onsteppingout, Action<Creature, Tile, Tile> onstepout)
	registermovementscreaturestepout = {
		type = "function",
		description = "",
		args = "(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Creature, Tile, bool> onsteppingout, Action<Creature, Tile, Tile> onstepout)",
		returns = "void"
	},
	-- void registermovementsinventoryequip(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Inventory, Item, byte, bool> onequipping, Action<Inventory, Item, byte> onequip)
	registermovementsinventoryequip = {
		type = "function",
		description = "",
		args = "(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Inventory, Item, byte, bool> onequipping, Action<Inventory, Item, byte> onequip)",
		returns = "void"
	},
	-- void registermovementsinventorydeequip(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Inventory, Item, byte, bool> ondeequipping, Action<Inventory, Item, byte> ondeequip)
	registermovementsinventorydeequip = {
		type = "function",
		description = "",
		args = "(uint? id, ushort? uniqueid, ushort? actionid, ushort? opentibiaid, Func<Inventory, Item, byte, bool> ondeequipping, Action<Inventory, Item, byte> ondeequip)",
		returns = "void"
	},
	-- void registertalkactionsplayersay(string message, Func<Player, string, bool> onsay)
	registertalkactionsplayersay = {
		type = "function",
		description = "",
		args = "(string message, Func<Player, string, bool> onsay)",
		returns = "void"
	},
	-- void registercreaturescriptsplayerlogin(Action<Player> onlogin)
	registercreaturescriptsplayerlogin = {
		type = "function",
		description = "",
		args = "(Action<Player> onlogin)",
		returns = "void"
	},
	-- void registercreaturescriptsplayerlogout(Action<Player> onlogout)
	registercreaturescriptsplayerlogout = {
		type = "function",
		description = "",
		args = "(Action<Player> onlogout)",
		returns = "void"
	},
	-- void registercreaturescriptsplayeradvancelevel(Action<Player, ushort, ushort> onadvancelevel)
	registercreaturescriptsplayeradvancelevel = {
		type = "function",
		description = "",
		args = "(Action<Player, ushort, ushort> onadvancelevel)",
		returns = "void"
	},
	-- void registercreaturescriptsplayeradvanceskill(Action<Player, Skill, byte, byte> onadvanceskill)
	registercreaturescriptsplayeradvanceskill = {
		type = "function",
		description = "",
		args = "(Action<Player, Skill, byte, byte> onadvanceskill)",
		returns = "void"
	},
	-- void registercreaturescriptscreaturedeath(Action<Creature, Creature, Creature> ondeath)
	registercreaturescriptscreaturedeath = {
		type = "function",
		description = "",
		args = "(Action<Creature, Creature, Creature> ondeath)",
		returns = "void"
	},
	-- void registercreaturescriptscreaturekill(Action<Creature, Creature> onkill)
	registercreaturescriptscreaturekill = {
		type = "function",
		description = "",
		args = "(Action<Creature, Creature> onkill)",
		returns = "void"
	},
	-- void registercreaturescriptsplayerearnachievement(Action<Player, string> onearnachievement)
	registercreaturescriptsplayerearnachievement = {
		type = "function",
		description = "",
		args = "(Action<Player, string> onearnachievement)",
		returns = "void"
	},
	-- void registerglobaleventsserverstartup(Action onstartup)
	registerglobaleventsserverstartup = {
		type = "function",
		description = "",
		args = "(Action onstartup)",
		returns = "void"
	},
	-- void registerglobaleventsservershutdown(Action onshutdown)
	registerglobaleventsservershutdown = {
		type = "function",
		description = "",
		args = "(Action onshutdown)",
		returns = "void"
	},
	-- void registerglobaleventsserversave(Action onsave)
	registerglobaleventsserversave = {
		type = "function",
		description = "",
		args = "(Action onsave)",
		returns = "void"
	},
	-- void registerglobaleventsserverrecord(Action<uint> onrecord)
	registerglobaleventsserverrecord = {
		type = "function",
		description = "",
		args = "(Action<uint> onrecord)",
		returns = "void"
	},
	-- void registernpcsdialogue(string name, npchandler handler)
	registernpcsdialogue = {
		type = "function",
		description = "",
		args = "(string name, npchandler handler)",
		returns = "void"
	},
	-- void registeritemsitemcreation(ushort opentibiaid, Action<Item> onstart, Action<Item> onstop)
	registeritemsitemcreation = {
		type = "function",
		description = "",
		args = "(ushort opentibiaid, Action<Item> onstart, Action<Item> onstop)",
		returns = "void"
	},
	-- void registermonstersmonstercreation(string name, Action<Monster> onstart, Action<Monster> onstop)
	registermonstersmonstercreation = {
		type = "function",
		description = "",
		args = "(string name, Action<Monster> onstart, Action<Monster> onstop)",
		returns = "void"
	},
	-- void registernpcsnpccreation(string name, Action<Npc> onstart, Action<Npc> onstop)
	registernpcsnpccreation = {
		type = "function",
		description = "",
		args = "(string name, Action<Npc> onstart, Action<Npc> onstop)",
		returns = "void"
	},
	-- void registerplayersplayercreation(string name, Action<Player> onstart, Action<Player> onstop)
	registerplayersplayercreation = {
		type = "function",
		description = "",
		args = "(string name, Action<Player> onstart, Action<Player> onstop)",
		returns = "void"
	},
	-- void registerspell(words, name, group, cooldown, groupcooldown, level, mana, soul, premium, vocations, requirestarget, oncasting, oncast)
	registerspell = {
		type = "function",
		description = "",
		args = "(words, name, group, cooldown, groupcooldown, level, mana, soul, premium, vocations, requirestarget, oncasting, oncast)",
		returns = "void"
	},
	-- void registerrune(opentibiaid, name, group, groupcooldown, level, magiclevel, vocations, requirestarget, onusingrune, onuserune)
	registerrune = {
		type = "function",
		description = "",
		args = "(opentibiaid, name, group, groupcooldown, level, magiclevel, vocations, requirestarget, onusingrune, onuserune)",
		returns = "void"
	},
	-- void registerweapon(opentibiaid, level, mana, vocations, onusingweapon, onuseweapon)
	registerweapon = {
		type = "function",
		description = "",
		args = "(opentibiaid, level, mana, vocations, onusingweapon, onuseweapon)",
		returns = "void"
	},
	-- void registerammunition(opentibiaid, onusingammunition, onuseammunition)
	registerammunition = {
		type = "function",
		description = "",
		args = "(opentibiaid, onusingammunition, onuseammunition)",
		returns = "void"
	},
	-- void registerraid(name, repeatable, interval, chance, enabled, onraid)
	registerraid = {
		type = "function",
		description = "",
		args = "(name, repeatable, interval, chance, enabled, onraid)",
		returns = "void"
	},
	-- void registermonsterattack(string name, Func<Monster, Creature, bool> onattacking, Action<Monster, Creature, int, int, Dictionary<string, string>> onattack)
	registermonsterattack = {
		type = "function",
		description = "",
		args = "(string name, Func<Monster, Creature, bool> onattacking, Action<Monster, Creature, int, int, Dictionary<string, string>> onattack)",
		returns = "void"
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
			-- void command.registerplugin(string nodeType, LuaTable parameters)
			registerplugin = {
				type = "function",
				description = "",
				args = "(string nodeType, LuaTable parameters)",
				returns = "void"
			},
			-- PromiseResult<object[]> command.waithandle()
			waithandle = {
				type = "function",
				description = "",
				args = "()",
				returns = "PromiseResult<object[]>"
			},
			-- object[] command.wait(PromiseResult<object[]> promise) block
			wait = {
				type = "function",
				description = "",
				args = "(PromiseResult<object[]> promise)",
				returns = "object[]"
			},
			-- void command.set(PromiseResult<object[]> promise, params object[] parameters)
			set = {
				type = "function",
				description = "",
				args = "(PromiseResult<object[]> promise, params object[] parameters)",
				returns = "void"
			},
			-- void command.yield(Action callback)
			yield = {
				type = "function",
				description = "",
				args = "(Action callback)",
				returns = "void"
			},
			-- void command.delay(int milliseconds) block
			-- string command.delay(int milliseconds, Action callback)
			-- void command.delay(GameObject gameObject, int milliseconds) block
			-- string command.delay(GameObject gameObject, int milliseconds, Action callback)
			delay = {
				type = "function",
				description = "",
				args = "([GameObject gameObject, ] int milliseconds [, Action callback])",
				returns = "string"
			},
			-- bool command.canceldelay(string key)
			canceldelay = {
				type = "function",
				description = "",
				args = "(string key)",
				returns = "bool"
			},
			-- When eventName is fired, notify
			-- string command.eventhandler(string eventName, Action<GameEventArgs> callback)
			-- string command.eventhandler(GameObject gameObject, string eventName, Action<GameEventArgs> callback)
			eventhandler = {
				type = "function",
				description = "",
				args = "([GameObject gameObject, ] string eventName, Action<GameEventArgs> callback)",
				returns = "string"
			},
			-- bool command.canceleventhandler(string key)
			canceleventhandler = {
				type = "function",
				description = "",
				args = "(string key)",
				returns = "bool"
			},
			-- When eventName is fired by eventSource, notify
			-- string command.gameobjecteventhandler(GameObject eventSource, string eventName, Action<GameEventArgs> callback)
			-- string command.gameobjecteventhandler(GameObject gameObject, GameObject eventSource, string eventName, Action<GameEventArgs> callback)
			gameobjecteventhandler = {
				type = "function",
				description = "",
				args = "([GameObject gameObject, ] GameObject eventSource, string eventName, Action<GameEventArgs> callback)",
				returns = "string"
			},
			-- bool command.cancelgameobjecteventhandler(string key)
			cancelgameobjecteventhandler = {
				type = "function",
				description = "",
				args = "(string key)",
				returns = "bool"
			},
			-- When eventName is fired near observer, notify
			-- string command.positionaleventhandler(GameObject observer, string eventName, Action<GameEventArgs> callback)
			-- string command.positionaleventhandler(GameObject gameObject, GameObject observer, string eventName, Action<GameEventArgs> callback)
			positionaleventhandler = {
				type = "function",
				description = "",
				args = "([GameObject gameObject, ] GameObject observer, string eventName, Action<GameEventArgs> callback)",
				returns = "string"
			},
			-- bool command.cancelpositionaleventhandler(string key)
			cancelpositionaleventhandler = {
				type = "function",
				description = "",
				args = "(string key)",
				returns = "bool"
			},
			-- void command.containeradditem(Container container, Item item)
			containeradditem = {
				type = "function",
				description = "",
				args = "(Container container, Item item)",
				returns = "void"
			},	
			-- Item command.containercreateitem(Container container, ushort openTibiaId, byte typeCount)
			containercreateitem = {
				type = "function",
				description = "",
				args = "(Container container, ushort openTibiaId, byte typeCount)",
				returns = "Item"
			},	
			-- void command.containerremoveitem(Container container, Item item)
			containerremoveitem = {
				type = "function",
				description = "",
				args = "(Container container, Item item)",
				returns = "void"
			},
			-- void command.containerreplaceitem(Container container, Item fromItem, Item toItem)
			containerreplaceitem = {
				type = "function",
				description = "",
				args = "(Container container, Item fromItem, Item toItem)",
				returns = "void"
			},
			-- void command.creatureaddcondition(Creature target, Condition condition)
			creatureaddcondition = {
				type = "function",
				description = "",
				args = "(Creature target, Condition condition)",
				returns = "void"
			},	
			-- void command.creatureattackarea(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, Attack attack, Condition condition)
			-- void command.creatureattackarea(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType, ushort openTibiaId, byte typeCount, Attack attack, Condition condition)
			creatureattackarea = {
				type = "function",
				description = "",
				args = "(Creature attacker, bool beam, Position center, Offset[] area, ProjectileType? projectileType, MagicEffectType? magicEffectType [, ushort openTibiaId, byte typeCount], Attack attack, Condition condition)",
				returns = "void"
			},	
			-- void command.creatureattackcreature(Creature attacker, Creature target, Attack attack, Condition condition)
			creatureattackcreature = {
				type = "function",
				description = "",
				args = "(Creature attacker, Creature target, Attack attack, Condition condition)",
				returns = "void"
			},
			-- void command.creatureremovecondition(Creature creature, ConditionSpecialCondition conditionSpecialCondition)
			creatureremovecondition = {
				type = "function",
				description = "",
				args = "(Creature creature, ConditionSpecialCondition conditionSpecialCondition)",
				returns = "void"
			},	
			-- void command.creaturedestroy(Creature creature)
			creaturedestroy = {
				type = "function",
				description = "",
				args = "(Creature creature)",
				returns = "void"
			},					
			-- void command.creatureupdatedirection(Creature creature, Direction direction)
			creatureupdatedirection = {
				type = "function",
				description = "",
				args = "(Creature creature, Direction direction)",
				returns = "void"
			},
			-- void command.creatureupdatehealth(Creature creature, int health)
			creatureupdatehealth = {
				type = "function",
				description = "",
				args = "(Creature creature, int health)",
				returns = "void"
			},
			-- void command.creatureupdateinvisible(Creature creature, bool invisible)
			creatureupdateinvisible = {
				type = "function",
				description = "",
				args = "(Creature creature, bool invisible)",
				returns = "void"
			},
			-- void command.creatureupdatelight(Creature creature, Light conditionLight, Light itemLight)
			creatureupdatelight = {
				type = "function",
				description = "",
				args = "(Creature creature, Light conditionLight, Light itemLight)",
				returns = "void"
			},
			-- void command.creatureupdateoutfit(Creature creature, Outfit baseOutfit, Outfit conditionOutfit, bool swimming, bool conditionStealth, bool itemStealth)
			creatureupdateoutfit = {
				type = "function",
				description = "",
				args = "(Creature creature, Outfit baseOutfit, Outfit conditionOutfit, bool swimming, bool conditionStealth, bool itemStealth)",
				returns = "void"
			},
			-- void command.creatureupdatespeed(Creature creature, int conditionSpeed, int itemSpeed)
			creatureupdatespeed = {
				type = "function",
				description = "",
				args = "(Creature creature, int conditionSpeed, int itemSpeed)",
				returns = "void"
			},
			-- void command.creaturemove(Creature creature, Tile toTile)
			creaturemove = {
				type = "function",
				description = "",
				args = "(Creature creature, Tile toTile)",
				returns = "void"
			},
			-- void command.fluiditemupdatefluidtype(FluidItem fluidItem, FluidType fluidType)
			fluiditemupdatefluidtype = {
				type = "function",
				description = "",
				args = "(FluidItem fluidItem, FluidType fluidType)",
				returns = "void"
			},
			-- void command.inventoryadditem(Inventory inventory, byte slot, Item item)
			inventoryadditem = {
				type = "function",
				description = "",
				args = "(Inventory inventory, byte slot, Item item)",
				returns = "void"
			},
			-- Item command.inventorycreateitem(Inventory inventory, byte slot, ushort openTibiaId, byte typeCount)
			inventorycreateitem = {
				type = "function",
				description = "",
				args = "(Inventory inventory, byte slot, ushort openTibiaId, byte typeCount)",
				returns = "Item"
			},
			-- void command.inventoryremoveitem(Inventory inventory, Item item)
			inventoryremoveitem = {
				type = "function",
				description = "",
				args = "(Inventory inventory, Item item)",
				returns = "void"
			},
			-- void command.inventoryreplaceitem(Inventory inventory, Item fromItem, Item toItem)
			inventoryreplaceitem = {
				type = "function",
				description = "",
				args = "(Inventory inventory, Item fromItem, Item toItem)",
				returns = "void"
			},
			-- Item command.itemclone(Item item, bool deepClone)
			itemclone = {
				type = "function",
				description = "",
				args = "(Item item, bool deepClone)",
				returns = "Item"
			},
			-- void command.itemdecrement(Item item, byte count)
			itemdecrement = {
				type = "function",
				description = "",
				args = "(Item item, byte count)",
				returns = "void"
			},
			-- void command.itemdestroy(Item item)
			itemdestroy = {
				type = "function",
				description = "",
				args = "(Item item)",
				returns = "void"
			},
			-- void command.itemmove(Item item, IContainer toContainer, byte toIndex)
			itemmove = {
				type = "function",
				description = "",
				args = "(Item item, IContainer toContainer, byte toIndex)",
				returns = "void"
			},
			-- Item command.itemtransform(Item item, ushort openTibiaId, byte typeCount)
			itemtransform = {
				type = "function",
				description = "",
				args = "(Item item, ushort openTibiaId, byte typeCount)",
				returns = "Item"
			},
			-- void command.monstersay(Monster monster, string message)
			monstersay = {
				type = "function",
				description = "",
				args = "(Monster monster, string message)",
				returns = "void"
			},
			-- void command.monsteryell(Monster monster, string message)
			monsteryell = {
				type = "function",
				description = "",
				args = "(Monster monster, string message)",
				returns = "void"
			},
			-- void command.npcsay(Npc npc, string message)
			npcsay = {
				type = "function",
				description = "",
				args = "(Npc npc, string message)",
				returns = "void"
			},
			-- void command.npcsaytoplayer(Npc npc, Player player, string message)
			npcsaytoplayer = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player, string message)",
				returns = "void"
			},
			-- void command.npctrade(Npc npc, Player player, object[] offers)
			npctrade = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player, object[] offers)",
				returns = "void"
			},
			-- void command.npcidle(Npc npc, Player player)
			npcidle = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player)",
				returns = "void"
			},
			-- void command.npcfarewell(Npc npc, Player player)
			npcfarewell = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player)",
				returns = "void"
			},
			-- void command.npcdisappear(Npc npc, Player player)
			npcdisappear = {
				type = "function",
				description = "",
				args = "(Npc npc, Player player)",
				returns = "void"
			},
			-- void command.playercreatemoney(Player player, int price)
			playercreatemoney = {
				type = "function",
				description = "",
				args = "(Player player, int price)",
				returns = "void"
			},
			-- bool command.playerdestroymoney(Player player, int price)
			playerdestroymoney = {
				type = "function",
				description = "",
				args = "(Player player, int price)",
				returns = "bool"
			},
			-- int command.playercountmoney(Player player)
			playercountmoney = {
				type = "function",
				description = "",
				args = "(Player player)",
				returns = "int"
			},
			-- void command.playercreateitems(Player player, ushort openTibiaId, byte type, int count)
			playercreateitems = {
				type = "function",
				description = "",
				args = "(Player player, ushort openTibiaId, byte type, int count)",
				returns = "void"
			},
			-- bool command.playerdestroyitems(Player player, ushort openTibiaId, byte type, int count)
			playerdestroyitems = {
				type = "function",
				description = "",
				args = "(Player player, ushort openTibiaId, byte type, int count)",
				returns = "bool"
			},
			-- int command.playercountitems(Player player, ushort openTibiaId, byte type)
			playercountitems = {
				type = "function",
				description = "",
				args = "(Player player, ushort openTibiaId, byte type)",
				returns = "int"
			},
			-- void command.playerachievement(Player player, int incrementStorageKey, int requiredStorageValue, string achievementName)
			playerachievement = {
				type = "function",
				description = "",
				args = "(Player player, int incrementStorageKey, int requiredStorageValue, string achievementName)",
				returns = "void"
			},
			-- void command.playerbless(Player player, string message, string blessName)
			playerbless = {
				type = "function",
				description = "",
				args = "(Player player, string message, string blessName)",
				returns = "void"
			},
			-- void command.playeraddexperience(Player player, ulong experience)
			playeraddexperience = {
				type = "function",
				description = "",
				args = "(Player player, ulong experience)",
				returns = "void"
			},
			-- void command.playerremoveexperience(Player player, ulong experience)
			playerremoveexperience = {
				type = "function",
				description = "",
				args = "(Player player, ulong experience)",
				returns = "void"
			},
			-- void command.playeraddskillpoints(Player player, Skill skill, ulong skillPoints)
			playeraddskillpoints = {
				type = "function",
				description = "",
				args = "(Player player, Skill skill, ulong skillPoints)",
				returns = "void"
			},
			-- void command.playeraremovekillpoints(Player player, Skill skill, ulong skillPoints)
			playeraremovekillpoints = {
				type = "function",
				description = "",
				args = "(Player player, Skill skill, ulong skillPoints)",
				returns = "void"
			},
			-- void command.playerupdateskill(Player player, Skill skill, ulong skillPoints, byte skillLevel, byte skillPercent, int conditionSkillLevel, int itemSkillLevel)
			playerupdateskill = {
				type = "function",
				description = "",
				args = "(Player player, Skill skill, ulong skillPoints, byte skillLevel, byte skillPercent, int conditionSkillLevel, int itemSkillLevel)",
				returns = "void"
			},
			-- void command.playersay(Player player, string message)
			playersay = {
				type = "function",
				description = "",
				args = "(Player player, string message)",
				returns = "void"
			},
			-- void command.playerupdatecapacity(Player player, int capacity)
			playerupdatecapacity = {
				type = "function",
				description = "",
				args = "(Player player, int capacity)",
				returns = "void"
			},
			-- void command.playerupdateexperience(Player player, ulong experience, ushort level, byte levelPercent)
			playerupdateexperience = {
				type = "function",
				description = "",
				args = "(Player player, ulong experience, ushort level, byte levelPercent)",
				returns = "void"
			},
			-- void command.playerupdatemana(Player player, int mana)
			playerupdatemana = {
				type = "function",
				description = "",
				args = "(Player player, int mana)",
				returns = "void"
			},
			-- void command.playerupdatesoul(Player player, int soul)
			playerupdatesoul = {
				type = "function",
				description = "",
				args = "(Player player, int soul)",
				returns = "void"
			},
			-- void command.playerupdatestamina(Player player, int stamina)
			playerupdatestamina = {
				type = "function",
				description = "",
				args = "(Player player, int stamina)",
				returns = "void"
			},
			-- void command.playerwhisper(Player player, string message)
			playerwhisper = {
				type = "function",
				description = "",
				args = "(Player player, string message)",
				returns = "void"
			},
			-- void command.playeryell(Player player, string message)
			playeryell = {
				type = "function",
				description = "",
				args = "(Player player, string message)",
				returns = "void"
			},
			-- (bool, Addon) command.playergetoutfit(Player player, ushort outfitId)
			playergetoutfit = {
				type = "function",
				description = "",
				args = "(Player player, ushort outfitId)",
				returns = "(bool, Addon)"
			},
			-- void command.playersetoutfit(Player player, ushort outfitId, Addon addon)
			playersetoutfit = {
				type = "function",
				description = "",
				args = "(Player player, ushort outfitId, Addon addon)",
				returns = "void"
			},
			-- void command.playerremoveoutfit(Player player, ushort outfitId)
			playerremoveoutfit = {
				type = "function",
				description = "",
				args = "(Player player, ushort outfitId)",
				returns = "void"
			},
			-- (bool, int) command.playergetstorage(Player player, int key)
			playergetstorage = {
				type = "function",
				description = "",
				args = "(Player player, int key)",
				returns = "(bool, int)"
			},
			-- void command.playersetstorage(Player player, int key, int value)
			playersetstorage = {
				type = "function",
				description = "",
				args = "(Player player, int key, int value)",
				returns = "void"
			},
			-- void command.playerremovestorage(Player player, int key)
			playerremovestorage = {
				type = "function",
				description = "",
				args = "(Player player, int key)",
				returns = "void"
			},
			-- string[] command.playergetachievements(Player player)
			playergetachievements = {
				type = "function",
				description = "",
				args = "(Player player)",
				returns = "string[]"
			},
			-- bool command.playerhasachievement(Player player, string name)
			playerhasachievement = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "bool"
			},
			-- void command.playersetachievement(Player player, string name)
			playersetachievement = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "void"
			},
			-- void command.playerremoveachievement(Player player, string name)
			playerremoveachievement = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "void"
			},
			-- string[] command.playergetspells(Player player)
			playergetspells = {
				type = "function",
				description = "",
				args = "(Player player)",
				returns = "string[]"
			},
			-- bool command.playerhasspell(Player player, string name)
			playerhasspell = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "bool"
			},
			-- void command.playersetspell(Player player, string name)
			playersetspell = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "void"
			},
			-- void command.playerremovespell(Player player, string name)
			playerremovespell = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "void"
			},
			-- string[] command.playergetblesses(Player player)
			playergetblesses = {
				type = "function",
				description = "",
				args = "(Player player)",
				returns = "string[]"
			},
			-- bool command.playerhasbless(Player player, string name)
			playerhasbless = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "bool"
			},
			-- void command.playersetbless(Player player, string name)
			playersetbless = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "void"
			},
			-- void command.playerremovebless(Player player, string name)
			playerremovebless = {
				type = "function",
				description = "",
				args = "(Player player, string name)",
				returns = "void"
			},
			-- void command.playerstopwalk(Player player)
			playerstopwalk = {
				type = "function",
				description = "",
				args = "(Player player)",
				returns = "void"
			},
			-- void command.showanimatedtext(Position position, AnimatedTextColor animatedTextColor, string message)
			-- void command.showanimatedtext(IContent content, AnimatedTextColor animatedTextColor, string message)
			showanimatedtext = {
				type = "function",
				description = "",
				args = "(Position position, AnimatedTextColor animatedTextColor, string message)",
				returns = "void"
			},
			-- void command.showmagiceffect(Position position, MagicEffectType magicEffectType)
			-- void command.showmagiceffect(IContent content, MagicEffectType magicEffectType)
			showmagiceffect = {
				type = "function",
				description = "",
				args = "(Position position, MagicEffectType magicEffectType)",
				returns = "void"
			},
			-- void command.showprojectile(IContent fromContent, IContent toContent, ProjectileType projectileType)
			-- void command.showprojectile(IContent fromContent, Position toPosition, ProjectileType projectileType)
			-- void command.showprojectile(Position fromPosition, IContent toContent, ProjectileType projectileType)
			-- void command.showprojectile(Position fromPosition, Position toPosition, ProjectileType projectileType)
			showprojectile = {
				type = "function",
				description = "",
				args = "(Position fromPosition, Position toPosition, ProjectileType projectileType)",
				returns = "void"
			},
			-- void command.showtext(Creature creature, TalkType talkType, string message)
			showtext = {
				type = "function",
				description = "",
				args = "(Creature creature, TalkType talkType, string message)",
				returns = "void"
			},
			-- void command.showwindowtext(Player player, TextColor textColor, string message)
			showwindowtext = {
				type = "function",
				description = "",
				args = "(Player player, TextColor textColor, string message)",
				returns = "void"
			},
			-- void command.splashitemupdatefluidtype(SplashItem splashItem, FluidType fluidType)
			splashitemupdatefluidtype = {
				type = "function",
				description = "",
				args = "(SplashItem splashItem, FluidType fluidType)",
				returns = "void"
			},
			-- void command.stackableitemupdatecount(StackableItem stackableItem, byte count)
			stackableitemupdatecount = {
				type = "function",
				description = "",
				args = "(StackableItem stackableItem, byte count)",
				returns = "void"
			},
			-- void command.tileaddcreature(Tile tile, Creature creature)
			tileaddcreature = {
				type = "function",
				description = "",
				args = "(Tile tile, Creature creature)",
				returns = "void"
			},
			-- void command.tileadditem(Tile tile, Item item)
			tileadditem = {
				type = "function",
				description = "",
				args = "(Tile tile, Item item)",
				returns = "void"
			},
			-- Item command.tilecreateitem(Tile tile, ushort openTibiaId, byte typeCount)
			tilecreateitem = {
				type = "function",
				description = "",
				args = "(Tile tile, ushort openTibiaId, byte typeCount)",
				returns = "Item"
			},
			-- void command.tilecreateitemorincrement(Tile tile, ushort openTibiaId, byte typeCount)
			tilecreateitemorincrement = {
				type = "function",
				description = "",
				args = "(Tile tile, ushort openTibiaId, byte typeCount)",
				returns = "void"
			},
			-- Monster command.tilecreatemonster(Tile tile, string name)
			tilecreatemonster = {
				type = "function",
				description = "",
				args = "(Tile tile, string name)",
				returns = "Monster"
			},
			-- Npc command.tilecreatenpc(Tile tile, string name)
			tilecreatenpc = {
				type = "function",
				description = "",
				args = "(Tile tile, string name)",
				returns = "Npc"
			},
			-- void command.tileremovecreature(Tile tile, Creature creature)
			tileremovecreature = {
				type = "function",
				description = "",
				args = "(Tile tile, Creature creature)",
				returns = "void"
			},
			-- void command.tileremoveitem(Tile tile, Item item)
			tileremoveitem = {
				type = "function",
				description = "",
				args = "(Tile tile, Item item)",
				returns = "void"
			},
			-- void command.tilereplaceitem(Tile tile, Item fromItem, Item toItem)
			tilereplaceitem = {
				type = "function",
				description = "",
				args = "(Tile tile, Item fromItem, Item toItem)",
				returns = "void"
			},
			-- Town command.mapgettownbyname(string name)
			mapgettownbyname = {
				type = "function",
				description = "",
				args = "(string name)",
				returns = "Town"
			},
			-- Waypoint command.mapgetwaypointbyname(string name)
			mapgetwaypointbyname = {
				type = "function",
				description = "",
				args = "(string name)",
				returns = "Waypoint"
			},
			-- House command.mapgethousebyname(string name)
			mapgethousebyname = {
				type = "function",
				description = "",
				args = "(string name)",
				returns = "House"
			},
			-- Tile command.mapgettile(Position position)
			mapgettile = {
				type = "function",
				description = "",
				args = "(Position position)",
				returns = "Tile"
			},
			-- Player[] command.mapgetobserversoftypeplayer(Position position)
			mapgetobserversoftypeplayer = {
				type = "function",
				description = "",
				args = "(Position position)",
				returns = "Player[]"
			},
			-- Player[] command.gameobjectsgetplayers()
			gameobjectsgetplayers = {
				type = "function",
				description = "",
				args = "()",
				returns = "Player[]"
			},
			-- Player command.gameobjectsgetplayerbyname(string name)
			gameobjectsgetplayerbyname = {
				type = "function",
				description = "",
				args = "(string name)",
				returns = "Player"
			}
		}
	}
}