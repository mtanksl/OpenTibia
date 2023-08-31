topicondition = { }

topicondition.mt = {
    __index = topicondition
}

function topicondition:new(success, continue, captures)
    local o = {
        success = success,
        continue = continue,
        captures = captures
    }
    setmetatable(o, self.mt)
    return o
end

topiccallback = { }

topiccallback.mt = {
    __index = topiccallback
}

function topiccallback:new(parameters, answer)
    local o = {
        parameters = parameters,
        answer = answer
    }
    setmetatable(o, self.mt)
    return o
end

topicmatch = { }

topicmatch.mt = {
    __index = topicmatch
}

function topicmatch:new(condition, callback)
    local o = {
        condition = condition,
        callback = callback
    }
    setmetatable(o, self.mt)
    return o
end

topicmatchresult = { }

topicmatchresult.mt = {
    __index = topicmatchresult
}

function topicmatchresult:new(topic, captures, callback)
    local o = {
        topic = topic,
        captures = captures,
        callback = callback
    }
    setmetatable(o, self.mt)
    return o
end

topic = { }

topic.mt = {
    __index = topic
}

function topic:new(parent)
    local o = {
        parent = parent,
        matches = { }
    }
    setmetatable(o, self.mt)
    return o
end

function topic:add(question, answer)
    local condition = nil
    if type(question) == "function" then
        condition = question
    else 
        if type(question) == "table" then
            condition = function(npc, player, message)
                return question
            end
        else 
            if question == "" then
                condition = function(npc, player, message)
                    return topicondition:new(true, true)
                end
            else
                condition = function(npc, player, message)
                    local captures = { string.match(message, question) }
                    if #captures == 0 then
                        return topicondition:new(false)
                    end
                    return topicondition:new(true, false, captures)
                end
            end
        end
    end
    local callback = nil
    if type(answer) == "function" then
        callback = answer
    else
        if type(answer) == "table" then
            callback = function(npc, player, message, captures, parameters)
                return answer
            end
        else
            callback = function(npc, player, message, captures, parameters)
                return topiccallback:new( { }, answer)
            end
        end
    end
    table.insert(self.matches, topicmatch:new(condition, callback) )
end

function topic:match(npc, player, message)
    local result = nil
    local current = self
    while current do
        for _, topicmatch in ipairs(current.matches) do
            local topiccondition = topicmatch.condition(npc, player, message)
            if topiccondition.success then
                if topiccondition.continue then
                    if not result then
                        result = topicmatchresult:new(current, topiccondition.captures, topicmatch.callback)
                    end
                else
                    result = topicmatchresult:new(current, topiccondition.captures, topicmatch.callback)
                    break
                end
            end
        end
        current = current.parent
    end
    return result
end

npchandler = { }

npchandler.mt = {
    __index = npchandler
}

function npchandler:new(responses)
    local o = {
        responses = responses,
        players = { }
    }
    setmetatable(o, self.mt)
    return o
end

function npchandler:shouldgreet(npc, player, message)
    return message == "hi" or message == "hello"
end

function npchandler:shouldfarewell(npc, player, message)
    return message == "bye" or message == "farewell"
end

function npchandler:ongreet(npc, player)
    local greet = self.responses.greet
    self.players[player.Id] = {
        topic = self.responses.say
    }
    npcsay(self:replace(npc, player, greet) )
end

function npchandler:onbusy(npc, player)
    local busy = self.responses.busy
    npcsay(self:replace(npc, player, busy) )
end

function npchandler:onsay(npc, player, message)
    local topic = self.players[player.Id].topic
    local topicmatchresult = topic:match(npc, player, message)
    if topicmatchresult then
        local topiccallback = topicmatchresult.callback(npc, player, message, topicmatchresult.captures, self.players[player.Id] )
        self.players[player.Id].topic = topicmatchresult.topic
        for key, value in pairs(topiccallback.parameters) do
	        self.players[player.Id][key] = value
        end
        npcsay(self:replace(npc, player, topiccallback.answer) )
    end
end

function npchandler:onfarewell(npc, player)
    local farewell = self.responses.farewell
    npcsay(self:replace(npc, player, farewell) )
end

function npchandler:ondismiss(npc, player)
    local dismiss = self.responses.dismiss
    npcsay(self:replace(npc, player, dismiss) )
end

function npchandler:replace(npc, player, message)    
    for key, value in pairs(self.players[player.Id] ) do
	    message = string.gsub(message, "%{" .. key .. "%}", tostring(value) )
    end
    message = string.gsub(message, "%{playername%}", player.Name)
    message = string.gsub(message, "%{npcname%}", npc.Name)
    return message
end