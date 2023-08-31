local handler = npchandler:new( {
    responses = {
        ["greet"] = "Welcome {playername}! Whats your need?",
        ["busy"] = "Wait, {playername}! One after the other.",
        ["say"] = {
            ["name"] = "I am the illusterous Rachel, of course.",
            ["job"] = "I am the head alchemist of Carlin. I keep the secret recipies of our ancestors. Besides, I am selling mana and life fluids, spellbooks, wands, rods and runes.",
            ["wares"] = "I sell blank runes and spell runes.",
            ["offer"] = "I sell blank runes and spell runes."
        },
        ["farewell"] = "Good bye,{playername}.",
        ["dismiss"] = "These impatient young brats!"
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