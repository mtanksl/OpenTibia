using OpenTibia.Game.Events;
using System.Diagnostics;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler<T> : EventHandler where T : GameEventArgs
    {
        [DebuggerStepThrough]
        public override void Handle(object e)
        {
            Handle( (T)e);
        }

        public abstract void Handle(T e);
    }
}