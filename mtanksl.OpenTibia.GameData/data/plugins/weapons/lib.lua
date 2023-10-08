formula = {
	melee = function(level, skill, attack, mode)
		local factor = 0.5
		if mode == fightmode.offensive then
			factor = 1
		elseif mode == fighmode.balanced then
			factor = 0.75
		end
		local min = 0
		local max = math.floor(0.085 * factor * skill * attack) + math.floor(level / 5.0)
		return min, max
	end,
	distance = function(level, skill, attack, mode)
		local factor = 0.5
		if mode == fightmode.offensive then
			factor = 1
		elseif mode == fighmode.balanced then
			factor = 0.75
		end		
		local min = math.floor(level / 5.0)
		local max = math.floor(0.09 * factor * skill * attack) + min
		return min, max
	end,
	wand = function(attackstrength, attackvariation)
		local min = attackstrength - attackvariation
		local max = attackstrength + attackvariation
		return min, max;
	end
}