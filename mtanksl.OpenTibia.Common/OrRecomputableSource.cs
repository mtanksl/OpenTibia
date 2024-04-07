using System;

namespace OpenTibia.Common.Objects
{
    public class OrRecomputableSource : IRecomputableSource
    {
        public OrRecomputableSource(IRecomputableSource a, IRecomputableSource b)
        {
            a.Changed += (sender, e) =>
            {
                OnChanged();
            };

            b.Changed += (sender, e) =>
            {
                OnChanged();
            };
        }

        public event EventHandler Changed;

        protected virtual void OnChanged()
        {
            if (Changed != null)
            {
                Changed(this, EventArgs.Empty);
            }
        }
    }
}