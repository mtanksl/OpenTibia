function onwrapitem(player, item)
	print("Player " .. player.Name .. " wrapped item " .. item.Id)
	return true -- handled, stop process
end