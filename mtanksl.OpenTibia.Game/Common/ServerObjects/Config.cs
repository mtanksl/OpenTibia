using NLua;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class Config : IConfig
    {
        private IServer server;

        public Config(IServer server)
        {
            this.server = server;
        }

        ~Config()
        {
            Dispose(false);
        }

        public int InfoMaxConnections { get; set; }
        public int InfoPort { get; set; }

        public string ServerName { get; set; }
        public string IpAddress { get; set; }
        public int Port { get; set; }
        public string Location { get; set; }
        public string Url { get; set; }
        public string OwnerName { get; set; }
        public string OwnerEmail { get; set; }
        public string MapName { get; set; }
        public string MapAuthor { get; set; }
        public string Rules { get; set; }

        public int LoginMaxconnections { get; set; }
        public int LoginPort { get; set; }
        public bool LoginAccountManagerEnabled { get; set; }
        public bool LoginAccountManagerAllowChangePlayerName { get; set; }
        public bool LoginAccountManagerAllowChangePlayerGender { get; set; }
        public string LoginAccountManagerAccountName { get; set; }
        public string LoginAccountManagerAccountPassword { get; set; }
        public string LoginAccountManagerPlayerName { get; set; }
        public Position LoginAccountManagerPlayerPosition { get; set; }
        public Position LoginAccountManagerPlayerNewPosition { get; set; }
        public string LoginAccountManagerWorldName { get; set; }
        public string LoginAccountManagerIpAddress { get; set; }
        public int LoginAccountManagerPort { get; set; }

        public string Motd { get; set; }
        public DbWorld[] Worlds { get; set; }

        public int GameMaxConnections { get; set; }
        public int GamePort { get; set; }

        public int GameplayMaxPlayers { get; set; }
        public bool GameplayPrivateNpcSystem { get; set; }
        public bool GameplayLearnSpellFirst { get; set; }
        public bool GameplayRemoveChargesFromPotions { get; set; }
        public bool GameplayRemoveChargesFromRunes { get; set; }
        public bool GameplayRemoveWeaponAmmunition { get; set; }
        public bool GameplayRemoveWeaponCharges { get; set; }
        public bool GameplayAllowChangeOutfit { get; set; }
        public bool GameplayHotkeyAimbotEnabled { get; set; }
        public bool GameplayShowOnlineStatusInCharlist { get; set; }
        public bool GameplayAllowClones { get; set; }
        public bool GameplayOnePlayerOnlinePerAccount { get; set; }
        public bool GameplayReplaceKickOnLogin { get; set; }
        public int GameplayVipFreeLimit { get; set; }
        public int GameplayVipPremiumLimit { get; set; }
        public int GameplayDepotFreeLimit { get; set; }
        public int GameplayDepotPremiumLimit { get; set; }
        public int GameplayKickIdlePlayerAfterMinutes { get; set; }
        public int GameplayKickLostConnectionAfterMinutes { get; set; }
        public int GameplayMonsterDeSpawnRange { get; set; }
        public int GameplayMonsterDeSpawnRadius { get; set; }
        public bool GameplayMonsterRemoveOnDeSpawn { get; set; }
        public int GameplayLootRate { get; set; }
        public int GameplayMoneyRate { get; set; }
        public double GameplayExperienceRate { get; set; }
        public double GameplayMagicLevelRate { get; set; }
        public double GameplaySkillRate { get; set; }
        public ExperienceStagesConfig GameplayExperienceStages { get; set; }
        public RookingConfig GameplayRooking { get; set; }

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

        private ILuaScope script;

        public void Start()
        {
            script = server.LuaScripts.LoadScript(
                server.PathResolver.GetFullPath("data/server/config.lua"),
                server.PathResolver.GetFullPath("data/server/lib.lua"),
                server.PathResolver.GetFullPath("data/lib.lua") );

            InfoMaxConnections = LuaScope.GetInt32(script["server.info.maxconnections"], 1);

            InfoPort = LuaScope.GetInt32(script["server.info.port"], 7173);

            ServerName = LuaScope.GetString(script["server.info.public.servername"], "MTOTS");

            IpAddress = LuaScope.GetString(script["server.info.public.ipaddress"], "");

            Port = LuaScope.GetInt32(script["server.info.public.port"], 7171);

            Location = LuaScope.GetString(script["server.info.public.location"], "");

            Url = LuaScope.GetString(script["server.info.public.url"], "");

            OwnerName = LuaScope.GetString(script["server.info.public.ownername"], "");

            OwnerEmail = LuaScope.GetString(script["server.info.public.owneremail"], "");

            MapName = LuaScope.GetString(script["server.info.public.mapname"], "map");

            MapAuthor = LuaScope.GetString(script["server.info.public.mapauthor"], "");

            Rules = LuaScope.GetString(script["server.info.rules"], null);

            LoginMaxconnections = LuaScope.GetInt32(script["server.login.maxconnections"], 1000);  
            
            LoginPort = LuaScope.GetInt32(script["server.login.port"], 7171);

            LoginAccountManagerEnabled = LuaScope.GetBoolean(script["server.login.accountmanager.enabled"], true);

            LoginAccountManagerAllowChangePlayerName = LuaScope.GetBoolean(script["server.login.accountmanager.allowchangeplayername"], true);

            LoginAccountManagerAllowChangePlayerGender = LuaScope.GetBoolean(script["server.login.accountmanager.allowchangeplayergender"], true);

            LoginAccountManagerAccountName = LuaScope.GetString(script["server.login.accountmanager.accountname"], "");

            LoginAccountManagerAccountPassword = LuaScope.GetString(script["server.login.accountmanager.accountpassword"], "");

            LoginAccountManagerPlayerName = LuaScope.GetString(script["server.login.accountmanager.playername"], "");

            LuaTable position = (LuaTable)script["server.login.accountmanager.playerposition"];

            LoginAccountManagerPlayerPosition = new Position(LuaScope.GetInt32(position["x"] ), LuaScope.GetInt32(position["y"] ), LuaScope.GetInt32(position["z"] ) );

            LuaTable newPosition = (LuaTable)script["server.login.accountmanager.playernewposition"];

            LoginAccountManagerPlayerNewPosition = new Position(LuaScope.GetInt32(newPosition["x"] ), LuaScope.GetInt32(newPosition["y"] ), LuaScope.GetInt32(newPosition["z"] ) );

            LoginAccountManagerWorldName = LuaScope.GetString(script["server.login.accountmanager.worldname"], "");

            LoginAccountManagerIpAddress = LuaScope.GetString(script["server.login.accountmanager.ipaddress"], "127.0.0.1");

            try
            {
                var dns = Dns.GetHostEntry(LoginAccountManagerIpAddress);

                var ipv4 = dns.AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();

                if (ipv4 == null)
                {
                    throw new NotImplementedException("File config.lua parameter server.login.accountmanager.ipaddress could not be resolved to IPV4.");
                }

                LoginAccountManagerIpAddress = ipv4.ToString();
            }
            catch (SocketException)
            {
                throw new NotImplementedException("File config.lua parameter server.login.accountmanager.ipaddress could not be resolved to IPV4.");
            }

            LoginAccountManagerPort = LuaScope.GetInt32(script["server.login.accountmanager.port"], 7172);

            Motd = LuaScope.GetString(script["server.login.motd"], "MTOTS - An open Tibia server developed by mtanksl");

            LuaTable worlds = (LuaTable)script["server.login.worlds"]; 
            
            Worlds = worlds.Keys.Cast<string>().Select(key =>
            {
                string ipAddress = LuaScope.GetString( ( (LuaTable)worlds[key] )["ipaddress"] );

                int port = LuaScope.GetInt32( ( (LuaTable)worlds[key] )["port"] );

                try
                {
                    var dns = Dns.GetHostEntry(ipAddress);

                    var ipv4 = dns.AddressList.Where(a => a.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault();

                    if (ipv4 == null)
                    {
                        throw new NotImplementedException("File config.lua parameter server.login.words[\"" + key + "\"].ipaddress could not be resolved.");
                    }

                    ipAddress = ipv4.ToString();
                }
                catch (SocketException)
                {
                    throw new NotImplementedException("File config.lua parameter server.login.words[\"" + key + "\"].ipaddress could not be resolved.");
                }

                return new DbWorld()
                {
                    Name = key, 
                
                    Ip = ipAddress, 
                
                    Port = port 
                };

            } ).ToArray();

            GameMaxConnections = LuaScope.GetInt32(script["server.game.maxconnections"], 1100);

            GamePort = LuaScope.GetInt32(script["server.game.port"], 7172);

            GameplayMaxPlayers = LuaScope.GetInt32(script["server.game.gameplay.maxplayers"], 1000);

            GameplayPrivateNpcSystem = LuaScope.GetBoolean(script["server.game.gameplay.privatenpcsystem"], true);

            GameplayLearnSpellFirst = LuaScope.GetBoolean(script["server.game.gameplay.learnspellfirst"], false);

            GameplayRemoveChargesFromPotions = LuaScope.GetBoolean(script["server.game.gameplay.removechargesfrompotions"], true);

            GameplayRemoveChargesFromRunes = LuaScope.GetBoolean(script["server.game.gameplay.removechargesfromrunes"], true);

            GameplayRemoveWeaponAmmunition = LuaScope.GetBoolean(script["server.game.gameplay.removeweaponammunition"], true);

            GameplayRemoveWeaponCharges = LuaScope.GetBoolean(script["server.game.gameplay.removeweaponcharges"], true);

            GameplayAllowChangeOutfit = LuaScope.GetBoolean(script["server.game.gameplay.allowchangeoutfit"], true);

            GameplayHotkeyAimbotEnabled = LuaScope.GetBoolean(script["server.game.gameplay.hotkeyaimbotenabled"], true);

            GameplayShowOnlineStatusInCharlist = LuaScope.GetBoolean(script["server.game.gameplay.showOnlineStatusInCharlist"], false);

            GameplayAllowClones = LuaScope.GetBoolean(script["server.game.gameplay.allowclones"], false);

            GameplayOnePlayerOnlinePerAccount = LuaScope.GetBoolean(script["server.game.gameplay.oneplayeronlineperaccount"], false);

            GameplayReplaceKickOnLogin = LuaScope.GetBoolean(script["server.game.gameplay.replacekickonlogin"], false);

            GameplayVipFreeLimit = LuaScope.GetInt32(script["server.game.gameplay.vipfreelimit"], 20);

            GameplayVipPremiumLimit = LuaScope.GetInt32(script["server.game.gameplay.vippremiumlimit"], 100);

            GameplayDepotFreeLimit = LuaScope.GetInt32(script["server.game.gameplay.depotfreelimit"], 2000);

            GameplayDepotPremiumLimit = LuaScope.GetInt32(script["server.game.gameplay.depotpremiumlimit"], 15000);

            GameplayKickLostConnectionAfterMinutes = LuaScope.GetInt32(script["server.game.gameplay.kicklostconnectionafterminutes"], 1);

            GameplayKickIdlePlayerAfterMinutes = LuaScope.GetInt32(script["server.game.gameplay.kickidleplayerafterminutes"], 15);

            GameplayMonsterDeSpawnRange = LuaScope.GetInt32(script["server.game.gameplay.monsterdespawnrange"], 2);

            GameplayMonsterDeSpawnRadius = LuaScope.GetInt32(script["server.game.gameplay.monsterdespawnradius"], 50);

            GameplayMonsterRemoveOnDeSpawn = LuaScope.GetBoolean(script["server.game.gameplay.monsterremoveondespawn"], true);

            GameplayLootRate = LuaScope.GetInt32(script["server.game.gameplay.lootrate"], 1);

            GameplayMoneyRate = LuaScope.GetInt32(script["server.game.gameplay.moneyRate"], 1);

            GameplayExperienceRate = LuaScope.GetDouble(script["server.game.gameplay.experiencerate"], 1.0);

            GameplayMagicLevelRate = LuaScope.GetDouble(script["server.game.gameplay.magiclevelrate"], 1.0);

            GameplaySkillRate = LuaScope.GetDouble(script["server.game.gameplay.skillrate"], 1.0);

            LuaTable stages = (LuaTable)script["server.game.gameplay.experiencestages"];

            GameplayExperienceStages = new ExperienceStagesConfig()
            {
                Enabled = LuaScope.GetBoolean(stages["enabled"] ),

                Levels = ( (LuaTable)stages["levels"] ).Values.Cast<LuaTable>().Select(l => new LevelConfig() 
                { 
                    MinLevel = LuaScope.GetInt32(l["minlevel"] ),

                    MaxLevel = LuaScope.GetInt32(l["maxlevel"] ),

                    Multiplier = LuaScope.GetDouble(l["multiplier"] )

                } ).ToArray()
            };

            LuaTable rooking = (LuaTable)script["server.game.gameplay.rooking"];
                      
            LuaTable rookingPlayerNewPosition = (LuaTable)rooking["playernewposition"];

            GameplayRooking = new RookingConfig()
            {
                Enabled = LuaScope.GetBoolean(rooking["enabled"] ),

                ExperienceThreshold = LuaScope.GetUInt64(rooking["experiencethreshold"], 1500),

                PlayerNewPosition = new Position(LuaScope.GetInt32(rookingPlayerNewPosition["x"] ), LuaScope.GetInt32(rookingPlayerNewPosition["y"] ), LuaScope.GetInt32(rookingPlayerNewPosition["z"] ) )
            };

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

        /// <exception cref="ObjectDisposedException"></exception>

        public object GetValue(string key)
        {
            if (disposed)
            {
                throw new ObjectDisposedException(nameof(Config) );
            }

            return script[key];
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                disposed = true;

                if (disposing)
                {
                    if (script != null)
                    {
                        script.Dispose();
                    }
                }
            }
        }
    }
}