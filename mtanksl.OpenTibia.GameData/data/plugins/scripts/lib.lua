dofile(getfullpath("data/plugins/npcs/lib.lua"))

function registeractionsplayerrotateitem(uniqueid, actionid, opentibiaid, onrotateitem)
    registerplugin("actions", {
	    type = "PlayerRotateItem",
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
	    onrotateitem = onrotateitem
    } )
end

function registeractionsplayeruseitem(uniqueid, actionid, opentibiaid, onuseitem)
    registerplugin("actions", {
	    type = "PlayerUseItem",
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
	    onuseitem = onuseitem
    } )
end

function registeractionsplayeruseitemwithitem(uniqueid, actionid, opentibiaid, allowfaruse, onuseitemwithitem)
    registerplugin("actions", {
	    type = "PlayerUseItemWithItem",
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        allowfaruse = allowfaruse,
	    onuseitemwithitem = onuseitemwithitem
    } )
end

function registeractionsplayeruseitemwithcreature(uniqueid, actionid, opentibiaid, allowfaruse, onuseitemwithcreature)
    registerplugin("actions", {
	    type = "PlayerUseItemWithCreature",
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        allowfaruse = allowfaruse,
	    onuseitemwithcreature = onuseitemwithcreature
    } )
end

function registeractionsplayermoveitem(uniqueid, actionid, opentibiaid, onmoveitem)
    registerplugin("actions", {
	    type = "PlayerMoveItem",
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
	    onmoveitem = onmoveitem
    } )
end

function registeractionsplayermovecreature(name, onmovecreature)
    registerplugin("actions", {
	    type = "PlayerMoveCreature",
	    name = name,
	    onmovecreature = onmovecreature
    } )
end

function registermovementscreaturestepin(uniqueid, actionid, opentibiaid, onsteppingin, onstepin)
    registerplugin("movements", {
	    type = "CreatureStepIn",
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        onsteppingin = onsteppingin,
	    onstepin = onstepin
    } )
end

function registermovementscreaturestepout(uniqueid, actionid, opentibiaid, onsteppingout, onstepout)
    registerplugin("movements", {
	    type = "CreatureStepOut",
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        onsteppingout = onsteppingout,
	    onstepout = onstepout
    } )
end

function registermovementsinventoryequip(uniqueid, actionid, opentibiaid, onequipping, onequip)
    registerplugin("movements", {
	    type = "InventoryEquip",
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        onequipping = onequipping,
	    onequip = onequip
    } )
end

function registermovementsinventorydeequip(uniqueid, actionid, opentibiaid, ondeequipping, ondeequip)
    registerplugin("movements", {
	    type = "InventoryDeEquip",
	    uniqueid = uniqueid, 
	    actionid = actionid, 
	    opentibiaid = opentibiaid, 
        ondeequipping = ondeequipping,
	    ondeequip = ondeequip
    } )
end

function registertalkactionsplayersay(message, onsay)
    registerplugin("talkactions", {
	    type = "PlayerSay",
	    message = message,
	    onsay = onsay
    } )
end

function registercreaturescriptsplayerlogin(onlogin)
    registerplugin("creaturescripts", {
	    type = "PlayerLogin",
	    onlogin = onlogin
    } )
end

function registercreaturescriptsplayerlogout(onlogout)
    registerplugin("creaturescripts", {
	    type = "PlayerLogout",
	    onlogout = onlogout
    } )
end

function registercreaturescriptsplayeradvancelevel(onadvancelevel)
    registerplugin("creaturescripts", {
	    type = "PlayerAdvanceLevel",
	    onadvancelevel = onadvancelevel
    } )
end

function registercreaturescriptsplayeradvanceskill(onadvanceskill)
    registerplugin("creaturescripts", {
	    type = "PlayerAdvanceSkill",
	    onadvanceskill = onadvanceskill
    } )
end

function registercreaturescriptscreaturedeath(ondeath)
    registerplugin("creaturescripts", {
	    type = "CreatureDeath",
	    ondeath = ondeath
    } )
end

function registercreaturescriptscreaturekill(onkill)
    registerplugin("creaturescripts", {
	    type = "CreatureKill",
	    onkill = onkill
    } )
end

function registercreaturescriptsplayerearnachievement(onearnachievement)
    registerplugin("creaturescripts", {
	    type = "PlayerEarnAchievement",
	    onearnachievement = onearnachievement
    } )
end

function registerglobaleventsserverstartup(onstartup)
    registerplugin("globalevents", {
	    type = "ServerStartup",
	    onstartup = onstartup
    } )
end

function registerglobaleventsservershutdown(onshutdown)
    registerplugin("globalevents", {
	    type = "ServerShutdown",
	    onshutdown = onshutdown
    } )
end

function registerglobaleventsserversave(onsave)
    registerplugin("globalevents", {
	    type = "ServerSave",
	    onsave = onsave
    } )
end

function registerglobaleventsserverrecord(onrecord)
    registerplugin("globalevents", {
	    type = "ServerRecord",
	    onrecord = onrecord
    } )
end

function registernpcsdialogue(name, handler)
    registerplugin("npcs", { 
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
	registerplugin("items", {
		type = "ItemCreation",
		opentibiaid = opentibiaid,
		onstart = onstart,
		onstop = onstop,
	} )
end

function registermonstersmonstercreation(name, onstart, onstop)
	registerplugin("monsters", {
		type = "MonsterCreation",
		name = name,
		onstart = onstart,
		onstop = onstop,
	} )
end

function registernpcsnpccreation(name, onstart, onstop)
	registerplugin("npcs", {
		type = "NpcCreation",
		name = name,
		onstart = onstart,
		onstop = onstop,
	} )
end

function registerplayersplayercreation(name, onstart, onstop)
	registerplugin("players", {
		type = "PlayerCreation",
		name = name,
		onstart = onstart,
		onstop = onstop,
	} )
end

function registerspell(words, name, group, cooldown, groupcooldown, level, mana, soul, premium, vocations, requirestarget, oncasting, oncast)
    registerplugin("spells", {
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
    registerplugin("runes", {
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
    registerplugin("weapons", {
        opentibiaid = opentibiaid,
        level = level,
        mana = mana,
        vocations = vocations,
        onusingweapon = onusingweapon,
        onuseweapon = onuseweapon
    } )
end

function registerammunition(opentibiaid, level, onusingammunition, onuseammunition)
    registerplugin("ammunitions", {
        opentibiaid = opentibiaid,
        level = level,
        onusingammunition = onusingammunition,
        onuseammunition = onuseammunition
    } )
end

function registerraid(name, repeatable, interval, chance, enabled, onraid)
    registerplugin("raids", {
        name = name,
        repeatable = repeatable,
        interval = interval,
        chance = chance,
        enabled = enabled,
	    onraid = onraid
    } )
end

function registermonsterattack(name, onattacking, onattack)
    registerplugin("monsterattacks", {
        name = name,
        onattacking = onattacking,
        onattack = onattack
    } )
end