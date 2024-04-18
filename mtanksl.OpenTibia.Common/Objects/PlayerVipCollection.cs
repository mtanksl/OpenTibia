using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerVipCollection
    {
        private Dictionary<int, string> vips = new Dictionary<int, string>();

        public int Count
        {
            get
            {
                return vips.Count;
            }
        }

        public bool TryGetVip(int databasePlayerId, out string name)
        {
            return vips.TryGetValue(databasePlayerId, out name);
        }

        public bool AddVip(int databasePlayerId, string name)
        {
            if ( !vips.ContainsKey(databasePlayerId) )
            {
                vips.Add(databasePlayerId, name);

                return true;
            }

            return false;
        }

        public void RemoveVip(int databasePlayerId)
        {
            vips.Remove(databasePlayerId);
        }

        public IEnumerable< KeyValuePair<int, string> > GetIndexed()
        {
            foreach (var item in vips)
            {
                yield return item;
            }
        }
    }
}