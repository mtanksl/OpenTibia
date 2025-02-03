function onmoveitem(player, item, tocontainer, toindex, count)
	print("Player " .. player.Name .. " moved item " .. item.Metadata.OpenTibiaId)  
	return true -- handled, stop process
end