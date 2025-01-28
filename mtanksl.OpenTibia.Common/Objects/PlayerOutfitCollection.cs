using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerOutfitCollection
    {
        private Dictionary<ushort, Addon> outfits = new Dictionary<ushort, Addon>();

        public bool TryGetOutfit(ushort outfitId, out Addon addon)
        {
            return outfits.TryGetValue(outfitId, out addon);
        }

        public void SetOutfit(ushort outfitId, Addon addon)
        {
            outfits[outfitId] = addon;
        }

        public void RemoveOutfit(ushort outfitId)
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