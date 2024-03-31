local say = topic:new()
say:add("name", "My name is Cipfried.")
say:add("job", "I am just a humble {monk}. Ask me if you need help or healing.")
say:add("monk", "I sacrifice my life to serve the good gods of {Tibia}.")
say:add("tibia", "That's where we are. The world of Tibia.")

local handler = npchandler:new( {
	greet = "Hello, [playername]! Feel free to ask me for help.",
	busy = "Please wait, [playername]. I already talk to someone!",
	say = say,
	farewell = "Farewell, [playername]!",
	disappear = "Well, bye then."
} )

registernpcsdialogue("Cipfried", handler)