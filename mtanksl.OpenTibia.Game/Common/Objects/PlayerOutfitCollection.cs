using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerOutfitCollection : IPlayerOutfitCollection
    {
        private Dictionary<ushort, Addon> outfits = new Dictionary<ushort, Addon>();

        public bool TryGetValue(ushort outfitId, out Addon _addon)
        {
            return outfits.TryGetValue(outfitId, out _addon);
        }

        public void SetValue(ushort outfitId, Addon addon)
        {
            outfits[outfitId] = addon;
        }

        public void RemoveValue(ushort outfitId)
        {
            outfits.Remove(outfitId);
        }
                
        public IEnumerable< KeyValuePair<ushort, Addon> > GetIndexed()
        {
            foreach (var item in outfits)
            {
                yield return item;
            }
        }
    }
}