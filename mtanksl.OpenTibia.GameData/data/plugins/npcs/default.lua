local say = topic:new()
say:add("name", "My name is {npcname}.")

local handler = npchandler:new( {
    greet = "Hello {playername}.",
    busy = "I'll talk to you soon {playername}.",
    say = say,
    farewell = "Bye {playername}.",
    disappear = "Bye."
} )

function shouldgreet(npc, player, message) return handler:shouldgreet(npc, player, message) end
function shouldfarewell(npc, player, message) return handler:shouldfarewell(npc, player, message) end
function ongreet(npc, player) handler:ongreet(npc, player) end
function onbusy(npc, player) handler:onbusy(npc, player) end
function onsay(npc, player, message) handler:onsay(npc, player, message) end
function onbuy(npc, player, item, type, count, price, ignoreCapacity, buyWithBackpacks) handler:onbuy(npc, player, item, type, count, price, ignoreCapacity, buyWithBackpacks) end
function onsell(npc, player, item, type, count, price, keepEquipped) handler:onsell(npc, player, item, type, count, price, keepEquipped) end
function onclosenpctrade(npc, player) handler:onclosenpctrade(npc, player) end
function onfarewell(npc, player) handler:onfarewell(npc, player) end
function ondisappear(npc, player) handler:ondisappear(npc, player) end
function onenqueue(npc, player) handler:onenqueue(npc, player) end
function ondequeue(npc, player) handler:ondequeue(npc, player) end