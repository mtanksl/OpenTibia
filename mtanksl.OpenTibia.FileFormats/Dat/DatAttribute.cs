namespace OpenTibia.FileFormats.Dat
{
    public enum DatAttribute : byte
    {
        IsGround = 0,

        AlwaysOnTop1 = 1,

        AlwaysOnTop2 = 2,

        AlwaysOnTop3 = 3,

        IsContainer = 4,

        Stackable = 5,

        IsCorpse = 6,

        Useable = 7,

        Writeable = 8,

        Readable = 9,

        IsFluid = 10,

        IsSplash = 11,

        NotWalkable = 12,

        NotMoveable = 13,

        BlockProjectile = 14,

        BlockPathFinding = 15,

        Pickupable = 16,

        Hangable = 17,
        
        Horizontal = 18,

        Vertical = 19,

        Rotatable = 20,

        Light = 21,

        Offset = 24,

        HasHeight = 25,

        IdleAnimation = 27,

        MinimapColor = 28,

        ExtraInfo = 29,

        SolidGround = 30,

        LookThrough = 31,

        Cloth = 32,

        Market = 33,

        Usable = 34,

        Wrappable = 35,

        Unwrappable = 36,

        TopEffect = 37,

        NoMoveAnimation = 253, // On 10.10 the original value is 16

        End = 255
    }
}