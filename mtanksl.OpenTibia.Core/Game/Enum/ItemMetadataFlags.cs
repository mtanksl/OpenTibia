using System;

namespace OpenTibia
{
    [Flags]
    public enum ItemMetadataFlags
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

        Hangable = 1024,

        Horizontal = 2048,

        Vertical = 4096,

        Rotatable = 8192,

        HasHeight = 16384
    }
}