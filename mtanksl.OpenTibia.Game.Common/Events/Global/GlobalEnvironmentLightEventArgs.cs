namespace OpenTibia.Game.Events
{
    public class GlobalEnvironmentLightEventArgs : GameEventArgs
    {
        public static readonly int Interval = 10 * 1000;

        public static readonly GlobalEnvironmentLightEventArgs Instance = new GlobalEnvironmentLightEventArgs(Interval);

        private GlobalEnvironmentLightEventArgs(int ticks)
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