PRIVATE_NPC_SYSTEM = getconfig("server", "server.gameplay.privatenpcsystem")

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

topic.responses = { 
	buy = {
		items = "Do you want to buy [count] [plural] for [buyprice] gold?",
		item = "Do you want to buy [article] [name] for [buyprice] gold?",
		yes = "Thank you. Here it is.",
		notenoughtgold = "Sorry, you do not have enough gold.",
		no = "Maybe you will buy it another time."
	},
	sell = {
		items = "Do you want to sell [count] [plural] for [sellprice] gold?",
		item = "Do you want to sell [article] [name] for [sellprice] gold?",
		yes = "Ok. Here is your money.",
		notenoughitems = "Sorry, you do not have so many.",
		notenoughitem = "Sorry, you do not have one.",
		no = "Maybe next time.",
	},
	trade = {
		question = "trade",
		answer = "Take a look in the trade window to your right.",
		onbuy = "Thank you. Here it is.",
		onsell = "Ok. Here is your money.",
		onclosenpctrade = "Do you want to trade something else?"
	},
	travel = {
		passage = "Do you seek a passage to [town] for [price] gold?",
		yes = "Set the sails!",
		notenoughtgold = "You don't have enough money.",
		no = "We would like to serve you some time."	
	}
}

topic.responses.buy.mt = {
	__index = topic.responses.buy
}

topic.responses.sell.mt = {
	__index = topic.responses.sell
}

topic.responses.trade.mt = {
	__index = topic.responses.trade
}

