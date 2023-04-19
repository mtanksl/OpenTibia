using System;

namespace OpenTibia.Game.Commands
{
    [Flags]
    public enum ConditionSpecialCondition
    {
        None = 0,

        Poisoned = 1,

        Burning = 2,

        Electrified = 4,

        Drunk = 8,

        MagicShield = 16,

        Slowed = 32,

        Haste = 64,

        LogoutBlock = 128,

        Drowning = 256,

        Freezing = 512,

        Dazzled = 1024,

        Cursed = 2048,

        Bleeding = 4096,

        Outfit = 32768,

        Invisible = 65536,

        Light = 131072,

        Regeneration = 262144,

        Soul = 524288,

        Muted = 1048576
    }
}