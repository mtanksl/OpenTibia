using System;

namespace OpenTibia
{
    [Flags]
    public enum Addons : byte
    {
        None = 0,

        First = 1,

        Second = 2,

        Both = First | Second
    }
}