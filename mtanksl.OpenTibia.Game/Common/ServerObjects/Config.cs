namespace OpenTibia.Game
{
    public class Config
    {
        private Server server;

        public Config(Server server)
        {
            this.server = server;
        }

        private LuaScope script;

        public void Start()
        {
            script = server.LuaScripts.Create(server.PathResolver.GetFullPath("data/server/config.lua") );

            LoginMaxconnections = (int)(long)script["server.login.maxconnections"];
            LoginPort = (int)(long)script["server.login.port"];

            GameMaxConnections = (int)(long)script["server.game.maxconnections"];
            GamePort = (int)(long)script["server.game.port"];
            GameMaxPlayers = (int)(long)script["server.game.maxplayers"];

            SecurityMaxConnectionsWithSameIpAddress = (int)(long)script["server.security.maxconnectionswithsameipaddress"];
            SecurityConnectionsWithSameIpAddressAbuseBanMilliseconds = (int)(long)script["server.security.connectionswithsameipaddressabusebanmilliseconds"];

            SecurityMaxConnections = (int)(long)script["server.security.maxconnections"];
            SecurityMaxConnectionsPerMilliseconds = (int)(long)script["server.security.maxconnectionspermilliseconds"];
            SecurityConnectionsAbuseBanMilliseconds = (int)(long)script["server.security.connectionsabusebanmilliseconds"];

            SecurityMaxPackets = (int)(long)script["server.security.maxpackets"];
            SecurityMaxPacketsPerMilliseconds = (int)(long)script["server.security.maxpacketspermilliseconds"];                     
            SecurityPacketsAbuseBanMilliseconds = (int)(long)script["server.security.packetsabusebanmilliseconds"];

            SecurityMaxLoginAttempts = (int)(long)script["server.security.maxloginattempts"];
            SecurityMaxLoginAttemptsPerMilliseconds = (int)(long)script["server.security.maxloginattemptspermilliseconds"];
            SecurityLoginAttemptsAbuseBanMilliseconds = (int)(long)script["server.security.loginattemptsabusebanmilliseconds"];

            SocketReceiveTimeoutMilliseconds = (int)(long)script["server.security.socketreceivetimeoutmilliseconds"];           
            SocketSendTimeoutMilliseconds = (int)(long)script["server.security.socketsendtimeoutmilliseconds"];
            SecurityMaxSlowSockets = (int)(long)script["server.security.maxslowsockets"];
            SecurityMaxSlowSocketsPerMilliseconds = (int)(long)script["server.security.maxslowsocketspermilliseconds"];
            SecuritySlowSocketsAbuseBanMilliseconds = (int)(long)script["server.security.slowsocketsabusbanmilliseconds"];

            SecurityMaxInvalidMessages = (int)(long)script["server.security.maxinvalidmessages"];
            SecurityMaxInvalidMessagesPerMilliseconds = (int)(long)script["server.security.maxinvalidmessagespermilliseconds"];
            SecurityInvalidMessagesAbuseBanMilliseconds = (int)(long)script["server.security.invalidmessagesabusebanmilliseconds"];

            SecurityMaxUnknownPackets = (int)(long)script["server.security.maxunknownpackets"];
            SecurityMaxUnknownPacketsPerMilliseconds = (int)(long)script["server.security.maxunknownpacketspermilliseconds"];
            SecurityUnknownPacketsAbuseBanMilliseconds = (int)(long)script["server.security.unknownpacketsabusebanmilliseconds"];

            DatabaseType = (string)script["server.database.type"];
            DatabaseSource = (string)script["server.database.source"];
            DatabaseHost = (string)script["server.database.host"];
            DatabasePort = (int)(long)script["server.database.port"];
            DatabaseUser = (string)script["server.database.user"];
            DatabasePassword = (string)script["server.database.password"];
            DatabaseName = (string)script["server.database.name"];
        }

        public int LoginMaxconnections { get; set; }
        public int LoginPort { get; set; }

        public int GameMaxConnections { get; set; }
        public int GamePort { get; set; }
        public int GameMaxPlayers { get; set; }

        public int SecurityMaxConnectionsWithSameIpAddress { get; set; }
        public int SecurityConnectionsWithSameIpAddressAbuseBanMilliseconds { get; set; }

        public int SecurityMaxConnections { get; set; }
        public int SecurityMaxConnectionsPerMilliseconds { get; set; }
        public int SecurityConnectionsAbuseBanMilliseconds { get; set; }

        public int SecurityMaxPackets { get; set; }
        public int SecurityMaxPacketsPerMilliseconds { get; set; }
        public int SecurityPacketsAbuseBanMilliseconds { get; set; }

        public int SecurityMaxLoginAttempts { get; set; }
        public int SecurityMaxLoginAttemptsPerMilliseconds { get; set; }
        public int SecurityLoginAttemptsAbuseBanMilliseconds { get; set; }

        public int SocketReceiveTimeoutMilliseconds { get; set; }
        public int SocketSendTimeoutMilliseconds { get; set; }
        public int SecurityMaxSlowSockets { get; set; }
        public int SecurityMaxSlowSocketsPerMilliseconds { get; set; }
        public int SecuritySlowSocketsAbuseBanMilliseconds { get; set; }

        public int SecurityMaxInvalidMessages { get; set; }
        public int SecurityMaxInvalidMessagesPerMilliseconds { get; set; }
        public int SecurityInvalidMessagesAbuseBanMilliseconds { get; set; }

        public int SecurityMaxUnknownPackets { get; set; }
        public int SecurityMaxUnknownPacketsPerMilliseconds { get; set; }
        public int SecurityUnknownPacketsAbuseBanMilliseconds { get; set; }

        public string DatabaseType { get; set; }
        public string DatabaseSource { get; set; }
        public string DatabaseHost { get; set; }    
        public int DatabasePort { get; set; }    
        public string DatabaseUser { get; set; }    
        public string DatabasePassword { get; set; }    
        public string DatabaseName { get; set; }        

        public void Dispose()
        {
            script.Dispose();
        }
    }
}