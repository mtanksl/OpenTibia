using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IHasFeatureFlag
    {
        bool HasFeatureFlag(FeatureFlag featureFlag);

        byte GetByteForMagicEffectType(MagicEffectType magicEffectType);

        byte GetByteForProjectileType(ProjectileType projectileType);

        byte GetByteForFluidType(FluidType fluidType);

        byte GetByteForTextColor(TextColor textColor);

        TalkType GetTalkTypeForByte(byte value);

        byte GetByteForTalkType(TalkType talkType);
    }
}