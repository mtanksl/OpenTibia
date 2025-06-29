using System;

namespace OpenTibia.Common.Structures
{
    [Flags]
    public enum ItemMetadataFlags : uint
    {
        None = 0,

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

        Hangable = 1024,

        Horizontal = 2048,

        Vertical = 4096,

        Rotatable = 8192,

        HasHeight = 16384,

        Readable = 32768,

        Writeable = 65536,

        AllowDistanceRead = 131072,

        IsAnimated = 262144,

        Wrappable = 524288,

        Unwrappable = 1048576
    }

    public static class ItemMetadataFlagsExtensions
    {
        public static bool Is(this ItemMetadataFlags flags, ItemMetadataFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}