function onuseitemwithcreature(player, item, tocreature)
	print("Player " .. player.Name .. " used item " .. item.Metadata.OpenTibiaId .. " with creature " .. tocreature.Name)
	return true
end