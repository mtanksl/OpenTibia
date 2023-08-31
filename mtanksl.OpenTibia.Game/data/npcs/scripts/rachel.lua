local say = topic:new()
say:add("name", "I am the illusterous Rachel, of course.")
say:add("job", "I am the head alchemist of Carlin. I keep the secret recipies of our ancestors. Besides, I am selling mana and life fluids, spellbooks, wands, rods and runes.")
say:add("wares", "I sell blank runes and spell runes.")
say:add("offer", "I sell blank runes and spell runes.")

local handler = npchandler:new( {
    greet = "Welcome {playername}! Whats your need?",
    busy = "Wait, {playername}! One after the other.",
    say = say,
    farewell = "Good bye, {playername}.",
    dismiss = "These impatient young brats!"
} )

function shouldgreet(npc, player, message) return handler:shouldgreet(npc, player, message) end
function shouldfarewell(npc, player, message) return handler:shouldfarewell(npc, player, message) end
function ongreet(npc, player) handler:ongreet(npc, player) end
function onbusy(npc, player) handler:onbusy(npc, player) end
function onsay(npc, player, message) handler:onsay(npc, player, message) end
function onfarewell(npc, player) handler:onfarewell(npc, player) end
function ondismiss(npc, player) handler:ondismiss(npc, player) end