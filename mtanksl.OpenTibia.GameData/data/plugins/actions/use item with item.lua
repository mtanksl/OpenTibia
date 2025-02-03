function onuseitemwithitem(player, item, toitem)
	print("Player " .. player.Name .. " used item " .. item.Metadata.OpenTibiaId .. " with item " .. toitem.Id)
	return true -- handled, stop process
end