using OpenTibia.Game.Events;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler<T> : EventHandler where T : GameEventArgs
    {
        public override void Handle(object e)
        {
            Handle( (T)e);
        }

        public abstract void Handle(T e);
    }
}