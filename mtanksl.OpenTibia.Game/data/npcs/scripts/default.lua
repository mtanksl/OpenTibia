local say = topic:new()
say:add("name", "My name is {npcname}.")

local handler = npchandler:new( {
    greet = "Hello {playername}.",
    busy = "I'll talk to you soon {playername}.",
    say = say,
    farewell = "Bye {playername}.",
    dismiss = "Bye."
} )

function shouldgreet(npc, player, message) return handler:shouldgreet(npc, player, message) end
function shouldfarewell(npc, player, message) return handler:shouldfarewell(npc, player, message) end
function ongreet(npc, player) handler:ongreet(npc, player) end
function onbusy(npc, player) handler:onbusy(npc, player) end
function onsay(npc, player, message) handler:onsay(npc, player, message) end
function onfarewell(npc, player) handler:onfarewell(npc, player) end
function ondismiss(npc, player) handler:ondismiss(npc, player) end