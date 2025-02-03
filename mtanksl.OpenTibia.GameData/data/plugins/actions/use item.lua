function onuseitem(player, item)
	print("Player " .. player.Name .. " used item " .. item.Metadata.OpenTibiaId)  
	return true -- handled, stop process
end