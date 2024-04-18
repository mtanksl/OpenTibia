using OpenTibia.Common.Structures;
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

    public static class SpecialConditionExtensions
    {
        public static SpecialCondition ToSpecialCondition(this ConditionSpecialCondition conditionSpecialCondition)
        {
            SpecialCondition specialCondition;

            switch (conditionSpecialCondition)
            {
                case ConditionSpecialCondition.Poisoned:

                    specialCondition = SpecialCondition.Poisoned;

                    break;

                case ConditionSpecialCondition.Burning:

                    specialCondition = SpecialCondition.Burning;

                    break;

                case ConditionSpecialCondition.Electrified:

                    specialCondition = SpecialCondition.Electrified;

                    break;

                case ConditionSpecialCondition.Drunk:

                    specialCondition = SpecialCondition.Drunk;

                    break;

                case ConditionSpecialCondition.MagicShield:

                    specialCondition = SpecialCondition.MagicShield;

                    break;

                case ConditionSpecialCondition.Slowed:

                    specialCondition = SpecialCondition.Slowed;

                    break;

                case ConditionSpecialCondition.Haste:

                    specialCondition = SpecialCondition.Haste;

                    break;

                case ConditionSpecialCondition.LogoutBlock:

                    specialCondition = SpecialCondition.LogoutBlock;

                    break;

                case ConditionSpecialCondition.Drowning:

                    specialCondition = SpecialCondition.Drowning;

                    break;

                case ConditionSpecialCondition.Freezing:

                    specialCondition = SpecialCondition.Freezing;

                    break;

                case ConditionSpecialCondition.Dazzled:

                    specialCondition = SpecialCondition.Dazzled;

                    break;

                case ConditionSpecialCondition.Cursed:

                    specialCondition = SpecialCondition.Cursed;

                    break;

                case ConditionSpecialCondition.Bleeding:

                    specialCondition = SpecialCondition.Bleeding;

                    break;

                default:

                    specialCondition = SpecialCondition.None;

                    break;
            }

            return specialCondition;
        }
    }
}