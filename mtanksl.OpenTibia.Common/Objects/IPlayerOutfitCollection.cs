using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IPlayerOutfitCollection
    {
        bool TryGetValue(ushort outfitId, out Addon _addon);

        void SetValue(ushort outfitId, Addon addon);

        void RemoveValue(ushort outfitId);

        IEnumerable< KeyValuePair<ushort, Addon> > GetIndexed();
    }
}