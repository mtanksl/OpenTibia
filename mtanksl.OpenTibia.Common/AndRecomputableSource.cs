using System;

namespace OpenTibia.Common.Objects
{
    public class AndRecomputableSource : IRecomputableSource
    {
        private bool aChanged = false;

        private bool bChanged = false;

        public AndRecomputableSource(IRecomputableSource a, IRecomputableSource b)
        {
            a.Changed += (sender, e) =>
            {
                aChanged = true;

                if (aChanged && bChanged)
                {
                    aChanged = false;

                    bChanged = false;

                    OnChanged();
                }
            };

            b.Changed += (sender, e) =>
            {
                bChanged = true;

                if (aChanged && bChanged)
                {
                    aChanged = false;

                    bChanged = false;

                    OnChanged();
                }
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