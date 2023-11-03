using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerOutfitCollection : IPlayerOutfitCollection
    {
        private Dictionary<int, int> outfits = new Dictionary<int, int>();

        public bool TryGetValue(int key, out int value)
        {
            return outfits.TryGetValue(key, out value);
        }

        public void SetValue(int key, int value)
        {
            outfits[key] = value;
        }

        public void RemoveValue(int key)
        {
            outfits.Remove(key);
        }
                
        public IEnumerable< KeyValuePair<int, int> > GetIndexed()
        {
            foreach (var item in outfits)
            {
                yield return item;
            }
        }
    }
}