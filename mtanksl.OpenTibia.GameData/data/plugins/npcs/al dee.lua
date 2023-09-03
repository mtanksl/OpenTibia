local say = topic:new()
say:add("name", "My name is Al Dee, but you can call me Al. Do you want to buy something?")
say:add("job", "I am a merchant. What can I do for you?")
say:add("wares", "I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?")
say:add("offer", "I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?")
say:add("weapon", "I sell spears, rapiers, sabres, daggers, hand axes, axes, and short swords. Just tell me what you want to buy.")
say:add("shield", "I sell wooden shields and studded shields. Just tell me what you want to buy.")
say:add("armor", "I sell jackets, coats, doublets, leather armor, and leather legs. Just tell me what you want to buy.")
say:add("helmet", "I sell leather helmets, studded helmets, and chain helmets. Just tell me what you want to buy.")
say:add("equipment", "I sell torches, bags, scrolls, shovels, picks, backpacks, sickles, scythes, ropes, fishing rods and sixpacks of worms. Just tell me what you want to buy.")
say:addsell( {
    questionitems = "Do you want to sell {count} {plural} for {price} gold?",
    questionitem = "Do you want to sell {article} {name} for {price} gold?",
    yes = "Ok. Here is your money.",
    notenoughitems = "Sorry, you do not have so many.",
    notenoughitem = "Sorry, you do not have one.",
    no = "Maybe next time."
}, {
    { article = "a", name = "spear", plural = "spears", item = 2389, type = 1, price = 2 },
    { article = "an", name = "axe", plural = "axes", item = 2386, type = 1, price = 10 }
} )
say:addbuy( {
    questionitems = "Do you want to buy {count} {plural} for {price} gold?",
    questionitem = "Do you want to buy {article} {name} for {price} gold?",
    yes = "Thank you. Here it is.",
    notenoughtgold = "Sorry, you do not have enough gold.",
    no = "Maybe you will buy it another time."
}, {
    { article = "a", name = "spear", plural = "spears", item = 2389, type = 1, price = 10 },
    { article = "an", name = "axe", plural = "axes", item = 2386, type = 1, price = 20 }
} )

local handler = npchandler:new( {
    greet = "Hello, hello, {playername}! Please come in, look, and buy!",
    busy = "I'll be with you in a moment, {playername}.",
    say = say,
    farewell = "Bye, bye.",
    dismiss = "Bye, bye."
} )

function shouldgreet(npc, player, message) return handler:shouldgreet(npc, player, message) end
function shouldfarewell(npc, player, message) return handler:shouldfarewell(npc, player, message) end
function ongreet(npc, player) handler:ongreet(npc, player) end
function onbusy(npc, player) handler:onbusy(npc, player) end
function onsay(npc, player, message) handler:onsay(npc, player, message) end
function onfarewell(npc, player) handler:onfarewell(npc, player) end
function ondismiss(npc, player) handler:ondismiss(npc, player) end