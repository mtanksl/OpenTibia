local say = topic:new()
say:add("name", "I am the illusterous Rachel, of course.")
say:add("job", "I am the head alchemist of Carlin. I keep the secret recipies of our ancestors. Besides, I am selling mana and life fluids, spellbooks, wands, rods and runes.")
say:add("rune", "I sell blank runes and spell runes.")
say:addtrade( {
    { article = "a", name = "mana fluid", plural = "mana fluids", item = 11396, type = 7, buyprice = 55 },
    { article = "a", name = "life fluid", plural = "life fluids", item = 11396, type = 10, buyprice = 60 },
    { article = "a", name = "spellbook", plural = "spellbooks", item = 2175, type = 0, buyprice = 150 },
    { article = "a", name = "blank rune", plural = "blank runes", item = 2260, type = 0, buyprice = 10 }
} )

local handler = npchandler:new( {
    greet = "Welcome [playername]! Whats your need?",
    busy = "Wait, [playername]! One after the other.",
    say = say,
    farewell = "Good bye, [playername].",
    disappear = "These impatient young brats!"
} )

registernpcsdialogue("Rachel", handler)