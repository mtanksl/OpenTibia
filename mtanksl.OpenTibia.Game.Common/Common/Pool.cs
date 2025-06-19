using System;
using System.Collections.Generic;
using System.Threading;

namespace OpenTibia.Game.Common
{
    //Reference: https://stackoverflow.com/a/2572919

    public enum LoadingMode 
    {
        EagerBounded,

        LazyBounded,

        LazyUnbounded
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

        private IItemStore itemStore;

        public Pool(Func<Pool<T>, T> factory) : this(-1, factory, LoadingMode.LazyUnbounded, AccessMode.FIFO)
        {

        }

        public Pool(int maxCount, Func<Pool<T>, T> factory, LoadingMode loadingMode, AccessMode accessMode)
        {
            if (factory == null)
            {
                throw new ArgumentNullException(nameof(factory) );
            }

            if (loadingMode == LoadingMode.EagerBounded || loadingMode == LoadingMode.LazyBounded)
            {
                if (maxCount <= 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(maxCount), maxCount, "Argument must be greater than zero.");
                }

                this.sync = new Semaphore(maxCount, maxCount);
            }

            this.factory = factory;

            this.loadingMode = loadingMode;

            switch (accessMode)
            {
                case AccessMode.FIFO:
                default:

                    this.itemStore = new QueueStore();

                    break;

                case AccessMode.LIFO:

                    this.itemStore = new StackStore();

                    break;
            }

            if (loadingMode == LoadingMode.EagerBounded)
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
            switch (loadingMode)
            {
                case LoadingMode.EagerBounded:

                    sync.WaitOne();

                    lock (itemStore)
                    {
                        return itemStore.Fetch();
                    }

                case LoadingMode.LazyBounded:

                    sync.WaitOne();

                    lock (itemStore)
                    {
                        if (itemStore.Count > 0)
                        {
                            return itemStore.Fetch();
                        }
                    }

                    return factory(this);

                case LoadingMode.LazyUnbounded:
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
            switch (loadingMode)
            {
                case LoadingMode.EagerBounded:
                case LoadingMode.LazyBounded:

                    lock (itemStore)
                    {
                        itemStore.Store(item);
                    }

                    sync.Release();

                    break;

                case LoadingMode.LazyUnbounded:
                default:

                    lock (itemStore)
                    {
                        itemStore.Store(item);
                    }

                    break;
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

                    switch (loadingMode)
                    {
                        case LoadingMode.EagerBounded:
                        case LoadingMode.LazyBounded:

                            sync.Close();

                            break;
                    }
                }
            }
        }
    }
}