npchandler = {}

npchandler.mt = {
    __index = npchandler
}

function npchandler:new(o)
    o = o or {}
    setmetatable(o, self.mt)
    return o
end

function npchandler:shouldgreet(npc, player, message)
    return message == "hi" or message == "hello"
end

function npchandler:shouldfarewell(npc, player, message)
    return message == "bye" or message == "farewell"
end

function npchandler:greet(npc, player)
    if self.responses["greet"] then
        npcsay(self:replace(npc, player, self.responses["greet"] ) )
    else
        npcsay(self:replace(npc, player, "Hello {playername}.") )
    end
end

function npchandler:busy(npc, player)
    if self.responses["busy"] then
        npcsay(self:replace(npc, player, self.responses["busy"] ) )
    else
        npcsay(self:replace(npc, player, "I'll talk to you soon {playername}.") )
    end  
end

function npchandler:say(npc, player, message)
    if self.responses["say"] then
        for question, answer in pairs(self.responses["say"] ) do
	        if message == question then
                 npcsay(self:replace(npc, player, answer) )
            end
        end
    else
        if message == "name" then
            npcsay(self:replace(npc, player, "My name is {npcname}.") )
        end
    end    
end

function npchandler:farewell(npc, player)
    if self.responses["farewell"] then
        npcsay(self:replace(npc, player, self.responses["farewell"] ) )
    else
        npcsay(self:replace(npc, player, "Bye {playername}.") )
    end
end

function npchandler:dismiss(npc, player)
    if self.responses["dismiss"] then
        npcsay(self:replace(npc, player, self.responses["dismiss"] ) )
    else
        npcsay(self:replace(npc, player, "Bye.") )
    end    
end

function npchandler:replace(npc, player, message)
    message = string.gsub(message, "%{playername%}", player.Name)
    message = string.gsub(message, "%{npcname%}", npc.Name)
    return message
end