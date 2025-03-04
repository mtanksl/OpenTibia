dofile(getfullpath("data/plugins/npcs/lib.lua"))

function registeractionsplayerrotateitem(id, uniqueid, actionid, opentibiaid, onrotateitem)
    command.registerplugin("actions", {
	    type = "PlayerRotateItem",
	    id = id, 
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
	    onrotateitem = onrotateitem
    } )
end

function registeractionsplayeruseitem(id, uniqueid, actionid, opentibiaid, onuseitem)
    command.registerplugin("actions", {
	    type = "PlayerUseItem",
	    id = id, 
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
	    onuseitem = onuseitem
    } )
end

function registeractionsplayeruseitemwithitem(id, uniqueid, actionid, opentibiaid, allowfaruse, onuseitemwithitem)
    command.registerplugin("actions", {
	    type = "PlayerUseItemWithItem",
	    id = id, 
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        allowfaruse = allowfaruse,
	    onuseitemwithitem = onuseitemwithitem
    } )
end

function registeractionsplayeruseitemwithcreature(id, uniqueid, actionid, opentibiaid, allowfaruse, onuseitemwithcreature)
    command.registerplugin("actions", {
	    type = "PlayerUseItemWithCreature",
	    id = id, 
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        allowfaruse = allowfaruse,
	    onuseitemwithcreature = onuseitemwithcreature
    } )
end

function registeractionsplayermoveitem(id, uniqueid, actionid, opentibiaid, onmoveitem)
    command.registerplugin("actions", {
	    type = "PlayerMoveItem",
	    id = id, 
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
	    onmoveitem = onmoveitem
    } )
end

function registeractionsplayermovecreature(name, onmovecreature)
    command.registerplugin("actions", {
	    type = "PlayerMoveCreature",
	    name = name,
	    onmovecreature = onmovecreature
    } )
end

function registermovementscreaturestepin(id, uniqueid, actionid, opentibiaid, onsteppingin, onstepin)
    command.registerplugin("movements", {
	    type = "CreatureStepIn",
	    id = id, 
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        onsteppingin = onsteppingin,
	    onstepin = onstepin
    } )
end

function registermovementscreaturestepout(id, uniqueid, actionid, opentibiaid, onsteppingout, onstepout)
    command.registerplugin("movements", {
	    type = "CreatureStepOut",
	    id = id, 
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        onsteppingout = onsteppingout,
	    onstepout = onstepout
    } )
end

function registermovementsinventoryequip(id, uniqueid, actionid, opentibiaid, onequipping, onequip)
    command.registerplugin("movements", {
	    type = "InventoryEquip",
	    id = id, 
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        onequipping = onequipping,
	    onequip = onequip
    } )
end

function registermovementsinventorydeequip(id, uniqueid, actionid, opentibiaid, ondeequipping, ondeequip)
    command.registerplugin("movements", {
	    type = "InventoryDeEquip",
	    id = id, 
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        ondeequipping = ondeequipping,
	    ondeequip = ondeequip
    } )
end

function registertalkactionsplayersay(message, onsay)
    command.registerplugin("talkactions", {
	    type = "PlayerSay",
	    message = message,
	    onsay = onsay
    } )
end

function registercreaturescriptsplayerlogin(onlogin)
    command.registerplugin("creaturescripts", {
	    type = "PlayerLogin",
	    onlogin = onlogin
    } )
end

function registercreaturescriptsplayerlogout(onlogout)
    command.registerplugin("creaturescripts", {
	    type = "PlayerLogout",
	    onlogout = onlogout
    } )
end

function registercreaturescriptsplayeradvancelevel(onadvancelevel)
    command.registerplugin("creaturescripts", {
	    type = "PlayerAdvanceLevel",
	    onadvancelevel = onadvancelevel
    } )
end

function registercreaturescriptsplayeradvanceskill(onadvanceskill)
    command.registerplugin("creaturescripts", {
	    type = "PlayerAdvanceSkill",
	    onadvanceskill = onadvanceskill
    } )
end

function registercreaturescriptscreaturedeath(ondeath)
    command.registerplugin("creaturescripts", {
	    type = "CreatureDeath",
	    ondeath = ondeath
    } )
end

function registercreaturescriptscreaturekill(onkill)
    command.registerplugin("creaturescripts", {
	    type = "CreatureKill",
	    onkill = onkill
    } )
end

function registercreaturescriptsplayerearnachievement(onearnachievement)
    command.registerplugin("creaturescripts", {
	    type = "PlayerEarnAchievement",
	    onearnachievement = onearnachievement
    } )
end

function registerglobaleventsserverstartup(onstartup)
    command.registerplugin("globalevents", {
	    type = "ServerStartup",
	    onstartup = onstartup
    } )
