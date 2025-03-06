namespace OpenTibia.Game.Events
{
    public class GlobalDecayEventArgs : GameEventArgs
    {
        public static readonly int Interval = 1000;

        public static readonly int Count = 4;

        public static GlobalDecayEventArgs Instance(int index)
        {
            return instances[index];
        }

        public static GlobalDecayEventArgs Instance(uint id)
        {
            return instances[id % instances.Length];
        }

        private static readonly GlobalDecayEventArgs[] instances;

        static GlobalDecayEventArgs()
        {
            instances = new GlobalDecayEventArgs[Count];

            for (int i = 0; i < instances.Length; i++)
            {
                instances[i] = new GlobalDecayEventArgs(i, Interval);
            }
        }
        
        private GlobalDecayEventArgs(int index, int ticks)
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