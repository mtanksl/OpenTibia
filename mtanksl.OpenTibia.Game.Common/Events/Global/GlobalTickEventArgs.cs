namespace OpenTibia.Game.Events
{
    public class GlobalTickEventArgs : GameEventArgs
    {
        public static GlobalTickEventArgs[] Instance = new GlobalTickEventArgs[10]
        {
            new GlobalTickEventArgs() { Index = 0 },
            new GlobalTickEventArgs() { Index = 1 },
            new GlobalTickEventArgs() { Index = 2 },
            new GlobalTickEventArgs() { Index = 3 },
            new GlobalTickEventArgs() { Index = 4 },
            new GlobalTickEventArgs() { Index = 5 },
            new GlobalTickEventArgs() { Index = 6 },
            new GlobalTickEventArgs() { Index = 7 },
            new GlobalTickEventArgs() { Index = 8 },
            new GlobalTickEventArgs() { Index = 9 }
        };

        public int Index { get; set; }
    }
}