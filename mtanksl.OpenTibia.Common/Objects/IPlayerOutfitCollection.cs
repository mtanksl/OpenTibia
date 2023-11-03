using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IPlayerOutfitCollection
    {
        bool TryGetOutfit(ushort outfitId, out Addon _addon);

        void SetOutfit(ushort outfitId, Addon addon);

        void RemoveOutfit(ushort outfitId);

        IEnumerable< KeyValuePair<ushort, Addon> > GetIndexed();
    }
}