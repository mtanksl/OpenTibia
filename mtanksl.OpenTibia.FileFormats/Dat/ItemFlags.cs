using System;

namespace OpenTibia.FileFormats.Dat
{
    [Flags]
    public enum ItemFlags : uint
    {
        IsGround = 1,

        AlwaysOnTop1 = 2,

        AlwaysOnTop2 = 4,

        AlwaysOnTop3  = 8,

        IsContainer = 16,

        Stackable = 32,

        Useable = 64,

        Writeable = 128,

        Readable = 256,

        IsFluid = 512,

        IsSplash = 1024,

        NotWalkable = 2048,

        NotMoveable = 4096,

        BlockProjectile = 8192, 

        BlockPathFinding = 16384,

        Pickupable = 32768,

        Hangable = 65536,

        Horizontal = 131072,

        Vertical = 262144,

        Rotatable = 524288,

        IdleAnimation  = 1048576,

        SolidGround = 2097152,

        LookThrough = 4194304
    }

    public static class ItemFlagsExtensions
    {
        public static bool Is(this ItemFlags flags, ItemFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}