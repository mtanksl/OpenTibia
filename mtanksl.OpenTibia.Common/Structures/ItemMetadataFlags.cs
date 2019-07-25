using System;

namespace OpenTibia.Common.Structures
{
    [Flags]
    public enum ItemMetadataFlags : uint
    {
        IsContainer = 1,

        Stackable = 2,

        Useable = 4,
        
        NotWalkable = 8,

        NotMoveable = 16,

        BlockProjectile = 32,

        BlockPathFinding = 64,

        Pickupable = 128,

        Rotatable = 256,

        HasHeight = 512
    }

    public static class ItemMetadataFlagsExtensions
    {
        public static bool Is(this ItemMetadataFlags flags, ItemMetadataFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}