namespace OpenTibia.Game.Events
{

    public class GlobalServerReloadedEventArgs : GameEventArgs
    {
        public static readonly GlobalServerReloadedEventArgs Instance = new GlobalServerReloadedEventArgs();
    }
}