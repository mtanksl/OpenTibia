PRIVATE_NPC_SYSTEM = getconfig("server", "server.game.gameplay.privatenpcsystem")

topiccondition = {}

topiccondition.mt = {
	__index = topiccondition
}

function topiccondition:new(success, continue)
	local o = {
		success = success,
		continue = continue
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

function topicmatchresult:new(topic, callback)
	local o = {
		topic = topic,
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

function topic:add(question, answer, parameters)
	local condition = nil
	if type(question) == "function" then
		condition = question
	else 
		if question == "" then
			condition = function(context)
				return topiccondition:new(true, true)
			end
		else
			condition = function(context)
				local captures = { 
					string.match(context.message, question)
				}
				if #captures > 0 then
					for key, value in pairs(captures) do
						context.captures[key] = value
					end
					return topiccondition:new(true, false)
				end
				return topiccondition:new(false)
			end
		end
	end
	local callback = nil
	if type(answer) == "function" then
		callback = answer
	else
		callback = function(context)
			if parameters then
				context:setparameters(parameters)
			end
			context:say(answer)
		end	
	end
	table.insert(self.matches, topicmatch:new(condition, callback) )
end

function topic:match(context)
	local result = nil
	local current = self
	while current do
		for _, topicmatch in ipairs(current.matches) do
			local topiccondition = topicmatch.condition(context)
			if topiccondition.success then
				if topiccondition.continue then
					if not result then
						result = topicmatchresult:new(current, topicmatch.callback)
					end
				else
					result = topicmatchresult:new(current, topicmatch.callback)
					return result
				end
			end
		end
		current = current.parent
	end
	return result
end

function topic:addtrade(offers, responses)
	if PRIVATE_NPC_SYSTEM then
		responses = responses or {}
		responses.trade = responses.trade or {}
		setmetatable(responses.trade, self.responses.trade.mt)
		self:add(responses.trade.question, function(context) 
			context:setresponses( { onbuy = responses.trade.onbuy, onsell = responses.trade.onsell, onclosenpctrade = responses.trade.onclosenpctrade } )
			context:trade(offers)
			context:say(responses.trade.answer)
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
					self:add("sell (%d+) " .. offer.name, function(context)
						local count = math.max(1, math.min(10000, tonumber(context.captures[1] ) ) ) 
						context:setparameters(offer, { count = count, sellprice = offer.sellprice * count, topic = confirm } )
						context:say(responses.sell.items)
					end)
					self:add("sell " .. offer.name, function(context) 
						context:setparameters(offer, { count = 1, sellprice = offer.sellprice, topic = confirm } )
						context:say(responses.sell.item)
					end)
				end
			end
			confirm:add("yes", function(context) 
				context:setparameters( { topic = self } )
				if command.playerdestroyitems(context.player, context.parameters.item, context.parameters.type, context.parameters.count) then
					command.playercreatemoney(context.player, context.parameters.sellprice)
					context:say(responses.sell.yes)
				else
					if context.parameters.count > 1 then
						context:say(responses.sell.notenoughitems)
					else
						context:say(responses.sell.notenoughitem)
					end
				end
			end)
			confirm:add("", function(context) 
				context:setparameters( { topic = self } )
				context:say(responses.sell.no)
			end)
		end
		if buying then
			responses = responses or {}
			responses.buy = responses.buy or {}
			setmetatable(responses.buy, self.responses.buy.mt)
			local confirm = topic:new(self)
			for _, offer in ipairs(offers) do
				if offer.buyprice and offer.buyprice > 0 then 
					self:add("buy (%d+) " .. offer.name, function(context) 
						local count = math.max(1, math.min(100, tonumber(context.captures[1] ) ) )
						context:setparameters(offer, { count = count, buyprice = offer.buyprice * count, topic = confirm } )
						context:say(responses.buy.items)
					end)
					self:add("(%d+) " .. offer.name, function(context) 
						local count = math.max(1, math.min(100, tonumber(context.captures[1] ) ) )
						context:setparameters(offer, { count = count, buyprice = offer.buyprice * count, topic = confirm } )
						context:say(responses.buy.items)
					end)
					self:add("buy " .. offer.name, function(context) 
						context:setparameters(offer, { count = 1, buyprice = offer.buyprice, topic = confirm } )
						context:say(responses.buy.item)
					end)
					self:add("" .. offer.name, function(context) 
						context:setparameters(offer, { count = 1, buyprice = offer.buyprice, topic = confirm } )
						context:say(responses.buy.item)
					end)
				end
			end
			confirm:add("yes", function(context) 
				context:setparameters( { topic = self } )
				if command.playerdestroymoney(context.player, context.parameters.buyprice) then
					command.playercreateitems(context.player, context.parameters.item, context.parameters.type, context.parameters.count)
					context:say(responses.buy.yes)
				else
					context:say(responses.buy.notenoughtgold)
				end
			end)
			confirm:add("", function(context) 
				context:setparameters( { topic = self } )
				context:say(responses.buy.no)
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
		self:add("" .. destination.name, function(context) 
			context:setparameters(destination, { topic = confirm } )
			context:say(responses.travel.passage)
		end)
	end
	confirm:add("yes", function(context) 
		context:setparameters( { topic = self } )
		if command.playerdestroymoney(context.player, context.parameters.price) then
			context:say(responses.travel.yes)
			context:idle()
			command.creaturemove(context.player, context.parameters.position)
			command.showmagiceffect(context.parameters.position, magiceffecttype.teleport)
		else
			context:say(responses.travel.notenoughtgold)
		end
	end)
	confirm:add("", function(context) 
		context:setparameters( { topic = self } )
		context:say(responses.travel.no)
	end)
end

context = {}

context.mt = {
	__index = context
}

function context:new(npchandler, npc, player, parameters)
	local o = {
		npchandler = npchandler,
		npc = npc,
		player = player,
		message = nil,
		captures = nil,
		parameters = parameters,
		delay = nil
	}
	setmetatable(o, self.mt)
	return o
end

function context:setresponses(responses) 
	for key, value in pairs(responses) do
		self.npchandler.responses[key] = value
	end
end

function context:setparameters(...)
	local t = { ... }
	for _, arg in ipairs(t) do
		for key, value in pairs(arg) do
			self.parameters[key] = value
		end
	end
end

function context:say(answer)
	if self.delay then
		command.canceldelay(self.delay)
	end
	self.npchandler:say(self.npc, self.player, answer)
end

function context:tell(answers, resolve)
	if self.delay then
		command.canceldelay(self.delay)
	end
	function next(i)
		if answers[i] then
			self.npchandler:say(self.npc, self.player, answers[i] )
			self.delay = command.delaygameobject(self.npc, math.max(3, math.ceil(string.len(answers[i] ) / 10) ), function() 
				next(i + 1)
			end)
		else
			if resolve then
				resolve()
			end
		end
	end
	next(1)
end

function context:trade(offers)
	command.npctrade(self.npc, self.player, offers)
end

function context:idle()
	if self.delay then
		command.canceldelay(self.delay)
	end
	command.npcidle(self.npc, self.player)
end

function context:farewell()
	if self.delay then
		command.canceldelay(self.delay)
	end
	command.npcfarewell(self.npc, self.player)
end

function context:disappear()
	if self.delay then
		command.canceldelay(self.delay)
	end
	command.npcdisappear(self.npc, self.player)
end

npchandler = {}

npchandler.mt = {
	__index = npchandler
}

function npchandler:new(responses)
	local o = {
		responses = responses,
		contexts = {}
	}
	setmetatable(o, self.mt)
	return o
end

function npchandler:say(npc, player, answer)
	if self.contexts[player.Id] then
		for key, value in pairs(self.contexts[player.Id].parameters) do
			answer = string.gsub(answer, "%[" .. tostring(key) .. "%]", tostring(value) )
		end
	end
	answer = string.gsub(answer, "%[playername%]", player.Name)
	answer = string.gsub(answer, "%[npcname%]", npc.Name)	
	if PRIVATE_NPC_SYSTEM then
		command.npcsaytoplayer(npc, player, answer)
	else
		answer = string.gsub(answer, "%{", "")
		answer = string.gsub(answer, "%}", "")
		command.npcsay(npc, answer)
	end
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
	local context = self.contexts[player.Id]
	context.message = message
	context.captures = {}
	local topic = context.parameters.topic
	local topicmatchresult = topic:match(context)
	if topicmatchresult then
		context.parameters.topic = topicmatchresult.topic
		topicmatchresult.callback(context)
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
	self.contexts[player.Id] = context:new(self, npc, player, { 
		topic = self.responses.say
	} )
end

function npchandler:ondequeue(npc, player) 
	if self.contexts[player.Id].key then
		command.canceldelay(self.contexts[player.Id].key)
	end
	self.contexts[player.Id] = nil
end