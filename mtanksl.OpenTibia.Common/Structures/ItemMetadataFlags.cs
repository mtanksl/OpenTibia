using System;

namespace OpenTibia.Common.Structures
{
    [Flags]
    public enum ItemMetadataFlags : uint
    {
        IsContainer = 16,

        Stackable = 32,

        Useable = 64,

        NotWalkable = 512,

        NotMoveable = 1024,

        BlockProjectile = 2048,

        BlockPathFinding = 4096,

        Pickupable = 8192,

        Rotatable = 131072
    }

    public static class ItemMetadataFlagsExtensions
    {
        public static bool Is(this ItemMetadataFlags flags, ItemMetadataFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}