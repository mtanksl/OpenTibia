topiccondition = {}

topiccondition.mt = {
	__index = topiccondition
}

-- @args (bool success, bool continue, string[] captures)
-- @returns TopicCondition
function topiccondition:new(success, continue, captures)
	local o = {
		success = success,		-- bool
		continue = continue,	-- bool
		captures = captures		-- string[]
	}
	setmetatable(o, self.mt)
	return o
end

topiccallback = {}

topiccallback.mt = {
	__index = topiccallback
}

-- @args (object[] newparameters, string answer)
-- @returns TopicCallback
function topiccallback:new(newparameters, answer)
	local o = {
		newparameters = newparameters,	-- object[]
		answer = answer					-- string
	}
	setmetatable(o, self.mt)
	return o
end

topicmatch = {}

topicmatch.mt = {
	__index = topicmatch
}

-- @args (Func<Npc, Player, string, TopicCondition> condition, Func<Npc, Player, string, string[], object[], TopicCallback> callback)
-- @returns TopicMatch
function topicmatch:new(condition, callback)
	local o = {
		condition = condition,	-- Func<Npc, Player, string, TopicCondition>
		callback = callback		-- Func<Npc, Player, string, string[], object[], TopicCallback>
	}
	setmetatable(o, self.mt)
	return o
end

topicmatchresult = {}

topicmatchresult.mt = {
	__index = topicmatchresult
}

-- @args (Topic topic, string[] captures, Func<Npc, Player, string, string[], object[], TopicCallback> callback)
-- @returns TopicMatchResult
function topicmatchresult:new(topic, captures, callback)
	local o = {
		topic = topic,			-- Topic
		captures = captures,	-- string[]
		callback = callback		-- Func<Npc, Player, string, string[], object[], TopicCallback>
	}
	setmetatable(o, self.mt)
	return o
end

topic = {}

topic.mt = {
	__index = topic
}

-- @args (Topic parent)
-- @returns Topic
function topic:new(parent)
	local o = {
		parent = parent,		-- Topic
		matches = {}			-- TopicMatch[]
	}
	setmetatable(o, self.mt)
	return o
end

-- @args (string question, string answer, object[] newparameters = null)
-- @args (TopicCondition question, TopicCallback answer)
-- @args (Func<Npc, Player, string, TopicCondition> question, Func<Npc, Player, string, string[], object[], TopicCallback> answer)
-- @returns void
function topic:add(question, answer, newparameters)
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
					return topiccondition:new(true, true)
				end
			else
				if string.find(question, "%(.+%)") then
					condition = function(npc, player, message)
						local captures = { string.match(message, question) }
						if #captures > 0 then
							return topiccondition:new(true, false, captures)
						end
						return topiccondition:new(false)
					end
				else
					condition = function(npc, player, message)
						if message == question then
							return topiccondition:new(true, false, {} )
						end
						return topiccondition:new(false)
					end
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
				return topiccallback:new(newparameters or {}, answer)
			end
		end
	end
	table.insert(self.matches, topicmatch:new(condition, callback) )
end

-- @args (object[] responses, object[] offers)
-- @returns void
function topic:addsell(responses, offers)
	local confirm = topic:new(self)
	for _, offer in ipairs(offers) do
		self:add("sell (%d+) " .. offer.name, function(npc, player, message, captures, parameters)
			local count = math.max(1, math.min(100, tonumber(captures[1] ) ) ) 
			return topiccallback:new(table.extend(offer, { count = count, price = offer.price * count, topic = confirm } ), responses.questionitems) 
		end)
		self:add("sell " .. offer.name, function(npc, player, message, captures, parameters) 
			return topiccallback:new(table.extend(offer, { count = 1, price = offer.price, topic = confirm } ), responses.questionitem)
		end)
	end
	confirm:add("yes", function(npc, player, message, captures, parameters) 
		if command.playerremoveitem(player, parameters.item, parameters.type, parameters.count) then
			command.playeraddmoney(player, parameters.price)
			return topiccallback:new( { topic = self }, responses.yes)
		end
		if parameters.count > 1 then
			return topiccallback:new( { topic = self }, responses.notenoughitems)
		end
		return topiccallback:new( { topic = self }, responses.notenoughitem)		
	end)
	confirm:add("", function(npc, player, message, captures, parameters) 
		return topiccallback:new( { topic = self }, responses.no)
	end)
end

