dofile(getfullpath("data/plugins/npcs/lib.lua"))

function registeractionsplayerrotateitem(opentibiaid, onrotateitem)
    registerplugin("actions", {
	    type = "PlayerRotateItem",
	    opentibiaid = opentibiaid, 
	    onrotateitem = onrotateitem
    } )
end

function registeractionsplayeruseitem(opentibiaid, onuseitem)
    registerplugin("actions", {
	    type = "PlayerUseItem",
	    opentibiaid = opentibiaid, 
	    onuseitem = onuseitem
    } )
end

function registeractionsplayeruseitemwithitem(opentibiaid, allowfaruse, onuseitemwithitem)
    registerplugin("actions", {
	    type = "PlayerUseItemWithItem",
	    opentibiaid = opentibiaid,
        allowfaruse = allowfaruse,
	    onuseitemwithitem = onuseitemwithitem
    } )
end

function registeractionsplayeruseitemwithcreature(opentibiaid, allowfaruse, onuseitemwithcreature)
    registerplugin("actions", {
	    type = "PlayerUseItemWithCreature",
	    opentibiaid = opentibiaid,
        allowfaruse = allowfaruse,
	    onuseitemwithcreature = onuseitemwithcreature
    } )
end

function registeractionsplayermoveitem(opentibiaid, onmoveitem)
    registerplugin("actions", {
	    type = "PlayerMoveItem",
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

function registermovementscreaturestepin(opentibiaid, onstepin)
    registerplugin("movements", {
	    type = "CreatureStepIn",
	    opentibiaid = opentibiaid,
	    onstepin = onstepin
    } )
end

function registermovementscreaturestepout(opentibiaid, onstepout)
    registerplugin("movements", {
	    type = "CreatureStepOut",
	    opentibiaid = opentibiaid,
	    onstepout = onstepout
    } )
end

function registermovementsinventoryequip(opentibiaid, onequip)
    registerplugin("movements", {
	    type = "InventoryEquip",
	    opentibiaid = opentibiaid,
	    onequip = onequip
    } )
end

function registermovementsinventorydeequip(opentibiaid, ondeequip)
    registerplugin("movements", {
	    type = "InventoryDeEquip",
	    opentibiaid = opentibiaid,
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