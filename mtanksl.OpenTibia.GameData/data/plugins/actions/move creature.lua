function onmovecreature(player, creature, tile)
	print("Player " .. player.Name .. " moved creature " .. creature.Name)  
	return true -- handled, stop process
end