function onusingweapon(player, target, weapon)
	return true -- validated, continue process
end

function onuseweapon(player, target, weapon)
	local min, max = formula.wand(65, 9)
	command.creatureattackcreature(player, target, attack.distance(cast(weapon.Metadata.ProjectileType, "System.Int64"), min, max), nil)
end