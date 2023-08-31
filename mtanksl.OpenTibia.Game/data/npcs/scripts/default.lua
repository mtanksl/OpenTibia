local handler = npchandler:new()

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