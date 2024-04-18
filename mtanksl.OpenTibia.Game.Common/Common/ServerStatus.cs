namespace OpenTibia.Game.Common
{
    public enum ServerStatus
    {
        /// <summary>
        /// It is not running.
        /// </summary>
        Stopped,

        /// <summary>
        /// It is running.
        /// </summary>
        Running,

        /// <summary>
        /// It is running, but not accepting new players.
        /// </summary>
        Paused
    }
}