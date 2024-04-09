function onstepout(creature, fromtile, totile)
	print("Creature " .. creature.Name .. " steped out ground " .. fromtile.Ground.Metadata.OpenTibiaId)  
end