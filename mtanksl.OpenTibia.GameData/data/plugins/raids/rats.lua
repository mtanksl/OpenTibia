function onraid()
	local rats = stage:new(10 * 60 * 1000)
	rats:announce("Rat plague!")
	rats:areaspawn("Rat", 5, 10, 930, 781, 7, 4)
	rats:areaspawn("Cave Rat", 5, 10, 930, 781, 7, 4)
	local exterminated = rats:execute()
end