IP_ADDRESS = "127.0.0.1"
LOGIN_PORT = 7171
GAME_PORT = 7172
INFO_PORT = 7173

server = {
	info = {
		maxconnections = 1,
		port = INFO_PORT,
		public = {
			servername = "MTOTS",
			ipaddress = IP_ADDRESS,
			port = LOGIN_PORT,
			location = "",
			url = "",
			ownername = "",
			owneremail = "",
			mapname = "map",
			mapauthor = ""
		}
	},
	login = {
		maxconnections = 100,
		port = LOGIN_PORT,
		motd = "MTOTS - An open Tibia server developed by mtanksl",
		worlds = {
			["Cormaya"] = {
				ipaddress = IP_ADDRESS,
				port = GAME_PORT
			}
		}
	},
	game = {
		maxconnections = 1100,
		port = GAME_PORT,
		gameplay = {
			maxplayers = 1000,
			privatenpcsystem = true,
			learnspellfirst = false,
			infinitepotions = false,
			infinitearrows = false,
			infiniterunes = false,
			maxvips = 100,
			maxdepotitems = 2000,
			kicklostconnectionafterminutes = 1,
			kickidleplayerafterminutes = 15,
			monsterdespawnrange = 2,
			monsterdespawnradius = 50,
			lootrate = 1,
			experiencerate = 1
		}
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