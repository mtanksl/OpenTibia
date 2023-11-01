server = {
	login = {
		port = 7171
	},
	game = {
		port = 7172,
		maxplayers = 1000
	},		
	ratelimiting = {
			maxpackets = 60,
			milliseconds = 1000,
			banmilliseconds = 15 * 60 * 1000
	},
	database = {
		type = "sqlite",

		-- sqlite
		source = "data/database.db",

		-- mysql or mssql
		host = "localhost",
		port = 3306,
		user = "root",
		password = "",
		name = "mtots"
	}
}