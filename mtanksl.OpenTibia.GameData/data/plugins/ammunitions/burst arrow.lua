function onuseammunition(player, target, weapon, ammunition)
	local area = {
		{-1, -1}, {0, -1}, {1, -1},
		{-1, 0},  {0, 0},  {1, 0},
		{-1, 1},  {0, 1},  {1, 1}
	}
	local min, max = formula.distance(player.Level, player.Skills.Distance, ammunition.Metadata.Attack, cast(player.Client.FightMode, "System.Int64") )
	command.creatureattackarea(player, false, target.Tile.Position, area, cast(ammunition.Metadata.ProjectileType, "System.Int64"), magiceffecttype.firearea, attack.simple(nil, nil, animatedtextcolor.orange, min, max), nil)
end