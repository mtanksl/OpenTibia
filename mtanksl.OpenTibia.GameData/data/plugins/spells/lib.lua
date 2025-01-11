formula = {
	haste = function(basespeed)
		return math.floor(basespeed * 1.3 - 24)
	end,
	stronghaste = function(basespeed)
		return math.floor(basespeed * 1.7 - 56)
	end,
	swiftfoot = function(level, basespeed)
		return basespeed + math.floor( (level * 1.6 + 110) / 2) * 2
	end,
	charge = function(level, basespeed)
		return basespeed + math.floor( (level * 1.8 + 123.3) / 2) * 2
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
	whirlwindthrow = function(level, skill, weapon)
		return math.floor( (skill + weapon) * 0.3 + level * 0.2), math.floor(skill + weapon + level * 0.2)
	end,
	etherealspear = function(level, skill)
		return math.floor( (skill + 25) * 0.3 + level * 0.2), math.floor(skill + 25 + level * 0.2)
	end,
	generic = function(level, magiclevel, minx, miny, maxx, maxy)
		return math.floor(level * 0.2 + magiclevel * minx + miny), math.floor(level * 0.2 + magiclevel * maxx + maxy)
	end
}