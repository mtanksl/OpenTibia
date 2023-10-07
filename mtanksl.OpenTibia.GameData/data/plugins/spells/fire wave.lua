function oncasting(player, message)
	return true
end

function oncast(player, message)
	local area = {
						  {0, 1},
				 {-1, 2}, {0, 2}, {1, 2},
				 {-1, 3}, {0, 3}, {1, 3},
		{-2, 4}, {-1, 4}, {0, 4}, {1, 4}, {2, 4}
	}	
	local min, max = formula.generic(player.Level, player.Skills.MagicLevel, 1.2, 0, 2, 0)
	command.creatureattackarea(player, true, player.Tile.Position, area, nil, magiceffecttype.firearea, attack.simple(nil, nil, animatedtextcolor.orange, min, max), nil)
end