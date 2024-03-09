local say = topic:new()
local confirm = topic:new(say)
say:add("name", "I am the illusterous Rachel, of course.")
say:add("job", "I am the head alchemist of Carlin. I keep the secret recipies of our ancestors. Besides, I am selling mana and life fluids, spellbooks, wands, rods and runes.")
say:add("rune", "I sell blank runes and spell runes.")
say:addbuy( {
    questionitems = "Do you want to buy {count} {plural} for {price} gold?",
    questionitem = "Do you want to buy {article} {name} for {price} gold?",
    yes = "Here you are.",
    notenoughtgold = "Come back when you have enough money.",
    no = "Hmm, maybe next time."
}, {
    { article = "a", name = "spellbook", plural = "spellbooks", item = 2175, type = 1, price = 150 },
    { article = "a", name = "blank rune", plural = "blank runes", item = 2260, type = 1, price = 10 },
    { article = "a", name = "mana fluid", plural = "mana fluids", item = 11396, type = 7, price = 55 },
    { article = "a", name = "life fluid", plural = "life fluids", item = 11396, type = 10, price = 60 }
} )
say:add("vial",  function(npc, player, message, captures, parameters) 
    local count = math.max(1, command.playercountitem(player, 11396, 0) )
    return topiccallback:new( { item = 11396, type = 0, count = count, price = 5 * count, topic = confirm }, "I will pay you 5 gold for every empty vial. Ok?") 
end)
confirm:add("yes", function(npc, player, message, captures, parameters) 
    if command.playerremoveitem(player, parameters.item, parameters.type, parameters.count) then
        command.playeraddmoney(player, parameters.price)
        return topiccallback:new( { topic = say }, "Here you are... {price} gold.")
    end
    return topiccallback:new( { topic = say }, "You don't have any empty vials.")        
end)
confirm:add("", function(npc, player, message, captures, parameters) 
    return topiccallback:new( { topic = say }, "Hmm, but please keep Tibia litter free.") 
end)

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