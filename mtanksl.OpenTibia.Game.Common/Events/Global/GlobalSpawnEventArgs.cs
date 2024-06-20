namespace OpenTibia.Game.Events
{
    public class GlobalSpawnEventArgs : GameEventArgs
    {
        public static readonly GlobalSpawnEventArgs Instance = new GlobalSpawnEventArgs();
    }
}