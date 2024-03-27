local say = topic:new()
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
    { article = "a", name = "mana fluid", plural = "mana fluids", item = 11396, type = 7, price = 55 },
    { article = "a", name = "life fluid", plural = "life fluids", item = 11396, type = 10, price = 60 },
    { article = "a", name = "spellbook", plural = "spellbooks", item = 2175, type = 1, price = 150 },
    { article = "a", name = "blank rune", plural = "blank runes", item = 2260, type = 1, price = 10 }
} )

local handler = npchandler:new( {
    greet = "Welcome {playername}! Whats your need?",
    busy = "Wait, {playername}! One after the other.",
    say = say,
    farewell = "Good bye, {playername}.",
    disappear = "These impatient young brats!"
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