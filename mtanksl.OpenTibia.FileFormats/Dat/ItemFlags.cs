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

        IsFluid = 128,

        IsSplash = 256,

        NotWalkable = 512,

        NotMoveable = 1024,

        BlockProjectile = 2048, 

        BlockPathFinding = 4096,

        Pickupable = 8192,

        Hangable = 16384,

        Horizontal = 32768,

        Vertical = 65536,

        Rotatable = 131072,

        IdleAnimation  = 262144,

        SolidGround = 524288,

        LookThrough = 1048576
    }

    public static class ItemFlagsExtensions
    {
        public static bool Is(this ItemFlags flags, ItemFlags flag)
        {
            return (flags & flag) == flag;
        }
    }
}