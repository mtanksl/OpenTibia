using OpenTibia.Game;
using System;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class RateLimiting
    {
        private class RateLimitItem
        {
            public int ActiveConnections { get; set; }

            public DateTime LastConnection { get; set; }
            public int ConnectionCount { get; set; }

            public DateTime LastPacket { get; set; } 
            public int PacketCount { get; set; }

            public DateTime LastLoginAttempts { get; set; }
            public int LoginAttemptsCount { get; set; }

            public DateTime LastSlowSocket { get; set; }
            public int SlowSocketCount { get; set; }

            public DateTime LastInvalidMessage { get; set; }
            public int InvalidMessageCount { get; set; }

            public DateTime LastUnknownPacket { get; set; }
            public int UnknownPacketCount { get; set; }

            public DateTime BanTimeout { get; set; }
        }

        private readonly object sync = new object();

        private Server server;

        public RateLimiting(Server server)
        {
            this.server = server;
        }

        private Dictionary<string, RateLimitItem> items = new Dictionary<string, RateLimitItem>();

        public bool IncreaseActiveConnection(string ipAddress)
        {
            lock (sync)
            {
                RateLimitItem item;

                if ( !items.TryGetValue(ipAddress, out item) )
                {
                    item = new RateLimitItem();

                    items.Add(ipAddress, item);
                }

                if (DateTime.UtcNow < item.BanTimeout)
                {
                    return false;
                }

                item.ActiveConnections++;

                if (item.ActiveConnections > server.Config.SecurityMaxConnectionsWithSameIpAddress)
                {
                    item.BanTimeout = DateTime.UtcNow.AddMilliseconds(server.Config.SecurityConnectionsWithSameIpAddressAbuseBanMilliseconds);

                    return false;
                }

                return true;
            }
        }

        public void DecreaseActiveConnection(string ipAddress)
        {
            lock (sync)
            {
                RateLimitItem item;

                if ( !items.TryGetValue(ipAddress, out item) )
                {
                    item = new RateLimitItem();

                    items.Add(ipAddress, item);
                }

                if (DateTime.UtcNow < item.BanTimeout)
                {
                    return;
                }

                item.ActiveConnections--;

                return;
            }
        }

        public bool IsConnectionCountOk(string ipAddress)
        {
            lock (sync)
            {
                RateLimitItem item;

                if ( !items.TryGetValue(ipAddress, out item) )
                {
                    item = new RateLimitItem();

                    items.Add(ipAddress, item);
                }

                if (DateTime.UtcNow < item.BanTimeout)
                {
                    return false;
                }

                if ( (DateTime.UtcNow - item.LastConnection).TotalMilliseconds > server.Config.SecurityMaxConnectionsPerMilliseconds)
                {
                    item.LastConnection = DateTime.UtcNow;

                    item.ConnectionCount = 1;
                }
                else
                {
                    item.ConnectionCount++;
                }

                if (item.ConnectionCount > server.Config.SecurityMaxConnections)
                {
                    item.BanTimeout = DateTime.UtcNow.AddMilliseconds(server.Config.SecurityConnectionsAbuseBanMilliseconds);

                    return false;
                }

                return true;
            }
        }

        public bool IsPacketCountOk(string ipAddress)
        {
            lock (sync)
            {
                RateLimitItem item;

                if ( !items.TryGetValue(ipAddress, out item) )
                {
                    item = new RateLimitItem();

                    items.Add(ipAddress, item);
                }

                if (DateTime.UtcNow < item.BanTimeout)
                {
                    return false;
                }

                if ( (DateTime.UtcNow - item.LastPacket).TotalMilliseconds > server.Config.SecurityMaxPacketsPerMilliseconds)
                {
                    item.LastPacket = DateTime.UtcNow;

                    item.PacketCount = 1;
                }
                else
                {
                    item.PacketCount++;
                }

                if (item.PacketCount > server.Config.SecurityMaxPackets)
                {
                    item.BanTimeout = DateTime.UtcNow.AddMilliseconds(server.Config.SecurityPacketsAbuseBanMilliseconds);

                    return false;
                }

                return true;
            }            
        }
                
        public bool IsLoginAttempsOk(string ipAddress)
        {
            lock (sync)
            {
                RateLimitItem item;

                if ( !items.TryGetValue(ipAddress, out item) )
                {
                    item = new RateLimitItem();

                    items.Add(ipAddress, item);
                }

                if (DateTime.UtcNow < item.BanTimeout)
                {
                    return false;
                }

                if ( (DateTime.UtcNow - item.LastLoginAttempts).TotalMilliseconds > server.Config.SecurityMaxLoginAttemptsPerMilliseconds)
                {
                    item.LastLoginAttempts = DateTime.UtcNow;

                    item.LoginAttemptsCount = 1;
                }
                else
                {
                    item.LoginAttemptsCount++;
                }

                if (item.LoginAttemptsCount > server.Config.SecurityMaxLoginAttempts)
                {
                    item.BanTimeout = DateTime.UtcNow.AddMilliseconds(server.Config.SecurityLoginAttemptsAbuseBanMilliseconds);

                    return false;
                }

                return true;              
            }
        }

        public bool IncreaseSlowSocket(string ipAddress)
        {
            lock (sync)
            {
                RateLimitItem item;

                if ( !items.TryGetValue(ipAddress, out item) )
                {
                    item = new RateLimitItem();

                    items.Add(ipAddress, item);
                }

                if (DateTime.UtcNow < item.BanTimeout)
                {
                    return false;
                }

                if ( (DateTime.UtcNow - item.LastSlowSocket).TotalMilliseconds > server.Config.SecurityMaxSlowSocketsPerMilliseconds)
                {
                    item.LastSlowSocket = DateTime.UtcNow;

                    item.SlowSocketCount = 1;
                }
                else
                {
                    item.SlowSocketCount++;
                }

                if (item.SlowSocketCount > server.Config.SecurityMaxSlowSockets)
                {
                    item.BanTimeout = DateTime.UtcNow.AddMilliseconds(server.Config.SecuritySlowSocketsAbuseBanMilliseconds);

                    return false;
                }

                return true;              
            }
        }
  
        public bool IncreaseInvalidMessage(string ipAddress)
        {
            lock (sync)
            {
                RateLimitItem item;

                if ( !items.TryGetValue(ipAddress, out item) )
                {
                    item = new RateLimitItem();

                    items.Add(ipAddress, item);
                }

                if (DateTime.UtcNow < item.BanTimeout)
                {
                    return false;
                }

                if ( (DateTime.UtcNow - item.LastInvalidMessage).TotalMilliseconds > server.Config.SecurityMaxInvalidMessagesPerMilliseconds)
                {
                    item.LastInvalidMessage = DateTime.UtcNow;

                    item.InvalidMessageCount = 1;
                }
                else
                {
                    item.InvalidMessageCount++;
                }

                if (item.InvalidMessageCount > server.Config.SecurityMaxInvalidMessages)
                {
                    item.BanTimeout = DateTime.UtcNow.AddMilliseconds(server.Config.SecurityInvalidMessagesAbuseBanMilliseconds);

                    return false;
                }

                return true;              
            }
        }

        public bool IncreaseUnknownPacket(string ipAddress)
        {
            lock (sync)
            {
                RateLimitItem item;

                if ( !items.TryGetValue(ipAddress, out item) )
                {
                    item = new RateLimitItem();

                    items.Add(ipAddress, item);
                }

                if (DateTime.UtcNow < item.BanTimeout)
                {
                    return false;
                }

                if ( (DateTime.UtcNow - item.LastUnknownPacket).TotalMilliseconds > server.Config.SecurityMaxUnknownPacketsPerMilliseconds)
                {
                    item.LastUnknownPacket = DateTime.UtcNow;

                    item.UnknownPacketCount = 1;
                }
                else
                {
                    item.UnknownPacketCount++;
                }

                if (item.UnknownPacketCount > server.Config.SecurityMaxUnknownPackets)
                {
                    item.BanTimeout = DateTime.UtcNow.AddMilliseconds(server.Config.SecurityUnknownPacketsAbuseBanMilliseconds);

                    return false;
                }

                return true;
            }
        }
    }
}