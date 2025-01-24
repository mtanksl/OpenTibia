DEATH_EVENT = "OpenTibia.Game.Events.CreatureDeathEventArgs, mtanksl.OpenTibia.Game.Common"

stage = {}

stage.mt = {
	__index = stage
}

function stage:new(timeout)
	local o = {
		timeout = timeout,
		events = {},
		monsters = {},
		count = 0,
		waithandle = nil,
		delay = nil
	}
	setmetatable(o, self.mt)
	return o
end

function stage:announce(message)
	table.insert(self.events, { name = "announce", parameters = { message = message } } )
end

function stage:singlespawn(name, centerx, centery, centerz, radius) 
	table.insert(self.events, { name = "singlespawn", parameters = { name = name, centerx = centerx, centery = centery, centerz = centerz, radius = radius } } )
end

function stage:areaspawn(name, minamount, maxamount, centerx, centery, centerz, radius)
	table.insert(self.events, { name = "areaspawn", parameters = { name = name, minamount = minamount, maxamount = maxamount, centerx = centerx, centery = centery, centerz = centerz, radius = radius } } )
end

function stage:execute()
	function announce(message)
		local players = command.gameobjectsgetplayers()
		for _, player in ipairs(players) do
			command.showwindowtext(player, textcolor.whitecentergamewindowandserverlog, message)
		end
	end

	function singlespawn(name, centerx, centery, centerz, radius) 
		areaspawn(name, 1, 1, centerx, centery, centerz, radius)
	end

	function areaspawn(name, minamount, maxamount, centerx, centery, centerz, radius)
		for i = 1, math.random(minamount, maxamount) do
			for j = 1, 10 do
				local tile = command.mapgettile( { x = centerx + math.random(-radius, radius), y = centery + math.random(-radius, radius), z = centerz } )
				if tile and tile.Ground and not tile.NotWalkable and not tile.BlockPathFinding and not tile.Block and not tile.ProtectionZone then
					local monster = command.tilecreatemonster(tile, name)
					command.eventhandlergameobject(monster, DEATH_EVENT, function(e)
						if e.Creature.Id == monster.Id then
							self.count = self.count - 1
							if self.count == 0 then
								command.canceldelay(self.delay)
								command.set(self.waithandle, true)
							end
						end
					end)
					command.showmagiceffect(monster, magiceffecttype.teleport)
					table.insert(self.monsters, monster)
					self.count = self.count + 1
					break
				end		
			end
		end
	end

	function despawn()
		for _, monster in ipairs(self.monsters) do
			if monster.Tile and not monster.IsDestroyed then
				command.showmagiceffect(monster, magiceffecttype.puff)
				command.creaturedestroy(monster)
			end
		end
	end

	if not self.waithandle then
		self.waithandle = command.waithandle()
		self.delay = command.delay(self.timeout, function()
			despawn()
			command.set(self.waithandle, false)
		end)
		for _, event in ipairs(self.events) do
			if event.name == "announce" then
				announce(event.parameters.message)	
			elseif event.name == "singlespawn" then
				singlespawn(event.parameters.name, event.parameters.centerx, event.parameters.centery, event.parameters.centerz, event.parameters.radius)
			elseif event.name == "areaspawn" then
				areaspawn(event.parameters.name, event.parameters.minamount, event.parameters.maxamount, event.parameters.centerx, event.parameters.centery, event.parameters.centerz, event.parameters.radius)
			end
		end
		return command.wait(self.waithandle)
	end
	return nil
end