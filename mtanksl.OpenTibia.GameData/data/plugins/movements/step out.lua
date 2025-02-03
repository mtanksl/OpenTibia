function onsteppingout(creature, fromtile)
	print("Creature " .. creature.Name .. " stepping out ground " .. fromtile.Ground.Metadata.OpenTibiaId)
	return false -- not handled, continue process
end

function onstepout(creature, fromtile, totile)
	print("Creature " .. creature.Name .. " stepped out ground " .. fromtile.Ground.Metadata.OpenTibiaId .. " to " .. (totile and totile.Ground.Metadata.OpenTibiaId or "null") )
end