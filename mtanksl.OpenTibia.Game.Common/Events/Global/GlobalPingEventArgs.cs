namespace OpenTibia.Game.Events
{
    public class GlobalPingEventArgs : GameEventArgs
    {
        public static readonly GlobalPingEventArgs Instance = new GlobalPingEventArgs();

        private GlobalPingEventArgs()
        {
            
        }
    }
}