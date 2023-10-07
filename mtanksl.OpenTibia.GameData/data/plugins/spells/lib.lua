formula = {
	haste = function(basespeed)
		return math.floor(basespeed * 1.3 - 24);
	end,
	stronghaste = function(basespeed)
		return math.floor(basespeed * 1.7 - 56);
	end,
	groundshaker = function(level, skill, weapon)
		return math.floor( (skill + weapon) * 0.5 + level * 0.2), math.floor( (skill + weapon) * 1.1 + level * 0.2)
	end,
	berserk = function(level, skill, weapon)
		return math.floor( (skill + weapon) * 0.5 + level * 0.2), math.floor( (skill + weapon) * 1.5 + level * 0.2)
	end,
	fierceberserk = function(level, skill, weapon)
		return math.floor( (skill + weapon * 2) * 1.1 + level * 0.2), math.floor( (skill + weapon * 2) * 3 + level * 0.2)
	end,
	generic = function(level, magiclevel, minx, miny, maxx, maxy)
		return math.floor(level * 0.2 + magiclevel * minx + miny), math.floor(level * 0.2 + magiclevel * maxx + maxy)
	end
}