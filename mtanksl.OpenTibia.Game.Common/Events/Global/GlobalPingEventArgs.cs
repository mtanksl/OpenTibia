namespace OpenTibia.Game.Events
{
    public class GlobalPingEventArgs : GameEventArgs
    {
        public static readonly int Interval = 10 * 1000;

        public static readonly GlobalPingEventArgs Instance = new GlobalPingEventArgs(Interval);

        private GlobalPingEventArgs(int ticks)
        {
            this.ticks = ticks;
        }

        private int ticks;

        public int Ticks
        {
            get
            {
                return ticks;
            }
        }
    }
}