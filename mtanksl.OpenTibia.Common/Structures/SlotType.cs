using System;

namespace OpenTibia.Common.Structures
{
    [Flags]
    public enum SlotType : ushort
    {
       None = 0,

       Head = 1,

       Necklace = 2,

       Backpack = 4,

       Body = 8,

       Right = 16,

       Left = 32,

       Hand = Left | Right,

       Legs = 64,

       Feet = 128,

       Ring = 256,

       Ammo = 512,

       TwoHanded = 1024,

       Any = Head | Necklace | Backpack | Body | Right | Left | Legs | Feet | Ring | Ammo
    }

    public static class SlotTypeExtensions
    {
        public static bool Is(this SlotType flags, SlotType flag)
        {
            return (flags & flag) == flag;
        }
    }
}