function onequip(inventory, item, slot)
	print("Player " .. inventory.Player.Name .. " equiped " .. item.Metadata.OpenTibiaId)  
	return true
end