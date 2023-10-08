formula = {
	generic = function(level, magiclevel, base, variation)
		local formula = 3 * magiclevel + 2 * level
		return math.floor(formula * (base - variation) / 100), math.floor(formula * (base + variation) / 100)
	end
}