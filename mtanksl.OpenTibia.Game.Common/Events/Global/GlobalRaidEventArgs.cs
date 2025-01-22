namespace OpenTibia.Game.Events
{
    public class GlobalRaidEventArgs : GameEventArgs
    {
        public static readonly int Interval = 60 * 1000;

        public static readonly GlobalRaidEventArgs Instance = new GlobalRaidEventArgs(Interval);

        private GlobalRaidEventArgs(int ticks)
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