function onattacking(monster, target)
	return true -- validated, continue process
end

function onattack(monster, target, min, max, attributes)
	local area = {
			     {0, 1},
			     {0, 2}
		{-1, 3}, {0, 3}, {1, 3},
		{-1, 4}, {0, 4}, {1, 4},
		{-1, 5}, {0, 5}, {1, 5}
	}	
	command.creatureattackarea(player, true, player.Tile.Position, area, nil, magiceffecttype.firearea, attack.simple(nil, nil, damagetype.fire, min, max), nil)
end