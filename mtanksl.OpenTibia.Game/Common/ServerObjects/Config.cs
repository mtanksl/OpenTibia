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

            LoginPort = (int)(long)script["server.login.port"];

            GamePort = (int)(long)script["server.game.port"];

            GameMaxPlayers = (int)(long)script["server.game.maxplayers"];

            RateLimitingMaxPackets = (int)(long)script["server.security.maxpackets"];

            RateLimitingMaxPacketsPerMilliseconds = (int)(long)script["server.security.maxpacketspermilliseconds"];
                     
            RateLimitingPacketsAbuseBanMilliseconds = (int)(long)script["server.security.packetsabusebanmilliseconds"];

            RateLimitingMaxLoginAttempts = (int)(long)script["server.security.maxloginattempts"];

            RateLimitingMaxLoginAttemptsPerMilliseconds = (int)(long)script["server.security.maxloginattemptspermilliseconds"];

            RateLimitingLoginAttemptsAbuseBanMilliseconds = (int)(long)script["server.security.loginattemptsabusebanmilliseconds"];

            SocketReceiveTimeoutMilliseconds = (int)(long)script["server.security.socketreceivetimeoutmilliseconds"];
           
            SocketSendTimeoutMilliseconds = (int)(long)script["server.security.socketsendtimeoutmilliseconds"];

            DatabaseType = (string)script["server.database.type"];

            DatabaseSource = (string)script["server.database.source"];

            DatabaseHost = (string)script["server.database.host"];

            DatabasePort = (int)(long)script["server.database.port"];

            DatabaseUser = (string)script["server.database.user"];

            DatabasePassword = (string)script["server.database.password"];

            DatabaseName = (string)script["server.database.name"];
        }

        public int LoginPort { get; set; }

        public int GamePort { get; set; }

        public int GameMaxPlayers { get; set; }

        public int RateLimitingMaxPackets { get; set; }

        public int RateLimitingMaxPacketsPerMilliseconds { get; set; }

        public int RateLimitingPacketsAbuseBanMilliseconds { get; set; }

        public int RateLimitingMaxLoginAttempts { get; set; }

        public int RateLimitingMaxLoginAttemptsPerMilliseconds { get; set; }

        public int RateLimitingLoginAttemptsAbuseBanMilliseconds { get; set; }

        public int SocketReceiveTimeoutMilliseconds { get; set; }

        public int SocketSendTimeoutMilliseconds { get; set; }

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