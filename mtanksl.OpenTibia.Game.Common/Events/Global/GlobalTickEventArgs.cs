namespace OpenTibia.Game.Events
{
    public class GlobalTickEventArgs : GameEventArgs
    {
        public static GlobalTickEventArgs Instance(uint id)
        {
            return Instances[id % Instances.Length];
        }

        public static readonly GlobalTickEventArgs[] Instances = new GlobalTickEventArgs[10]
        {
            new GlobalTickEventArgs(0, 1000),
            new GlobalTickEventArgs(1, 1000),
            new GlobalTickEventArgs(2, 1000),
            new GlobalTickEventArgs(3, 1000),
            new GlobalTickEventArgs(4, 1000),
            new GlobalTickEventArgs(5, 1000),
            new GlobalTickEventArgs(6, 1000),
            new GlobalTickEventArgs(7, 1000),
            new GlobalTickEventArgs(8, 1000),
            new GlobalTickEventArgs(9, 1000)
        };

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