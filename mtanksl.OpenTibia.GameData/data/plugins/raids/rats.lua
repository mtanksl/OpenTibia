function onraid()
	local monsters = {}
	announce("Rat plague!")
	command.delay(1 * 1000)
	areaspawn(monsters, "Rat", 5, 10, 930, 781, 7, 4)
	areaspawn(monsters, "Cave Rat", 5, 10, 930, 781, 7, 4)
	command.delay(1 * 60 * 60 * 1000)
	despawn(monsters)
end