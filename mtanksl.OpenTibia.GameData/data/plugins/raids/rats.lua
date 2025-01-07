function onraid()
	announce("Rat plague!")
	command.delay(1 * 1000)
	areaspawn("Rat", 5, 10, 930, 781, 7, 4)
	areaspawn("Cave Rat", 5, 10, 930, 781, 7, 4)
	command.delay(1 * 60 * 60 * 1000)
	despawn()
end

local monsters = {}

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
				command.showmagiceffect(monster, magiceffecttype.teleport)
				table.insert(monsters, monster)
				break
			end		
		end
	end
end

function despawn()
	for _, monster in ipairs(monsters) do
		if monster.Tile and not monster.IsDestroyed then
			command.showmagiceffect(monster, magiceffecttype.puff)
			command.creaturedestroy(monster)
		end
	end
end