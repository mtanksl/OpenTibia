function announce(message)
	local players = command.gameobjectsgetplayers()
	for _, player in ipairs(players) do		
		command.showwindowtext(player, textcolor.whitecentergamewindowandserverlog, message)
	end
end

function singlespawn(monsters, name, centerx, centery, centerz, radius)
	areaspawn(monsters, name, 1, 1, centerx, centery, centerz, radius)
end

function areaspawn(monsters, name, minamount, maxamount, centerx, centery, centerz, radius)
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

function despawn(monsters)
	for _, monster in ipairs(monsters) do
		if monster.Tile and not monster.IsDestroyed then
			command.showmagiceffect(monster, magiceffecttype.puff)
			command.creaturedestroy(monster)
		end
	end
end