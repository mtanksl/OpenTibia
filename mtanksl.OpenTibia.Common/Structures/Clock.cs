namespace OpenTibia.Common.Structures
{
    public class Clock
    {
        public static readonly int Interval = 2500;

        public Clock(int hour, int minute)
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

        public Light Light
        {
            get
            {
                double minutes = hour * 60 + minute;

                if (minutes <= 720)
                {
                    // (m - 0) / (720 - 0) = (x - 40) / (250 - 40)

                    // m / 720 = (x - 40) / 210

                    // m / 720 * 210 + 40 = x

                    return new Light( (byte)( minutes * 210 / 720 + 40 ), 215 );
                }

                // (m - 720) / (1440 - 720) = (x - 250) / (40 - 250)

                // (m - 720) / 720 = (x - 250) / -210

                // (m - 720) / 720 * -210 + 250 = x

                return new Light( (byte)( (minutes - 720) * -210 / 720 + 250 ), 215 );
            }
        }

        public void Tick()
        {
            minute += Interval / 2500; // 1 hour = 24 game hours

                                       // 2.5 seconds = 1 game minute

            if (minute == 60)
            {
                minute = 0;

                hour += 1;

                if (hour == 24)
                {
                    hour = 0;
                }
            }
        }
    }
}