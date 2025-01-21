namespace OpenTibia.Common.Structures
{
    public enum DamageType : byte
    {
        None = 0,

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
        public static AnimatedTextColor? ToAnimatedTextColor(this DamageType damageType)
        {
            switch (damageType)
            {
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

            return null;
        }
    }
}