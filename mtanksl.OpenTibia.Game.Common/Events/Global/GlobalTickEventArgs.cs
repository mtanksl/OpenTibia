namespace OpenTibia.Game.Events
{
    public class GlobalTickEventArgs : GameEventArgs
    {
        public static readonly int Interval = 1000;

        public static readonly int Count = 10;

        public static GlobalTickEventArgs Instance(int index)
        {
            return instances[index];
        }

        public static GlobalTickEventArgs Instance(uint id)
        {
            return instances[id % instances.Length];
        }

        private static readonly GlobalTickEventArgs[] instances;

        static GlobalTickEventArgs()
        {
            instances = new GlobalTickEventArgs[Count];

            for (int i = 0; i < instances.Length; i++)
            {
                instances[i] = new GlobalTickEventArgs(i, Interval);
            }
        }
        
        private GlobalTickEventArgs(int index, int ticks)
        {
            this.index = index;

            this.ticks = ticks;
        }

        private int index;

        public int Index
        {
            get
            {
                return index;
            }
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