end

function registerglobaleventsservershutdown(onshutdown)
    command.registerplugin("globalevents", {
	    type = "ServerShutdown",
	    onshutdown = onshutdown
    } )
end

function registerglobaleventsserversave(onsave)
    command.registerplugin("globalevents", {
	    type = "ServerSave",
	    onsave = onsave
    } )
end

function registerglobaleventsserverrecord(onrecord)
    command.registerplugin("globalevents", {
	    type = "ServerRecord",
	    onrecord = onrecord
    } )
end

function registernpcsdialogue(name, handler)
    command.registerplugin("npcs", { 
        type = "Dialogue", 
        name = name,
        shouldgreet = function(npc, player, message) return handler:shouldgreet(npc, player, message) end,
        shouldfarewell = function(npc, player, message) return handler:shouldfarewell(npc, player, message) end,
        ongreet = function(npc, player) handler:ongreet(npc, player) end,
        onbusy = function(npc, player) handler:onbusy(npc, player) end,
        onsay = function(npc, player, message) handler:onsay(npc, player, message) end,
        onbuy = function(npc, player, item, type, count, price, ignoreCapacity, buyWithBackpacks) handler:onbuy(npc, player, item, type, count, price, ignoreCapacity, buyWithBackpacks) end,
        onsell = function(npc, player, item, type, count, price, keepEquipped) handler:onsell(npc, player, item, type, count, price, keepEquipped) end,
        onclosenpctrade = function(npc, player) handler:onclosenpctrade(npc, player) end,
        onfarewell = function(npc, player) handler:onfarewell(npc, player) end,
        ondisappear = function(npc, player) handler:ondisappear(npc, player) end,
        onenqueue = function(npc, player) handler:onenqueue(npc, player) end,
        ondequeue = function(npc, player) handler:ondequeue(npc, player) end
    } )
end

function registeritemsitemcreation(opentibiaid, onstart, onstop)
	command.registerplugin("items", {
		type = "ItemCreation",
		opentibiaid = opentibiaid,
		onstart = onstart,
		onstop = onstop,
	} )
end

function registermonstersmonstercreation(name, onstart, onstop)
	command.registerplugin("monsters", {
		type = "MonsterCreation",
		name = name,
		onstart = onstart,
		onstop = onstop,
	} )
end

function registernpcsnpccreation(name, onstart, onstop)
	command.registerplugin("npcs", {
		type = "NpcCreation",
		name = name,
		onstart = onstart,
		onstop = onstop,
	} )
end

function registerplayersplayercreation(name, onstart, onstop)
	command.registerplugin("players", {
		type = "PlayerCreation",
		name = name,
		onstart = onstart,
		onstop = onstop,
	} )
end

function registerspell(words, name, group, cooldown, groupcooldown, level, mana, soul, premium, vocations, requirestarget, oncasting, oncast)
    command.registerplugin("spells", {
        words = words, 
        name = name, 
        group = group, 
        cooldown = cooldown, 
        groupcooldown = groupcooldown, 
        level = level, 
        mana = mana, 
        soul = soul, 
        premium = premium, 
        vocations = vocations, 
        requirestarget = requirestarget, 
        oncasting = oncasting, 
        oncast = oncast
    } )
end

function registerrune(opentibiaid, name, group, groupcooldown, level, magiclevel, vocations, requirestarget, onusingrune, onuserune)
    command.registerplugin("runes", {
        opentibiaid = opentibiaid, 
        name = name, 
        group = group, 
        groupcooldown = groupcooldown, 
        level = level, 
        magiclevel = magiclevel,
        vocations = vocations, 
        requirestarget = requirestarget,
        onusingrune = onusingrune,
        onuserune = onuserune
    } )
end

function registerweapon(opentibiaid, level, mana, vocations, onusingweapon, onuseweapon)
    command.registerplugin("weapons", {
        opentibiaid = opentibiaid,
        level = level,
        mana = mana,
        vocations = vocations,
        onusingweapon = onusingweapon,
        onuseweapon = onuseweapon
    } )
end

function registerammunition(opentibiaid, level, onusingammunition, onuseammunition)
    command.registerplugin("ammunitions", {
        opentibiaid = opentibiaid,
        level = level,
        onusingammunition = onusingammunition,
        onuseammunition = onuseammunition
    } )
end

function registerraid(name, repeatable, interval, chance, enabled, onraid)
    command.registerplugin("raids", {
        name = name,
        repeatable = repeatable,
        interval = interval,
        chance = chance,
        enabled = enabled,
	    onraid = onraid
    } )
end

function registermonsterattack(name, onattacking, onattack)
    command.registerplugin("monsterattacks", {
        name = name,
        onattacking = onattacking,
        onattack = onattack
    } )
end