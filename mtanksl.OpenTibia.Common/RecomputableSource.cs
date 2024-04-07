using System;

namespace OpenTibia.Common.Objects
{
    public class RecomputableSource : IRecomputableSource
    {
        public void Change()
        {
            OnChanged();
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