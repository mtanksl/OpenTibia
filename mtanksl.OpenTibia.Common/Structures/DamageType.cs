using System;

namespace OpenTibia.Common.Structures
{
    public enum DamageType : byte
    {
        Healing = 0,

        Physical = 1,

        Earth = 2,

        Fire = 3,

        Energy = 4,

        Ice = 5,

        Death = 6,

        Holy = 7,

        Drown = 8,

        ManaDrain = 9,

        LifeDrain = 10
    }

    public static class DamageTypeExtensions
    {
        public static MagicEffectType? ToMagicEffectType(this DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Healing:

                    return MagicEffectType.BlueShimmer;

                case DamageType.Physical:

                    return MagicEffectType.RedSpark;

                case DamageType.Earth:

                    return MagicEffectType.GreenRings;

                case DamageType.Fire:

                    return MagicEffectType.FireDamage;

                case DamageType.Energy:

                    return MagicEffectType.EnergyDamage;

                case DamageType.Ice:

                    return MagicEffectType.IceDamage;

                case DamageType.Death:

                    return MagicEffectType.SmallClouds;

                case DamageType.Holy:

                    return MagicEffectType.HolyDamage;

                case DamageType.Drown:

                    return MagicEffectType.BlueRings;

                case DamageType.ManaDrain:

                    return MagicEffectType.BlueRings;

                case DamageType.LifeDrain:

                    return MagicEffectType.RedShimmer;
            }

            throw new NotImplementedException();
        }

        public static AnimatedTextColor? ToAnimatedTextColor(this DamageType damageType)
        {
            switch (damageType)
            {
                case DamageType.Healing:

                    return null;

                case DamageType.Physical:

                    return AnimatedTextColor.Red;

                case DamageType.Earth:

                    return AnimatedTextColor.Green;

                case DamageType.Fire:

                    return AnimatedTextColor.Orange;

                case DamageType.Energy:

                    return AnimatedTextColor.Purple;

                case DamageType.Ice:

                    return AnimatedTextColor.SeaBlue;

                case DamageType.Death:

                    return AnimatedTextColor.DarkRed;

                case DamageType.Holy:

                    return AnimatedTextColor.Yellow;

                case DamageType.Drown:

                    return AnimatedTextColor.Cyan;

                case DamageType.ManaDrain:

                    return AnimatedTextColor.Blue;

                case DamageType.LifeDrain:

                    return AnimatedTextColor.Red;
            }

            throw new NotImplementedException();
        }
    }
}