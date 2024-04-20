server = {
	login = {
		maxconnections = 100,
		port = 7171
	},
	game = {
		maxconnections = 1100,
		port = 7172,
		maxplayers = 1000,
	},		
	gameplay = {
		privatenpcsystem = true,
		learnspellfirst = false,
		infinitepotions = false,
		infinitearrows = false,
		infiniterunes = false,
		maxvips = 100,
		maxdepotitems = 2000,
		lootrate = 1
	},
	security = {
		-- multi-client
		maxconnectionswithsameipaddress = 2,
		connectionswithsameipaddressabusebanmilliseconds = 15 * 60 * 1000,
		-- connections abuse
		maxconnections = 2,
		maxconnectionspermilliseconds = 1 * 1000,
		connectionsabusebanmilliseconds = 15 * 60 * 1000,
		-- packets abuse
		maxpackets = 60,
		maxpacketspermilliseconds = 1 * 1000,
		packetsabusebanmilliseconds = 15 * 60 * 1000,
		-- login attempts abuse
		maxloginattempts = 12,
		maxloginattemptspermilliseconds = 60 * 1000,
		loginattemptsabusebanmilliseconds = 15 * 60 * 1000,
		-- socket timeout
		socketreceivetimeoutmilliseconds = 500,
		socketsendtimeoutmilliseconds = 500,
		maxslowsockets = 2,
		maxslowsocketspermilliseconds = 60 * 1000,
		slowsocketsabusbanmilliseconds = 5 * 60 * 1000,
		-- invalid message abuse
		maxinvalidmessages = 2,
		maxinvalidmessagespermilliseconds = 60 * 1000,
		invalidmessagesabusebanmilliseconds = 15 * 60 * 1000,
		-- unknown packet abuse
		maxunknownpackets = 2,
		maxunknownpacketspermilliseconds = 60 * 1000,
		unknownpacketsabusebanmilliseconds = 15 * 60 * 1000
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