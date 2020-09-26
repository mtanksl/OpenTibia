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

        NotWalkable = 16,

        NotMoveable = 32,

        BlockProjectile = 64,

        BlockPathFinding = 128,

        Pickupable = 256,

        Rotatable = 512,

        HasHeight = 1024,

        Readable = 2048
    }

    public static class ItemMetadataFlagsExtensions
    {
        public static bool Is(this ItemMetadataFlags flags, ItemMetadataFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}