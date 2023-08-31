local handler = npchandler:new( {
    responses = {
        ["greet"] = "Hello, {playername}! Feel free to ask me for help.",
        ["busy"] = "Please wait, {playername}. I already talk to someone!",
        ["say"] = {
            ["name"] = "My name is Cipfried.",
            ["job"] = "I am just a humble monk. Ask me if you need help or healing.",
            ["monk"] = "I sacrifice my life to serve the good gods of Tibia.",
            ["tibia"] = "That's where we are. The world of Tibia."
        },
        ["farewell"] = "Farewell, {playername}!",
        ["dismiss"] = "Well, bye then."
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