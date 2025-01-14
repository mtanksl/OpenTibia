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
			mapname = "map", -- data/world/map.otbm
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
		},
		accountmanager = {
			enabled = true,
			allowchangeplayername = true,
			allowchangeplayergender = true,
			accountname = "",
			accountpassword = "",
			playername = "Account Manager",
			playerposition = { x = 915, y = 769, z = 6 },
			playernewposition = { x = 921, y = 771, z = 6 },
			worldname = "Cormaya",
			ipaddress = IP_ADDRESS,
			port = GAME_PORT
		}
	},
	game = {
		maxconnections = 1100,
		port = GAME_PORT,
		gameplay = {
			maxplayers = 1000,
			-- true = NPCs channel (Tibia)
			-- false = Default channel
			privatenpcsystem = true,
			-- true = need to learn the spell before using it (Tibia)
			-- false = no need to learn the spell before using it
			learnspellfirst = false,
			-- true = potions disappear after use (Tibia)
			-- false = potions do not disappear after use
			removechargesfrompotions = true,
			-- true = ammunitions disappear after attack (Tibia)
			-- false = ammunitions do not disappear after attack
			removeweaponammunition = true,
			-- true = runes disappear after use (Tibia)
			-- false = runes do not disappear after use
			removechargesfromrunes = true,
			-- true = weapons disappear after attack (Tibia)
			-- false = weapons do not disappear after attack
			removeweaponcharges = true,
			-- true = allow change outfit (Tibia)
			-- false = do not allow change outfit
			allowchangeoutfit = true,
			-- true = allow use with hotkey on creature (Tibia)
			-- false = do not allow use with hotkey on creature
			hotkeyaimbotenabled = true,
			-- true = show online/offline in character list instead of world name
			-- false = do not show online/offline in character list instead of world name (Tibia)
			showOnlineStatusInCharlist = false,
			-- true = allow multiple players on the same account/player
			-- false = do not allow multiple players on the same account/player (Tibia)
			allowclones = false,
			-- true = displays "You may only login with one character of your account at the same time." (Tibia)
			-- false = allow multiple players on the same account
			oneplayeronlineperaccount = false,
			-- true = replace client after logging into the same account/player
			-- false = displays "You are already logged in." (Tibia)
			replacekickonlogin = false,
			vipfreelimit = 20,
			vippremiumlimit = 100,
			depotfreelimit = 2000,
			depotpremiumlimit = 15000,
			-- kick player after 1 minute without connection
			kicklostconnectionafterminutes = 1,
			-- kick player after 15 minutes idle
			kickidleplayerafterminutes = 15,
			monsterdespawnrange = 2,
			monsterdespawnradius = 50,
			monsterremoveondespawn = true,
			lootrate = 1,
			moneyRate = 1,
			experiencerate = 1.0,
			experiencestages = {
				enabled = false,
				levels = {
					{ minlevel = 1, maxlevel = 8, multiplier = 10.0 }
				}
			}
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