function onusingrune(player, target, toTile, rune)
	--TODO
	return true -- validated, continue process
end

function onuserune(player, target, toTile, rune)
	local area = {
							{-1, -3}, {0, -3}, {1, -3},
				  {-2, -2}, {-1, -2}, {0, -2}, {1, -2}, {2, -2},
		{-3, -1}, {-2, -1}, {-1, -1}, {0, -1}, {1, -1}, {2, -1}, {3, -1},
		{-3, 0},  {-2, 0},  {-1, 0},  {0, 0},  {1, 0},  {2, 0},  {3, 0},
		{-3, 1},  {-2, 1},  {-1, 1},  {0, 1},  {1, 1},  {2, 1},  {3, 1},
				  {-2, 2},  {-1, 2},  {0, 2},  {1, 2},  {2, 2},
							{-1, 3},  {0, 3},  {1, 3}
	}
	local min, max = formula.generic(player.Level, player.Skills.GetSkillLevel(skill.magiclevel), 1.2, 7, 2.8, 17)
	command.creatureattackarea(player, false, toTile.Position, area, projectiletype.fire, magiceffecttype.firearea, attack.simple(nil, nil, damagetype.fire, min, max), nil)
end