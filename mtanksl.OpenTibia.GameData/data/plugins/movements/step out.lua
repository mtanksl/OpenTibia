function onstepout(creature, fromtile)
	print("Creature " .. creature.Name .. " steped out ground " .. fromtile.Ground.Metadata.OpenTibiaId)  
	return true
end