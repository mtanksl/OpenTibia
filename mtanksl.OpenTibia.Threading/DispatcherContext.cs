using System;
using System.Collections.Generic;

namespace OpenTibia.Threading
{
    public class DispatcherContext : IDisposable
    {
        public static DispatcherContext Current
        {
            get
            {
                Scope<DispatcherContext> scope = Scope<DispatcherContext>.Current;

                if (scope != null)
                {
                    return scope.Value;
                }

                return null;
            }
        }

        private Scope<DispatcherContext> scope;
        
        public DispatcherContext()
        {
            scope = new Scope<DispatcherContext>(this);
        }

        private Dictionary<string, object> items = new Dictionary<string, object>();

        public T GetItem<T>(string key, Func<T> callback)
        {
            object value;

            if ( !items.TryGetValue(key, out value) )
            {
                value = callback();

                items.Add(key, value);
            }

            return (T)value;
        }

        //private MessageCollection messages = new MessageCollection();

        //public MessageCollection Messages
        //{
        //    get
        //    {
        //        return messages;
        //    }
        //}

        public void Dispose()
        {
            scope.Dispose();

            //messages.Flush();
        }
    }
}