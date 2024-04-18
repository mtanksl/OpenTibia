namespace OpenTibia.Game.Common.ServerObjects
{
    public class Config : IConfig
    {
        private IServer server;

        public Config(IServer server)
        {
            this.server = server;
        }

        public int LoginMaxconnections { get; set; }
        public int LoginPort { get; set; }

        public int GameMaxConnections { get; set; }
        public int GamePort { get; set; }
        public int GameMaxPlayers { get; set; }

        public bool GameplayPrivateNpcSystem { get; set; }
        public bool LearnSpellFirst { get; set; }
        public bool GameplayInfinitePotions { get; set; }
        public bool GameplayInfiniteArrows { get; set; }
        public bool GameplayInfiniteRunes { get; set; }
        public int GameplayMaxVips { get; set; }
        public int GameplayMaxDepotItems { get; set; }

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

        private LuaScope script;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/server/config.lua"),
                server.PathResolver.GetFullPath("data/server/lib.lua"),
                server.PathResolver.GetFullPath("data/lib.lua") );

            LoginMaxconnections = LuaScope.GetInt32(script["server.login.maxconnections"], 1000);
            
            LoginPort = LuaScope.GetInt32(script["server.login.port"], 7171);
                        
            GameMaxConnections = LuaScope.GetInt32(script["server.game.maxconnections"], 1100);
            
            GamePort = LuaScope.GetInt32(script["server.game.port"], 7172);
            
            GameMaxPlayers = LuaScope.GetInt32(script["server.game.maxplayers"], 1000);
                        
            GameplayPrivateNpcSystem = LuaScope.GetBoolean(script["server.gameplay.privatenpcsystem"], true);

            LearnSpellFirst = LuaScope.GetBoolean(script["server.gameplay.learnspellfirst"], false);

            GameplayInfinitePotions = LuaScope.GetBoolean(script["server.gameplay.infinitepotions"], false);
            
            GameplayInfiniteArrows = LuaScope.GetBoolean(script["server.gameplay.infinitearrows"], false);
            
            GameplayInfiniteRunes = LuaScope.GetBoolean(script["server.gameplay.infiniterunes"], false);

            GameplayMaxVips = LuaScope.GetInt32(script["server.gameplay.maxvips"], 100);

            GameplayMaxDepotItems = LuaScope.GetInt32(script["server.gameplay.maxdepotitems"], 2000);

            SecurityMaxConnectionsWithSameIpAddress = LuaScope.GetInt32(script["server.security.maxconnectionswithsameipaddress"], 2);
            
            SecurityConnectionsWithSameIpAddressAbuseBanMilliseconds = LuaScope.GetInt32(script["server.security.connectionswithsameipaddressabusebanmilliseconds"], 15 * 60 * 1000);

            SecurityMaxConnections = LuaScope.GetInt32(script["server.security.maxconnections"], 2);
            
            SecurityMaxConnectionsPerMilliseconds = LuaScope.GetInt32(script["server.security.maxconnectionspermilliseconds"], 1 * 1000);
            
            SecurityConnectionsAbuseBanMilliseconds = LuaScope.GetInt32(script["server.security.connectionsabusebanmilliseconds"], 15 * 60 * 1000);

            SecurityMaxPackets = LuaScope.GetInt32(script["server.security.maxpackets"], 60);
            
            SecurityMaxPacketsPerMilliseconds = LuaScope.GetInt32(script["server.security.maxpacketspermilliseconds"], 1 * 1000);
            
            SecurityPacketsAbuseBanMilliseconds = LuaScope.GetInt32(script["server.security.packetsabusebanmilliseconds"], 15 * 60 * 1000);

            SecurityMaxLoginAttempts = LuaScope.GetInt32(script["server.security.maxloginattempts"], 12);            
            
            SecurityMaxLoginAttemptsPerMilliseconds = LuaScope.GetInt32(script["server.security.maxloginattemptspermilliseconds"], 60 * 1000);
            
            SecurityLoginAttemptsAbuseBanMilliseconds = LuaScope.GetInt32(script["server.security.loginattemptsabusebanmilliseconds"], 15 * 60 * 1000);

            SocketReceiveTimeoutMilliseconds = LuaScope.GetInt32(script["server.security.socketreceivetimeoutmilliseconds"], 500);
            
            SocketSendTimeoutMilliseconds = LuaScope.GetInt32(script["server.security.socketsendtimeoutmilliseconds"], 500);
            
            SecurityMaxSlowSockets = LuaScope.GetInt32(script["server.security.maxslowsockets"], 2);
            
            SecurityMaxSlowSocketsPerMilliseconds = LuaScope.GetInt32(script["server.security.maxslowsocketspermilliseconds"], 60 * 1000);
            
            SecuritySlowSocketsAbuseBanMilliseconds = LuaScope.GetInt32(script["server.security.slowsocketsabusbanmilliseconds"], 15 * 60 * 1000);
                        
            SecurityMaxInvalidMessages = LuaScope.GetInt32(script["server.security.maxinvalidmessages"], 2);
            
            SecurityMaxInvalidMessagesPerMilliseconds = LuaScope.GetInt32(script["server.security.maxinvalidmessagespermilliseconds"], 60 * 1000);
            
            SecurityInvalidMessagesAbuseBanMilliseconds = LuaScope.GetInt32(script["server.security.invalidmessagesabusebanmilliseconds"], 15 * 60 * 1000);

            SecurityMaxUnknownPackets = LuaScope.GetInt32(script["server.security.maxunknownpackets"], 2);
            
            SecurityMaxUnknownPacketsPerMilliseconds = LuaScope.GetInt32(script["server.security.maxunknownpacketspermilliseconds"], 60 * 1000);
            
            SecurityUnknownPacketsAbuseBanMilliseconds = LuaScope.GetInt32(script["server.security.unknownpacketsabusebanmilliseconds"], 15 * 60 * 1000);

            DatabaseType = LuaScope.GetString(script["server.database.type"], "sqlite");

            DatabaseSource = LuaScope.GetString(script["server.database.source"], "data/database.db");

            DatabaseHost = LuaScope.GetString(script["server.database.host"], "localhost");

            DatabasePort = LuaScope.GetInt32(script["server.database.port"], 3306);

            DatabaseUser = LuaScope.GetString(script["server.database.user"], "root");

            DatabasePassword = LuaScope.GetString(script["server.database.password"], "");

            DatabaseName = LuaScope.GetString(script["server.database.name"], "mtots");
        }

        public object GetValue(string key)
        {
            return script[key];
        }

        public void Dispose()
        {
            script.Dispose();
        }
    }
}