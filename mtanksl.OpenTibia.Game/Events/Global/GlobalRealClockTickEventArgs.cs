namespace OpenTibia.Game.Events
{
    public class GlobalRealClockTickEventArgs : GameEventArgs
    {
        public GlobalRealClockTickEventArgs(int hour, int minute)
        {
            this.hour = hour;

            this.minute = minute;
        }

        private int hour;

        public int Hour
        {
            get
            {
                return hour;
            }
        }

        private int minute;

        public int Minute
        {
            get
            {
                return minute;
            }
        }
    }
}