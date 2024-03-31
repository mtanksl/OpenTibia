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
say:addtrade( {
    { article = "a", name = "spear", plural = "spears", item = 2389, type = 1, buyprice = 10, sellprice = 2 },
    { article = "an", name = "axe", plural = "axes", item = 2386, type = 1, buyprice = 20, sellprice = 10 }
} )

local handler = npchandler:new( {
    greet = "Hello, hello, [playername]! Please come in, look, and buy!",
    busy = "I'll be with you in a moment, [playername].",
    say = say,
    farewell = "Bye, bye.",
    disappear = "Bye, bye."
} )

registernpcsdialogue("Al Dee", handler)