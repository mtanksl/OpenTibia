using System;
using System.Collections.Generic;
using System.Threading;

namespace OpenTibia.Game.Common
{
    //Reference: https://stackoverflow.com/a/2572919

    public enum LoadingMode 
    {
        Eager,

        Lazy
    };

    public enum BoundMode
    {
        Bounded,

        Unbounded
    }

    public enum AccessMode 
    {
        FIFO, 

        LIFO
    };

    public class Pool<T> : IDisposable
    {                
        private interface IItemStore
        {
            int Count { get; }

            void Store(T item);

            T Fetch();
        }

        private class QueueStore : Queue<T>, IItemStore
        {
            public QueueStore() : base()
            {

            }

            public QueueStore(int capacity) : base(capacity)
            {

            }

            public void Store(T item)
            {
                Enqueue(item);
            }

            public T Fetch()
            {
                return Dequeue();
            }            
        }

        private class StackStore : Stack<T>, IItemStore
        {
            public StackStore() : base()
            {

            }

            public StackStore(int capacity) : base(capacity)
            {

            }

            public void Store(T item)
            {
                Push(item);
            }

            public T Fetch()
            {
                return Pop();
            }            
        }

        private Semaphore sync;

        private Func<Pool<T>, T> factory;

        private LoadingMode loadingMode;

        private BoundMode boundMode;

        private IItemStore itemStore;

        public Pool(Func<Pool<T>, T> factory) : this(-1, factory, LoadingMode.Lazy, BoundMode.Unbounded, AccessMode.FIFO)
        {

        }

        public Pool(int maxCount, Func<Pool<T>, T> factory, LoadingMode loadingMode, BoundMode boundMode, AccessMode accessMode)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory) );
            }

            if (maxCount <= 0 && (loadingMode == LoadingMode.Eager || boundMode == BoundMode.Bounded) )
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount), maxCount, "Argument must be greater than zero.");
            }

            if (boundMode == BoundMode.Bounded)
            {
                this.sync = new Semaphore(maxCount, maxCount);
            }

            this.factory = factory;

            this.loadingMode = loadingMode;

            this.boundMode = boundMode;

            if (accessMode == AccessMode.FIFO)
            {
                if (loadingMode == LoadingMode.Eager || boundMode == BoundMode.Bounded)
                {
                    this.itemStore = new QueueStore(maxCount);
                }
                else
                {
                    this.itemStore = new QueueStore();
                }
            }
            else
            {
                if (loadingMode == LoadingMode.Eager || boundMode == BoundMode.Bounded)
                {
                    this.itemStore = new StackStore(maxCount);
                }
                else
                {
                    this.itemStore = new StackStore();
                }
            }

            if (loadingMode == LoadingMode.Eager)
            {
                for (int i = 0; i < maxCount; i++)
                {
                    T item = factory(this);

                    this.itemStore.Store(item);
                }
            }
        }

        ~Pool()
        {
            Dispose(false);
        }

        public T Acquire()
        {
            if (boundMode == BoundMode.Bounded)
            {
                sync.WaitOne();
            }

            lock (itemStore)
            {
                if (itemStore.Count > 0)
                {
                    return itemStore.Fetch();
                }
            }

            return factory(this);
        }

        public void Release(T item)
        {
            lock (itemStore)
            {
                itemStore.Store(item);
            }

            if (boundMode == BoundMode.Bounded)
            {
                sync.Release();
            }
        }        

        private bool disposed = false;

        public bool Disposed
        {
            get
            {
                return disposed;
            }
        }

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
                    if (typeof(IDisposable).IsAssignableFrom(typeof(T) ) )
                    {
                        lock (itemStore)
                        {
                            while (itemStore.Count > 0)
                            {
                                T item = itemStore.Fetch();

                                ( (IDisposable)item ).Dispose();
                            }
                        }
                    }

                    if (boundMode == BoundMode.Bounded)
                    {
                        sync.Close();
                    }
                }
            }
        }
    }
}