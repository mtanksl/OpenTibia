function onsteppingin(creature, totile)
	print("Creature " .. creature.Name .. " stepping in ground " .. totile.Ground.Metadata.OpenTibiaId)  
	return false -- not handled, continue process
end

function onstepin(creature, fromtile, totile)
	print("Creature " .. creature.Name .. " stepped in ground " .. totile.Ground.Metadata.OpenTibiaId .. " from " .. (fromtile and fromtile.Ground.Metadata.OpenTibiaId or "null") )  
end