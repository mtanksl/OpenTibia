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

function topic:addsell(responses, trades)
    local confirm = topic:new(self)
    for _, trade in ipairs(trades) do
        self:add("sell (%d+) " .. trade.name, function(npc, player, message, captures, parameters)
            local count = math.max(1, math.min(100, tonumber(captures[1] ) ) ) 
            return topiccallback:new( { plural = trade.plural, item = trade.item, type = trade.type, count = count, price = trade.price * count, topic = confirm }, responses.questionitems) 
        end)
        self:add("sell " .. trade.name, topiccallback:new( { article = trade.article, name = trade.name, item = trade.item, type = trade.type, count = 1, price = trade.price, topic = confirm }, responses.questionitem) )
    end
    confirm:add("yes", function(npc, player, message, captures, parameters) 
        if npcremoveitem(player, parameters.item, parameters.type, parameters.count) then
            npcaddmoney(player, parameters.price)
            return topiccallback:new( { topic = self }, responses.yes)
        end
        if parameters.count > 1 then
            return topiccallback:new( { topic = self }, responses.notenoughitems)
        end
        return topiccallback:new( { topic = self }, responses.notenoughitem)        
    end)
    confirm:add("", topiccallback:new( { topic = self }, responses.no) )
end

function topic:addbuy(responses, trades)
    local confirm = topic:new(self)
    for _, trade in ipairs(trades) do
        self:add("buy (%d+) " .. trade.name, function(npc, player, message, captures, parameters) 
            local count = math.max(1, math.min(100, tonumber(captures[1] ) ) )
            return topiccallback:new( { plural = trade.plural,item = trade.item,type = trade.type, count = count, price = trade.price * count, topic = confirm }, responses.questionitems) 
        end)
        self:add("(%d+) " .. trade.name, function(npc, player, message, captures, parameters) 
            local count = math.max(1, math.min(100, tonumber(captures[1] ) ) )
            return topiccallback:new( { plural = trade.plural, item = trade.item, type = trade.type, count = count, price = trade.price * count, topic = confirm }, responses.questionitems) 
        end)
        self:add("buy " .. trade.name, topiccallback:new( { article = trade.article, name = trade.name, item = trade.item, type = trade.type, count = 1, price = trade.price, topic = confirm }, responses.questionitem) )
        self:add("" .. trade.name, topiccallback:new( { article = trade.article, name = trade.name, item = trade.item, type = trade.type, count = 1, price = trade.price, topic = confirm }, responses.questionitem) )
    end
    confirm:add("yes", function(npc, player, message, captures, parameters) 
        if npcdeletemoney(player, parameters.price) then
            npcadditem(player, parameters.item, parameters.type, parameters.count)
            return topiccallback:new( { topic = self }, responses.yes)
        end
        return topiccallback:new( { topic = self }, responses.notenoughtgold)
    end)
    confirm:add("", topiccallback:new( { topic = self }, responses.no) )
end

function topic:addtravel(responses, destinations)
    local confirm = topic:new(self)
    for _, destination in ipairs(destinations) do
	    self:add("" .. destination.name, topiccallback:new( { titlecasename = destination.titlecasename, price = destination.price, topic = confirm }, responses.question) )
    end
    confirm:add("yes", function(npc, player, message, captures, parameters) 
        if npcdeletemoney(player, parameters.price) then            
            -- TODO
            return topiccallback:new( { topic = self }, responses.yes)
        end
        return topiccallback:new( { topic = self }, responses.notenoughtgold)
    end)
    confirm:add("", topiccallback:new( { topic = self }, responses.no) )
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