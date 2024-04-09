dofile(getfullpath("data/plugins/npcs/lib.lua"))

function registeractionsplayerrotateitem(opentibiaid, handler)
    registerplugin("actions", {
	    type = "PlayerRotateItem",
	    opentibiaid = opentibiaid, 
	    onrotateitem = handler
    } )
end

function registeractionsplayeruseitem(opentibiaid, handler)
    registerplugin("actions", {
	    type = "PlayerUseItem",
	    opentibiaid = opentibiaid, 
	    onuseitem = handler
    } )
end

function registeractionsplayeruseitemwithitem(opentibiaid, allowfaruse, handler)
    registerplugin("actions", {
	    type = "PlayerUseItemWithItem",
	    opentibiaid = opentibiaid,
        allowfaruse = allowfaruse,
	    onuseitemwithitem = handler
    } )
end

function registeractionsplayeruseitemwithcreature(opentibiaid, allowfaruse, handler)
    registerplugin("actions", {
	    type = "PlayerUseItemWithCreature",
	    opentibiaid = opentibiaid,
        allowfaruse = allowfaruse,
	    onuseitemwithcreature = handler
    } )
end

function registeractionsplayermoveitem(opentibiaid, handler)
    registerplugin("actions", {
	    type = "PlayerMoveItem",
	    opentibiaid = opentibiaid,
	    onmoveitem = handler
    } )
end

function registeractionsplayermovecreature(name, handler)
    registerplugin("actions", {
	    type = "PlayerMoveCreature",
	    name = name,
	    onmovecreature = handler
    } )
end

function registermovementscreaturestepin(opentibiaid, handler)
    registerplugin("movements", {
	    type = "CreatureStepIn",
	    opentibiaid = opentibiaid,
	    onstepin = handler
    } )
end

function registermovementscreaturestepout(opentibiaid, handler)
    registerplugin("movements", {
	    type = "CreatureStepOut",
	    opentibiaid = opentibiaid,
	    onstepout = handler
    } )
end

function registermovementsinventoryequip(opentibiaid, handler)
    registerplugin("movements", {
	    type = "InventoryEquip",
	    opentibiaid = opentibiaid,
	    onequip = handler
    } )
end

function registermovementsinventorydeequip(opentibiaid, handler)
    registerplugin("movements", {
	    type = "InventoryDeEquip",
	    opentibiaid = opentibiaid,
	    ondeequip = handler
    } )
end

function registertalkactionsplayersay(message, handler)
    registerplugin("talkactions", {
	    type = "PlayerSay",
	    message = message,
	    onsay = handler
    } )
end

function registercreaturescriptsplayerlogin(handler)
    registerplugin("creaturescripts", {
	    type = "PlayerLogin",
	    onlogin = handler
    } )
end

function registercreaturescriptsplayerlogout(handler)
    registerplugin("creaturescripts", {
	    type = "PlayerLogout",
	    onlogout = handler
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