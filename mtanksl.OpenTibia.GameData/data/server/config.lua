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
		},
		rules = nil
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
			allowchangeplayername = false,
			allowchangeplayergender = false,
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
			-- true = runes disappear after use (Tibia)
			-- false = runes do not disappear after use
			removechargesfromrunes = true,
			-- true = ammunitions disappear after attack (Tibia)
			-- false = ammunitions do not disappear after attack
			removeweaponammunition = true,
			-- true = weapons disappear after attack (Tibia)
			-- false = weapons do not disappear after attack
			removeweaponcharges = true,
			-- true = amulets and rings disappear after defense (Tibia)
			-- false = amulets and rings do not disappear after defense 
			removearmorcharges = true,
			-- pvp or non-pvp
			worldtype = "non-pvp",
			protectionlevel = 0, -- When using pvp
			-- When you attack or get attacked by monsters or people, you will receive logout block for 60 seconds.
			logoutblockseconds = 1 * 60,
			-- If you kill another player, no matter if it's justified or not, you will receive logout block and protection zone block for 15 minutes.
			protectionzoneblockseconds = 15 * 60,
			-- -1 means use default formula (Tibia)
			-- 0 means 0%, or no experience loss and no skill loss
			-- 10 means 10%, as it was before Mar 10 2009 on version 8.41
			deathlosepercent = -1,
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
			magiclevelrate = 1.0,
			skillrate = 1.0,
			experiencestages = {
				enabled = false,
				levels = {
					{ minlevel = 1, maxlevel = 8, multiplier = 10.0 }
				}
			},
			rooking = {
				enabled = true,
				experiencethreshold = 1500,
				playernewposition = { x = 921, y = 771, z = 6 }
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
		maxloginattemptspermilliseconds = 1 * 60 * 1000,
		loginattemptsabusebanmilliseconds = 15 * 60 * 1000,
		-- socket timeout
		socketreceivetimeoutmilliseconds = 500,
		socketsendtimeoutmilliseconds = 500,
		maxslowsockets = 2,
		maxslowsocketspermilliseconds = 1 * 60 * 1000,
		slowsocketsabusbanmilliseconds = 5 * 60 * 1000,
		-- invalid message abuse
		maxinvalidmessages = 2,
		maxinvalidmessagespermilliseconds = 1 * 60 * 1000,
		invalidmessagesabusebanmilliseconds = 15 * 60 * 1000,
		-- unknown packet abuse
		maxunknownpackets = 2,
		maxunknownpacketspermilliseconds = 1 * 60 * 1000,
		unknownpacketsabusebanmilliseconds = 15 * 60 * 1000
	},
	database = {
		-- sqlite, mysql, mssql, postgresql or memory
		type = "sqlite", 

		source = "data/database.db", -- When using sqlite

		host = "localhost", -- When using mysql, mssql or postgresql
		port = 3306,
		user = "root",
		password = "",
		name = "mtots",

		overrideconnectionstring = nil
	}
}

server.info.rules = [[
	Tibia is an online role-playing game in which thousands of players from all over the world meet everyday.
	In order to ensure that the game is fun for everyone, we expect all players to behave in a reasonable and respectful manner.
	We reserve the right to stop destructive behaviour in the game, on the official website or in any other part of our services.
	Such behaviour includes, but is not limited to, the following offences:

	1. Names

	a) Offensive Name
	Names that are insulting, racist, sexually related, drug-related, harassing or generally objectionable.

	b) Name Containing Forbidden Advertising
	Names that advertise brands, products or services of third parties, content which is not related to the game or trades for real money.

	c) Unsuitable Name
	Names that express religious or political views.

	d) Name Supporting Rule Violation
	Names that support, incite, announce or imply a violation of the Tibia Rules.

	2. Statements

	a) Offensive Statement
	Insulting, racist, sexually related, drug-related, harassing or generally objectionable statements.

	b) Spamming
	Excessively repeating identical or similar statements or using badly formatted or nonsensical text.

	c) Forbidden Advertising
	Advertising brands, products or services of third parties, content which is not related to the game or trades for real money.

	d) Off-Topic Public Statement
	Religious or political public statements or other public statements which are not related to the topic of the used channel or board.

	e) Violating Language Restriction
	Non-English statements in boards and channels where the use of English is explicitly required.

	f) Disclosing Personal Data of Others
	Disclosing personal data of other people.

	g) Supporting Rule Violation
	Statements that support, incite, announce or imply a violation of the Tibia Rules.
 
	3. Cheating

	a) Bug Abuse
	Exploiting obvious errors of the game or any other part of our services.

	b) Using Unofficial Software to Play
	Manipulating the official client program or using additional software to play the game.
 
	4. Legal Issues

	a) Hacking
	Stealing other players' account or personal data.

	b) Attacking Service
	Attacking, disrupting or damaging the operation of any server, the game or any other part of our services.

	c) Violating Law or Regulations
	Violating any applicable law, the Service Agreement or rights of third parties.

	Violating or attempting to violate the Tibia Rules may lead to a temporary suspension of characters and accounts.
	In severe cases the removal or modification of character skills, attributes and belongings, as well as the permanent removal of characters and accounts without any compensation may be considered.
	The sanction is based on the seriousness of the rule violation and the previous record of the player.
	It can be imposed without any previous warning.
	These rules may be changed at any time.
]]