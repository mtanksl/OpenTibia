function onstepin(creature, totile)
	print("Creature " .. creature.Name .. " steped in ground " .. totile.Ground.Metadata.OpenTibiaId)  
	return true
end