using OpenTibia.Game;
using System;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class RateLimiting
    {
        private class RateLimitItem
        {
            public DateTime LastPacket { get; set; }

            public int Count { get; set; }

            public DateTime BanTimeout { get; set; }
        }

        private readonly object sync = new object();

        private Server server;

        public RateLimiting(Server server)
        {
            this.server = server;
        }

        private Dictionary<string, RateLimitItem> items = new Dictionary<string, RateLimitItem>();

        public bool CanReceive(string ipAddress)
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

                if ( (DateTime.UtcNow - item.LastPacket).TotalMilliseconds > server.Config.RateLimitingMilliseconds)
                {
                    item.LastPacket = DateTime.UtcNow;

                    item.Count = 0;
                }
                else
                {
                    item.Count++;

                    if (item.Count > server.Config.RateLimitingMaxPackets)
                    {
                        item.BanTimeout = DateTime.UtcNow.AddMilliseconds(server.Config.RateLimitingBanMilliseconds);

                        return false;
                    }
                }

                return true;
            }            
        }
    }
}