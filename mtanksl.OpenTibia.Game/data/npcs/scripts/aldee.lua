local handler = npchandler:new( {
    responses = {
        ["greet"] = "Hello, hello, {playername}! Please come in, look, and buy!",
        ["busy"] = "I'll be with you in a moment, {playername}.",
        ["say"] = {
            ["name"] = "My name is Al Dee, but you can call me Al. Do you want to buy something?",
            ["job"] = "I am a merchant. What can I do for you?",
            ["wares"] = "I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?",
            ["offer"] = "I sell weapons, shields, armor, helmets, and equipment. For what do you want to ask?"
        },
        ["farewell"] = "Bye, bye.",
        ["dismiss"] = "Bye, bye."
    }
} )

function shouldgreet(npc, player, message)
    return handler:shouldgreet(npc, player, message)
end

function shouldfarewell(npc, player, message)
    return handler:shouldfarewell(npc, player, message)
end

function greet(npc, player)
    handler:greet(npc, player)
end

function busy(npc, player)
    handler:busy(npc, player)
end

function say(npc, player, message)
    handler:say(npc, player, message)
end

function farewell(npc, player)
   handler:farewell(npc, player)
end

function dismiss(npc, player)
    handler:dismiss(npc, player)
end