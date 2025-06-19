using OpenTibia.Common.Objects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class MessageCollectionFactory : IMessageCollectionFactory
    {
        private class PooledMessageCollection : IMessageCollection
        {
            private Pool<IMessageCollection> pool;

            private MessageCollection messageCollection;

            public PooledMessageCollection(Pool<IMessageCollection> pool, MessageCollection messageCollection)
            {
                this.pool = pool;

                this.messageCollection = messageCollection;
            }

            public void Add(IOutgoingPacket packet, IHasFeatureFlag features)
            {
                messageCollection.Add(packet, features);
            }

            public IEnumerable<byte[]> GetMessages()
            {
                return messageCollection.GetMessages();
            }

            public void Dispose()
            {
                if (pool.Disposed)
                {
                    messageCollection.Dispose();
                }
                else
                {
                    pool.Release(this);
                }
            }
        }

        private Pool<IMessageCollection> pool;

        public MessageCollectionFactory()
        {
            pool = new Pool<IMessageCollection>(p => new PooledMessageCollection(p, new MessageCollection() ) );
        }

        ~MessageCollectionFactory()
        {
            Dispose(false);
        }

        public IMessageCollection Create()
        {
            return pool.Acquire();
        }

        private bool disposed = false;

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if ( !disposed )
            {
                disposed = true;

                if (disposing)
                {
                    if (pool != null)
                    {
                        pool.Dispose();
                    }
                }
            }
        } 
    }
}