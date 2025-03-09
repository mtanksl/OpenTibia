using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IHasFeatureFlag
    {
        bool HasFeatureFlag(FeatureFlag featureFlag);

        byte GetByteForFluidType(FluidType fluidType);

        byte GetByteForMagicEffectType(MagicEffectType magicEffectType);

        byte GetByteForProjectileType(ProjectileType projectileType);

        byte GetByteForTextColor(TextColor textColor);
    }
}