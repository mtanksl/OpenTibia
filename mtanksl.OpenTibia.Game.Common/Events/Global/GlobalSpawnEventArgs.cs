namespace OpenTibia.Game.Events
{
    public class GlobalSpawnEventArgs : GameEventArgs
    {
        public static readonly int Interval = 10 * 1000;

        public static readonly GlobalSpawnEventArgs Instance = new GlobalSpawnEventArgs(Interval);

        private GlobalSpawnEventArgs(int ticks)
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