namespace OpenTibia.Common.Structures
{
    public enum TopOrder : byte
    {
        Ground = 0,

        /// <summary>
        /// Carpet.
        /// </summary>
        HighPriority = 1,

        /// <summary>
        /// Decoration, not moveable.
        /// </summary>
        MediumPriority = 2,

        /// <summary>
        /// Arch.
        /// </summary>
        LowPriority = 3,

        Creature = 4,

        Other = 5
    }
}