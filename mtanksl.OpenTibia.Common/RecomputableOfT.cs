using System;

namespace OpenTibia.Common.Objects
{
    public class Recomputable<T>
    {
        private Func<T> callback;

        public Recomputable(IRecomputableSource source, Func<T> callback)
        {
            source.Changed += (sender, e) =>
            {
                recompute = true;
            };

            this.callback = callback;
        }

        private bool recompute = true;

        public bool IsValueCreated
        {
            get
            {
                return !recompute;
            }
        }

        public void EnsureUpdated()
        {
            if (recompute)
            {
                value = callback();

                recompute = false;
            }
        }

        private T value;

        public T Value
        {
            get
            {
                EnsureUpdated();

                return value;
            }
        }
    }
}