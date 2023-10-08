function onusingrune(player, target, tile, item)
	--TODO
	return true
end

function onuserune(player, target, tile, item)
	local area = {
												{-1, -3}, {0, -3}, {1, -3},
							{-2, -2}, {-1, -2}, {0, -2}, {1, -2}, {2, -2},
		{-3, -1}, {-2, -1}, {-1, -1}, {0, -1}, {1, -1}, {2, -1}, {3, -1},
		{-3, 0},  {-2, 0},  {-1, 0},  {0, 0},  {1, 0},  {2, 0},  {3, 0},
		{-3, 1},  {-2, 1},  {-1, 1},  {0, 1},  {1, 1},  {2, 1},  {3, 1},
							{-2, 2},  {-1, 2},  {0, 2},  {1, 2},  {2, 2},
												{-1, 3},  {0, 3},  {1, 3}
	}
	local min, max = formula.generic(player.Level, player.Skills.MagicLevel, 50, 15)
	command.creatureattackarea(player, false, tile.Position, area, projectiletype.Fire, magiceffecttype.firearea, attack.simple(nil, nil, animatedtextcolor.orange, min, max), nil)
end