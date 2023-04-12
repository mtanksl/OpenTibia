using OpenTibia.Game.Commands;
using OpenTibia.Game.Events;
using System.Diagnostics;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler<T> : EventHandler where T : GameEventArgs
    {
        [DebuggerStepThrough]
        public override Promise Handle(object e)
        {
            return Handle( (T)e);
        }

        public abstract Promise Handle(T e);
    }
}