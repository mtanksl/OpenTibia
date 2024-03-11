PRIVATE_NPC_SYSTEM = getconfig("server", "server.game.privatenpcsystem")

topiccondition = {}

topiccondition.mt = {
	__index = topiccondition
}

function topiccondition:new(success, continue, captures)
	local o = {
		success = success,
		continue = continue,
		captures = captures
	}
	setmetatable(o, self.mt)
	return o
end

topicmatch = {}

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

topicmatchresult = {}

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

topic = {}

topic.mt = {
	__index = topic
}

function topic:new(parent)
	local o = {
		parent = parent,
		matches = {}
	}
	setmetatable(o, self.mt)
	return o
end

function topic:add(question, answer, newparameters)
	local condition = nil
	if type(question) == "function" then
		condition = question
	else 
		if question == "" then
			condition = function(npc, player, message)
				return topiccondition:new(true, true)
			end
		else
			condition = function(npc, player, message)
				local captures = { string.match(message, question) }
				if #captures > 0 then
					return topiccondition:new(true, false, captures)
				end
				return topiccondition:new(false)
			end
		end
	end
	local callback = nil
	if type(answer) == "function" then
		callback = answer
	else
		callback = function(module, npc, player, message, captures, parameters)
			if newparameters then
				module.setparameters(newparameters)
			end
			module.say(answer)
		end	
	end
	table.insert(self.matches, topicmatch:new(condition, callback) )
end

function topic:addsell(responses, offers)
	if PRIVATE_NPC_SYSTEM then

	else 
		local confirm = topic:new(self)
		for _, offer in ipairs(offers) do
			self:add("sell (%d+) " .. offer.name, function(module, npc, player, message, captures, parameters)
				local count = math.max(1, math.min(100, tonumber(captures[1] ) ) ) 
				module.setparameters(offer, { count = count, price = offer.price * count, topic = confirm } )
				module.say(responses.questionitems)
			end)
			self:add("sell " .. offer.name, function(module, npc, player, message, captures, parameters) 
				module.setparameters(offer, { count = 1, price = offer.price, topic = confirm } )
				module.say(responses.questionitem)
			end)
		end
		confirm:add("yes", function(module, npc, player, message, captures, parameters) 
			module.setparameters( { topic = self } )
			if command.playerremoveitem(player, parameters.item, parameters.type, parameters.count) then
				command.playeraddmoney(player, parameters.price)
				module.say(responses.yes)
			else
				if parameters.count > 1 then
					module.say(responses.notenoughitems)
				else
					module.say(responses.notenoughitem)
				end
			end
		end)
		confirm:add("", function(module, npc, player, message, captures, parameters) 
			module.setparameters( { topic = self } )
			module.say(responses.no)
		end)
	end
end

function topic:addbuy(responses, offers)
	if PRIVATE_NPC_SYSTEM then

	else
		local confirm = topic:new(self)
		for _, offer in ipairs(offers) do
			self:add("buy (%d+) " .. offer.name, function(module, npc, player, message, captures, parameters) 
				local count = math.max(1, math.min(100, tonumber(captures[1] ) ) )
				module.setparameters(offer, { count = count, price = offer.price * count, topic = confirm } )
				module.say(responses.questionitems)
			end)
			self:add("(%d+) " .. offer.name, function(module, npc, player, message, captures, parameters) 
				local count = math.max(1, math.min(100, tonumber(captures[1] ) ) )
				module.setparameters(offer, { count = count, price = offer.price * count, topic = confirm } )
				module.say(responses.questionitems)
			end)
			self:add("buy " .. offer.name, function(module, npc, player, message, captures, parameters) 
				module.setparameters(offer, { count = 1, price = offer.price, topic = confirm } )
				module.say(responses.questionitem)
			end)
			self:add("" .. offer.name, function(module, npc, player, message, captures, parameters) 
				module.setparameters(offer, { count = 1, price = offer.price, topic = confirm } )
				module.say(responses.questionitem)
			end)
		end
		confirm:add("yes", function(module, npc, player, message, captures, parameters) 
			module.setparameters( { topic = self } )
			if command.playerremovemoney(player, parameters.price) then
				command.playeradditem(player, parameters.item, parameters.type, parameters.count)
				module.say(responses.yes)
			else
				module.say(responses.notenoughtgold)
			end
		end)
		confirm:add("", function(module, npc, player, message, captures, parameters) 
			module.setparameters( { topic = self } )
			module.say(responses.no)
		end)
	end
end

function topic:addtravel(responses, destinations)
	local confirm = topic:new(self)
	for _, destination in ipairs(destinations) do
		self:add("" .. destination.name, function(module, npc, player, message, captures, parameters) 
			module.setparameters(destination, { topic = confirm } )
			module.say(responses.question)
		end)
	end
	confirm:add("yes", function(module, npc, player, message, captures, parameters) 
		module.setparameters( { topic = self } )
		if command.playerremovemoney(player, parameters.price) then
			module.say(responses.yes)
			module.idle()
			command.creaturewalk(player, parameters.position)
			command.showmagiceffect(parameters.position, magiceffecttype.teleport)
		else
			module.say(responses.notenoughtgold)
		end
	end)
	confirm:add("", function(module, npc, player, message, captures, parameters) 
		module.setparameters( { topic = self } )
		module.say(responses.no)
	end)
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
					return result
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

function npchandler:new(responses)
	local o = {
		responses = responses,
		players = {}
	}
	setmetatable(o, self.mt)
	return o
end

function npchandler:say(npc, player, answer)
	if self.players[player.Id] then
		for key, value in pairs(self.players[player.Id] ) do
			answer = string.gsub(answer, "%{" .. tostring(key) .. "%}", tostring(value) )
		end
	end
	answer = string.gsub(answer, "%{playername%}", player.Name)
	answer = string.gsub(answer, "%{npcname%}", npc.Name)
	command.npcsay(npc, answer, PRIVATE_NPC_SYSTEM)
end

function npchandler:shouldgreet(npc, player, message)
	return message == "hi" or message == "hello" or message == "greet"
end

function npchandler:shouldfarewell(npc, player, message)
	return message == "bye" or message == "farewell"
end

function npchandler:ongreet(npc, player)
	self:say(npc, player, self.responses.greet)
end

function npchandler:onbusy(npc, player)
	self:say(npc, player, self.responses.busy)
end

function npchandler:onsay(npc, player, message)
	local topic = self.players[player.Id].topic
	local topicmatchresult = topic:match(npc, player, message)
	if topicmatchresult then
		self.players[player.Id].topic = topicmatchresult.topic
		local module = {
			setparameters = function(...)
				local t = { ... }
				for _, arg in ipairs(t) do
					for key, value in pairs(arg) do
						self.players[player.Id][key] = value
					end
				end
			end,
			say = function(answer)
				self:say(npc, player, answer)
			end,
			idle = function()
				command.npcidle(npc, player)
			end,
			farewell = function()
				command.npcfarewell(npc, player)
			end,
			disappear = function()
				command.npcdisappear(npc, player)
			end
		}
		topicmatchresult.callback(module, npc, player, message, topicmatchresult.captures, self.players[player.Id] )
	end
end

function npchandler:onfarewell(npc, player)
	self:say(npc, player, self.responses.farewell)
end

function npchandler:ondisappear(npc, player)
	self:say(npc, player, self.responses.disappear)
end

function npchandler:onenqueue(npc, player) 
	self.players[player.Id] = {
		topic = self.responses.say
	}
end

function npchandler:ondequeue(npc, player) 
	self.players[player.Id] = nil
end