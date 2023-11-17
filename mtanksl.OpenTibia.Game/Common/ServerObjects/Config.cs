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

            RateLimitingMaxConnections = (int)(long)script["server.security.maxconnections"];

            RateLimitingMaxConnectionsPerMilliseconds = (int)(long)script["server.security.maxconnectionspermilliseconds"];

            RateLimitingConnectionsAbuseBanMilliseconds = (int)(long)script["server.security.connectionsabusebanmilliseconds"];

            RateLimitingMaxPackets = (int)(long)script["server.security.maxpackets"];

            RateLimitingMaxPacketsPerMilliseconds = (int)(long)script["server.security.maxpacketspermilliseconds"];
                     
            RateLimitingPacketsAbuseBanMilliseconds = (int)(long)script["server.security.packetsabusebanmilliseconds"];

            RateLimitingMaxLoginAttempts = (int)(long)script["server.security.maxloginattempts"];

            RateLimitingMaxLoginAttemptsPerMilliseconds = (int)(long)script["server.security.maxloginattemptspermilliseconds"];

            RateLimitingLoginAttemptsAbuseBanMilliseconds = (int)(long)script["server.security.loginattemptsabusebanmilliseconds"];

            SocketReceiveTimeoutMilliseconds = (int)(long)script["server.security.socketreceivetimeoutmilliseconds"];
           
            SocketSendTimeoutMilliseconds = (int)(long)script["server.security.socketsendtimeoutmilliseconds"];

            RateLimitingMaxSlowSockets = (int)(long)script["server.security.maxslowsockets"];

            RateLimitingMaxSlowSocketsPerMilliseconds = (int)(long)script["server.security.maxslowsocketspermilliseconds"];

            RateLimitingSlowSocketsAbuseBanMilliseconds = (int)(long)script["server.security.slowsocketsabusbanmilliseconds"];

            RateLimitingMaxConnectionsWithSameIpAddress = (int)(long)script["server.security.maxconnectionswithsameipaddress"];

            RateLimitingConnectionsWithSameIpAddressAbuseBanMilliseconds = (int)(long)script["server.security.connectionswithsameipaddressabusebanmilliseconds"];

            DatabaseType = (string)script["server.database.type"];

            DatabaseSource = (string)script["server.database.source"];

            DatabaseHost = (string)script["server.database.host"];

            DatabasePort = (int)(long)script["server.database.port"];

            DatabaseUser = (string)script["server.database.user"];

            DatabasePassword = (string)script["server.database.password"];

            DatabaseName = (string)script["server.database.name"];
        }

        public int LoginMaxconnectionsWithSameIpAddress { get; set; }

        public int LoginMaxconnections { get; set; }

        public int LoginPort { get; set; }

        public int GameMaxconnectionsWithSameIpAddress { get; set; }

        public int GameMaxConnections { get; set; }

        public int GamePort { get; set; }

        public int GameMaxPlayers { get; set; }

        public int RateLimitingMaxConnections { get; set; }

        public int RateLimitingMaxConnectionsPerMilliseconds { get; set; }

        public int RateLimitingConnectionsAbuseBanMilliseconds { get; set; }

        public int RateLimitingMaxPackets { get; set; }

        public int RateLimitingMaxPacketsPerMilliseconds { get; set; }

        public int RateLimitingPacketsAbuseBanMilliseconds { get; set; }

        public int RateLimitingMaxLoginAttempts { get; set; }

        public int RateLimitingMaxLoginAttemptsPerMilliseconds { get; set; }

        public int RateLimitingLoginAttemptsAbuseBanMilliseconds { get; set; }

        public int SocketReceiveTimeoutMilliseconds { get; set; }

        public int SocketSendTimeoutMilliseconds { get; set; }

        public int RateLimitingMaxSlowSockets { get; set; }

        public int RateLimitingMaxSlowSocketsPerMilliseconds { get; set; }

        public int RateLimitingSlowSocketsAbuseBanMilliseconds { get; set; }

        public int RateLimitingMaxConnectionsWithSameIpAddress { get; set; }

        public int RateLimitingConnectionsWithSameIpAddressAbuseBanMilliseconds { get; set; }

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