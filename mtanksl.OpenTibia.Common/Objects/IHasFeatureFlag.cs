using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public interface IHasFeatureFlag
    {
        bool HasFeatureFlag(FeatureFlag featureFlag);
    }
}