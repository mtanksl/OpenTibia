server = {
	login = {
		port = 7171
	},
	game = {
		port = 7172,
		maxplayers = 1000
	},		
	security = {
		-- packets abuse
		maxpackets = 60,
		maxpacketspermilliseconds = 1000,
		packetsabusebanmilliseconds = 15 * 60 * 1000,		
		-- login attempts
		maxloginattempts = 10,
		maxloginattemptspermilliseconds = 60 * 1000,
		loginattemptsabusebanmilliseconds = 15 * 60 * 1000,
		-- socket timeout
		socketreceivetimeoutmilliseconds = 10 * 1000,
		socketsendtimeoutmilliseconds = 10 * 1000
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