topic.responses.travel.mt = {
	__index = topic.responses.travel
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

function topic:addtrade(offers, responses)
	if PRIVATE_NPC_SYSTEM then
		responses = responses or {}
		responses.trade = responses.trade or {}
		setmetatable(responses.trade, self.responses.trade.mt)
		self:add(responses.trade.question, function(module, npc, player, message, captures, parameters) 
			module.setresponses( { onbuy = responses.trade.onbuy, onsell = responses.trade.onsell, onclosenpctrade = responses.trade.onclosenpctrade } )
			module.trade(offers)
			module.say(responses.trade.answer)
		end)
	else
		local selling = false
		local buying = false
		for _, offer in ipairs(offers) do
			if offer.sellprice and offer.sellprice > 0 then
				selling = true
			end
			if offer.buyprice and offer.buyprice > 0 then 
				buying = true
			end
		end
		if selling then
			responses = responses or {}
			responses.sell = responses.sell or {}
			setmetatable(responses.sell, self.responses.sell.mt)
			local confirm = topic:new(self)
			for _, offer in ipairs(offers) do
				if offer.sellprice and offer.sellprice > 0 then
					self:add("sell (%d+) " .. offer.name, function(module, npc, player, message, captures, parameters)
						local count = math.max(1, math.min(100, tonumber(captures[1] ) ) ) 
						module.setparameters(offer, { count = count, sellprice = offer.sellprice * count, topic = confirm } )
						module.say(responses.sell.items)
					end)
					self:add("sell " .. offer.name, function(module, npc, player, message, captures, parameters) 
						module.setparameters(offer, { count = 1, sellprice = offer.sellprice, topic = confirm } )
						module.say(responses.sell.item)
					end)
				end
			end
			confirm:add("yes", function(module, npc, player, message, captures, parameters) 
				module.setparameters( { topic = self } )
				if command.playerdestroyitems(player, parameters.item, parameters.type, parameters.count) then
					command.playercreatemoney(player, parameters.sellprice)
					module.say(responses.sell.yes)
				else
					if parameters.count > 1 then
						module.say(responses.sell.notenoughitems)
					else
						module.say(responses.sell.notenoughitem)
					end
				end
			end)
			confirm:add("", function(module, npc, player, message, captures, parameters) 
				module.setparameters( { topic = self } )
				module.say(responses.sell.no)
			end)
		end
		if buying then
			responses = responses or {}
			responses.buy = responses.buy or {}
			setmetatable(responses.buy, self.responses.buy.mt)
			local confirm = topic:new(self)
			for _, offer in ipairs(offers) do
				if offer.buyprice and offer.buyprice > 0 then 
					self:add("buy (%d+) " .. offer.name, function(module, npc, player, message, captures, parameters) 
						local count = math.max(1, math.min(100, tonumber(captures[1] ) ) )
						module.setparameters(offer, { count = count, buyprice = offer.buyprice * count, topic = confirm } )
						module.say(responses.buy.items)
					end)
					self:add("(%d+) " .. offer.name, function(module, npc, player, message, captures, parameters) 
						local count = math.max(1, math.min(100, tonumber(captures[1] ) ) )
						module.setparameters(offer, { count = count, buyprice = offer.buyprice * count, topic = confirm } )
						module.say(responses.buy.items)
					end)
					self:add("buy " .. offer.name, function(module, npc, player, message, captures, parameters) 
						module.setparameters(offer, { count = 1, buyprice = offer.buyprice, topic = confirm } )
						module.say(responses.buy.item)
					end)
					self:add("" .. offer.name, function(module, npc, player, message, captures, parameters) 
						module.setparameters(offer, { count = 1, buyprice = offer.buyprice, topic = confirm } )
						module.say(responses.buy.item)
					end)
				end
			end
			confirm:add("yes", function(module, npc, player, message, captures, parameters) 
				module.setparameters( { topic = self } )
				if command.playerdestroymoney(player, parameters.buyprice) then
					command.playercreateitems(player, parameters.item, parameters.type, parameters.count)
					module.say(responses.buy.yes)
				else
					module.say(responses.buy.notenoughtgold)
				end
			end)
			confirm:add("", function(module, npc, player, message, captures, parameters) 
				module.setparameters( { topic = self } )
				module.say(responses.buy.no)
			end)
		end
	end
end

function topic:addtravel(destinations, responses)
	responses = responses or {}
	responses.travel = responses.travel or {}
	setmetatable(responses.travel, self.responses.travel.mt)
	local confirm = topic:new(self)
	for _, destination in ipairs(destinations) do
		self:add("" .. destination.name, function(module, npc, player, message, captures, parameters) 
			module.setparameters(destination, { topic = confirm } )
			module.say(responses.travel.passage)
		end)
	end
	confirm:add("yes", function(module, npc, player, message, captures, parameters) 
		module.setparameters( { topic = self } )
		if command.playerdestroymoney(player, parameters.price) then
			module.say(responses.travel.yes)
			module.idle()
			command.creaturemove(player, parameters.position)
			command.showmagiceffect(parameters.position, magiceffecttype.teleport)
		else
			module.say(responses.travel.notenoughtgold)
		end
	end)
	confirm:add("", function(module, npc, player, message, captures, parameters) 
		module.setparameters( { topic = self } )
		module.say(responses.travel.no)
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
			answer = string.gsub(answer, "%[" .. tostring(key) .. "%]", tostring(value) )
		end
	end
	answer = string.gsub(answer, "%[playername%]", player.Name)
	answer = string.gsub(answer, "%[npcname%]", npc.Name)	
	if not PRIVATE_NPC_SYSTEM then
		answer = string.gsub(answer, "%{", "")
		answer = string.gsub(answer, "%}", "")
	end
	command.npcsay(npc, answer)
end

function npchandler:shouldgreet(npc, player, message)
	return message == "hi" or message == "hello" or message == "greet"
end

function npchandler:shouldfarewell(npc, player, message)
	return message == "bye" or message == "farewell"
end

function npchandler:ongreet(npc, player)
	if self.responses.greet then
		self:say(npc, player, self.responses.greet)
	end
end

function npchandler:onbusy(npc, player)
	if self.responses.busy then
		self:say(npc, player, self.responses.busy)
	end
end

function npchandler:onsay(npc, player, message)
	local topic = self.players[player.Id].topic
	local topicmatchresult = topic:match(npc, player, message)
	if topicmatchresult then
		self.players[player.Id].topic = topicmatchresult.topic
		local module = {
			setresponses = function(responses) 
				for key, value in pairs(responses) do
					self.responses[key] = value
				end
			end,
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
			trade = function(offers)
				command.npctrade(npc, player, offers)
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

function npchandler:onbuy(npc, player, item, type, count, price, ignoreCapacity, buyWithBackpacks) 
	if command.playerdestroymoney(player, price) then
		command.playercreateitems(player, item, type, count)
		if self.responses.onbuy then
			self:say(npc, player, self.responses.onbuy)
		end
	end
end

function npchandler:onsell(npc, player, item, type, count, price, keepEquipped) 
	if command.playerdestroyitems(player, item, type, count) then
		command.playercreatemoney(player, price)
		if self.responses.onsell then
			self:say(npc, player, self.responses.onsell)
		end
	end
end

function npchandler:onclosenpctrade(npc, player) 
	if self.responses.onclosenpctrade then
		self:say(npc, player, self.responses.onclosenpctrade)
	end
end

function npchandler:onfarewell(npc, player)
	if self.responses.farewell then
		self:say(npc, player, self.responses.farewell)
	end
end

function npchandler:ondisappear(npc, player)
	if self.responses.disappear then
		self:say(npc, player, self.responses.disappear)
	end
end

function npchandler:onenqueue(npc, player) 
	self.players[player.Id] = {
		topic = self.responses.say
	}
end

function npchandler:ondequeue(npc, player) 
	self.players[player.Id] = nil
end