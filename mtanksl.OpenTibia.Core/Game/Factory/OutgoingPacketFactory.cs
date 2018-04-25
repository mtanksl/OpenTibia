using System;
using System.Runtime.Caching;

namespace OpenTibia
{
    public class OutgoingPacketFactory
    {
        private MemoryCache cache;

        private CacheItemPolicy policy;

        public OutgoingPacketFactory()
        {
            this.cache = MemoryCache.Default;

            this.policy = new CacheItemPolicy()
            {
                SlidingExpiration = TimeSpan.FromMinutes(5)
            };
        }

        public IOutgoingPacket Create(string key, Func<IOutgoingPacket> callback)
        {
            if ( !cache.Contains(key) )
            {
                IOutgoingPacket packet = new CachedOutgoingPacket( callback() );

                cache.Add(new CacheItem(key, packet), policy);

                return packet;
            }

            return (IOutgoingPacket)cache.Get(key);
        }   
    }
}