using OpenTibia.Game.Events;

namespace OpenTibia.Game.EventHandlers
{
    public abstract class EventHandler<T> : EventHandler where T : GameEventArgs
    {
        public override void Handle(Context context, object e)
        {
            Handle(context, (T)e);
        }

        public abstract void Handle(Context context, T e);
    }
}