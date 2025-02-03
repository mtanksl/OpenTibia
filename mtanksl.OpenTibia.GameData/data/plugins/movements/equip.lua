function onequipping(inventory, item, slot)
	print("Player " .. inventory.Player.Name .. " equipping " .. item.Metadata.OpenTibiaId)  
	return false -- not handled, continue process
end

function onequip(inventory, item, slot)
	print("Player " .. inventory.Player.Name .. " equipped " .. item.Metadata.OpenTibiaId)  
end