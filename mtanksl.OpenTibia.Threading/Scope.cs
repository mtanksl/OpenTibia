using System;

namespace OpenTibia.Threading
{
    public sealed class Scope<T> : IDisposable
    {
        [ThreadStatic]
        private static Scope<T> current;

        public static Scope<T> Current
        {
            get
            {
                return current;
            }
        }

        private Scope<T> _parent;

        /// <exception cref="ArgumentNullException"></exception>

        public Scope(T value)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value) );
            }

            _value = value;

            _parent = current; 
            
                      current = this;
        }

        private T _value;

        public T Value
        {
            get
            {
                return _value;
            }
        }

        public void Dispose()
        {
            current = _parent;
        }
    }
}