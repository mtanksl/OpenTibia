using OpenTibia.Common.Objects;

namespace OpenTibia.Game.Events
{
    public class ObserveEventArgs
    {
        public static ObserveEventArgs<T> Create<T>(GameObject observer, T sourceEvent) where T : GameEventArgs
        {
            return new ObserveEventArgs<T>(observer, sourceEvent);
        }
    }

    public class ObserveEventArgs<T> : GameEventArgs where T : GameEventArgs
    {
        public ObserveEventArgs(GameObject observer, T sourceEvent)
        {
            Observer = observer;

            SourceEvent = sourceEvent;
        }

        public GameObject Observer { get; }

        public T SourceEvent { get; }
    }
}