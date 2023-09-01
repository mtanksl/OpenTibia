local say = topic:new()
local sell = topic:new(say)
local buy = topic:new(say)

say:add("name", "My name is Al Dee, but you can call me Al. Do you want to buy something?")
say:add("job", "I am a merchant. What can I do for you?")
say:add("wares", "I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?")
say:add("offer", "I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?")
say:add("sell (%d+) spear", function(npc, player, message, captures, parameters) local count = math.max(1, math.min(100, tonumber(captures[1] ) ) ) return topiccallback:new( { item = 2389, type = 1, count = count, price = 2 * count, topic = sell }, "Do you want to sell this garbage? I give you {price} gold, ok?") end)
say:add("sell spear", topiccallback:new( { item = 2389, type = 1, count = 1, price = 2, topic = sell }, "Do you want to sell this garbage? I give you {price} gold, ok?") )
say:add("(%d+) spear", function(npc, player, message, captures, parameters) local count = math.max(1, math.min(100, tonumber(captures[1] ) ) ) return topiccallback:new( { item = 2389, type = 1, count = count, price = 10 * count, topic = buy }, "Do you want to buy {count} spear for {price} gold?") end)
say:add("spear", topiccallback:new( { item = 2389, type = 1, count = 1, price = 10, topic = buy }, "Do you want to buy a spear for {price} gold?" ) )

sell:add("yes", function(npc, player, message, captures, parameters) 
    if npcremoveitem(player, parameters.item, parameters.type, parameters.count) then
        npcaddmoney(player, parameters.price)
        return topiccallback:new( { topic = say }, "Ok. Here is your money.")
    end
    if parameters.count > 1 then
        return topiccallback:new( { topic = say }, "Sorry, you do not have so many.")
    end
    return topiccallback:new( { topic = say }, "Sorry, you do not have one.")        
end)

sell:add("", topiccallback:new( { topic = say }, "Maybe next time.") )

buy:add("yes", function(npc, player, message, captures, parameters) 
    if npcdeletemoney(player, parameters.price) then
        npcadditem(player, parameters.item, parameters.type, parameters.count)
        return topiccallback:new( { topic = say }, "Thank you. Here it is.")
    end
    return topiccallback:new( { topic = say }, "Sorry, you do not have enough gold.")
end)

buy:add("", topiccallback:new( { topic = say }, "Maybe you will buy it another time.") )

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