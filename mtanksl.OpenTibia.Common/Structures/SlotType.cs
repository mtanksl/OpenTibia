using System;

namespace OpenTibia.Common.Structures
{
    [Flags]
    public enum SlotType : ushort
    {
       None = 0,

       Head = 1,

       Amulet = 2,

       Container = 4,

       Armor = 8,

       Right = 16,

       Left = 32,

       Hand = Left | Right,

       Legs = 64,

       Feet = 128,

       Ring = 256,

       Extra = 512,

       TwoHand = 1024,

       Any = Head | Amulet | Container | Armor | Right | Left | Legs | Feet | Ring | Extra
    }

    public static class SlotTypeExtensions
    {
        public static bool Is(this SlotType flags, SlotType flag)
        {
            return (flags & flag) == flag;
        }
    }
}