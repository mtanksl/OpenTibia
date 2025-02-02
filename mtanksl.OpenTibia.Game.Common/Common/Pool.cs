using System;
using System.Collections.Generic;
using System.Threading;

namespace mtanksl.OpenTibia.Game.Common.Common
{
    //Reference: https://stackoverflow.com/a/2572919

    public enum LoadingMode 
    {
        Eager, 

        Lazy
    };

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

        private IItemStore itemStore;

        public Pool(int maxCount, Func<Pool<T>, T> factory)

            : this(maxCount, factory, LoadingMode.Lazy, AccessMode.FIFO)
        {

        }

        public Pool(int maxCount, Func<Pool<T>, T> factory, LoadingMode loadingMode, AccessMode accessMode)
        {
            if (maxCount <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxCount), maxCount, "Argument must be greater than zero.");
            }

            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory) );
            }

            this.sync = new Semaphore(maxCount, maxCount);

            this.factory = factory;

            this.loadingMode = loadingMode;

            switch (accessMode)
            {
                case AccessMode.FIFO:
                default:

                    this.itemStore = new QueueStore(maxCount);

                    break;

                case AccessMode.LIFO:

                    this.itemStore = new StackStore(maxCount);

                    break;
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
            sync.WaitOne();

            switch (loadingMode)
            {
                case LoadingMode.Eager:

                    lock (itemStore)
                    {
                        return itemStore.Fetch();
                    }

                case LoadingMode.Lazy:
                default:

                    lock (itemStore)
                    {
                        if (itemStore.Count > 0)
                        {
                            return itemStore.Fetch();
                        }
                    }

                    return factory(this);
            }
        }

        public void Release(T item)
        {
            lock (itemStore)
            {
                itemStore.Store(item);
            }

            sync.Release();
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

                    sync.Close();
                }
            }
        }
    }
}