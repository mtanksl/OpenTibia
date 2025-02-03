function ondeequipping(inventory, item, slot)
	print("Player " .. inventory.Player.Name .. " deequipping " .. item.Metadata.OpenTibiaId)  
	return false -- not handled, continue process
end

function ondeequip(inventory, item, slot)
	print("Player " .. inventory.Player.Name .. " deequipped " .. item.Metadata.OpenTibiaId)  
end