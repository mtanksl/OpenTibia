using System;

namespace OpenTibia.Common.Structures
{
    [Flags]
    public enum ItemMetadataFlags : uint
    {
        IsContainer = 1,

        Stackable = 2,

        Useable = 4,

        IsFluid = 8,

        IsSplash = 16,

        NotWalkable = 32,

        NotMoveable = 64,

        BlockProjectile = 128,

        BlockPathFinding = 256,

        Pickupable = 512,

        Rotatable = 1024,

        HasHeight = 2048,

        Readable = 4096
    }

    public static class ItemMetadataFlagsExtensions
    {
        public static bool Is(this ItemMetadataFlags flags, ItemMetadataFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}