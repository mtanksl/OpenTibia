namespace OpenTibia.Game.Events
{
    public class ObserveEventArgs
    {
        public static ObserveEventArgs<T> Create<T>(T originalEvent) where T : GameEventArgs
        {
            return new ObserveEventArgs<T>(originalEvent);
        }
    }

    public class ObserveEventArgs<T> : GameEventArgs where T : GameEventArgs
    {
        public ObserveEventArgs(T originalEvent)
        {
            OriginalEvent = originalEvent;
        }

        public T OriginalEvent { get; }
    }
}