-- @args (object[] responses, object[] offers)
-- @returns void
function topic:addbuy(responses, offers)
	local confirm = topic:new(self)
	for _, offer in ipairs(offers) do
		self:add("buy (%d+) " .. offer.name, function(npc, player, message, captures, parameters) 
			local count = math.max(1, math.min(100, tonumber(captures[1] ) ) )
			return topiccallback:new(table.extend(offer, { count = count, price = offer.price * count, topic = confirm } ), responses.questionitems) 
		end)
		self:add("(%d+) " .. offer.name, function(npc, player, message, captures, parameters) 
			local count = math.max(1, math.min(100, tonumber(captures[1] ) ) )
			return topiccallback:new(table.extend(offer, { count = count, price = offer.price * count, topic = confirm } ), responses.questionitems) 
		end)
		self:add("buy " .. offer.name, function(npc, player, message, captures, parameters) 
			return topiccallback:new(table.extend(offer, { count = 1, price = offer.price, topic = confirm } ), responses.questionitem)
		end)
		self:add("" .. offer.name, function(npc, player, message, captures, parameters) 
			return topiccallback:new(table.extend(offer, { count = 1, price = offer.price, topic = confirm } ), responses.questionitem)
		end)
	end
	confirm:add("yes", function(npc, player, message, captures, parameters) 
		if command.playerremovemoney(player, parameters.price) then
			command.playeradditem(player, parameters.item, parameters.type, parameters.count)
			return topiccallback:new( { topic = self }, responses.yes)
		end
		return topiccallback:new( { topic = self }, responses.notenoughtgold)
	end)
	confirm:add("", function(npc, player, message, captures, parameters) 
		return topiccallback:new( { topic = self }, responses.no)
	end)
end

-- @args (object[] responses, object[] destinations)
-- @returns void
function topic:addtravel(responses, destinations)
	local confirm = topic:new(self)
	for _, destination in ipairs(destinations) do
		self:add("" .. destination.name, function(npc, player, message, captures, parameters) 
			return topiccallback:new(table.extend(destination, { topic = confirm } ), responses.question)
		end)
	end
	confirm:add("yes", function(npc, player, message, captures, parameters) 
		if command.playerremovemoney(player, parameters.price) then			
			command.creaturewalk(player, parameters.position)
			command.showmagiceffect(parameters.position, magiceffecttype.teleport)
			return topiccallback:new( { topic = self }, responses.yes)
		end
		return topiccallback:new( { topic = self }, responses.notenoughtgold)
	end)
	confirm:add("", function(npc, player, message, captures, parameters) 
		return topiccallback:new( { topic = self }, responses.no)
	end)
end

-- @args (Npc npc, Player player, string message)
-- @returns TopicMatchResult
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

npchandler = {}

npchandler.mt = {
	__index = npchandler
}

-- @args (object[] responses)
-- @returns NpcHandler
function npchandler:new(responses)
	local o = {
		responses = responses,	-- object[]
		players = {}			-- Dictionary<int, object[]>
	}
	setmetatable(o, self.mt)
	return o
end

-- @args (Npc npc, Player player, string message)
-- @returns bool
function npchandler:shouldgreet(npc, player, message)
	return message == "hi" or message == "hello"
end

-- @args (Npc npc, Player player, string message)
-- @returns bool
function npchandler:shouldfarewell(npc, player, message)
	return message == "bye" or message == "farewell"
end

-- @args (Npc npc, Player player)
-- @returns void
function npchandler:ongreet(npc, player)
	local greet = self.responses.greet
	self.players[player.Id] = {
		topic = self.responses.say
	}
	command.npcsay(npc, self:replace(npc, player, greet) )
end

-- @args (Npc npc, Player player)
-- @returns void
function npchandler:onbusy(npc, player)
	local busy = self.responses.busy
	command.npcsay(npc, self:replace(npc, player, busy) )
end

-- @args (Npc npc, Player player, string message)
-- @returns void
function npchandler:onsay(npc, player, message)
	local topic = self.players[player.Id].topic
	local topicmatchresult = topic:match(npc, player, message)
	if topicmatchresult then
		local topiccallback = topicmatchresult.callback(npc, player, message, topicmatchresult.captures, self.players[player.Id] )
		self.players[player.Id].topic = topicmatchresult.topic
		for key, value in pairs(topiccallback.newparameters) do
			self.players[player.Id][key] = value
		end
		command.npcsay(npc, self:replace(npc, player, topiccallback.answer) )
	end
end

-- @args (Npc npc, Player player)
-- @returns void
function npchandler:onfarewell(npc, player)
	local farewell = self.responses.farewell
	command.npcsay(npc, self:replace(npc, player, farewell) )
end

-- @args (Npc npc, Player player)
-- @returns void
function npchandler:ondismiss(npc, player)
	local dismiss = self.responses.dismiss
	command.npcsay(npc, self:replace(npc, player, dismiss) )
end

-- @args (Npc npc, Player player, string message)
-- @returns string
function npchandler:replace(npc, player, message)	
	if self.players[player.Id] then
		for key, value in pairs(self.players[player.Id] ) do
			if type(value) == "function" then 
				value = value(npc, player)
			else 
				value = tostring(value)
			end
			message = string.gsub(message, "%{" .. key .. "%}", value)
		end
	end
	message = string.gsub(message, "%{playername%}", player.Name)
	message = string.gsub(message, "%{npcname%}", npc.Name)
	return message
end