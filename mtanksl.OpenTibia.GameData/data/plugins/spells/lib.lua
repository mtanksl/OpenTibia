formula = {
	generic = function(level, magiclevel, minx, miny, maxx, maxy)
		return math.floor(level * 0.2 + magiclevel * minx + miny), math.floor(level * 0.2 + magiclevel * maxx + maxy)
	end
}