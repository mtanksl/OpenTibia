dofile(getfullpath("../../../../mtanksl.OpenTibia.GameData/data/server/config.lua"))

-- Specify protocol
server.info.public.clientversion = "8.60"

-- Disable sockets
server.info.maxconnections = 0
server.login.maxconnections = 0
server.game.maxconnections = 0

-- Disable security checks
server.security.maxconnectionswithsameipaddress = 9999
server.security.maxconnections = 9999
server.security.maxpackets = 9999
server.security.maxloginattempts = 9999
server.security.maxloginattempts = 9999
server.security.maxinvalidmessages = 9999
server.security.maxunknownpackets = 9999

-- Enable in-memory database
server.database.type = "memory"
