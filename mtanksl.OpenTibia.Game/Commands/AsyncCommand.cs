using System;

namespace OpenTibia.Game.Commands
{
    public abstract class AsyncCommand : Command
    {
        public event EventHandler Complete;

        protected virtual void OnComplete()
        {
            if (Complete != null)
            {
                Complete(this, EventArgs.Empty);
            }
        }
    }
}