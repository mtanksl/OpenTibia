using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerVipCollection
    {
        public class Vip
        {
            public string Name { get; set; }

            public string Description { get; set; }

            public uint IconId { get; set; }

            public bool NotifyLogin { get; set; }
        }

        private Dictionary<int, Vip> vips = new Dictionary<int, Vip>();

        public int Count
        {
            get
            {
                return vips.Count;
            }
        }

        public bool TryGetVip(int databasePlayerId, out Vip vip)
        {
            return vips.TryGetValue(databasePlayerId, out vip);
        }

        public bool AddVip(int databasePlayerId, Vip vip)
        {
            if ( !vips.ContainsKey(databasePlayerId) )
            {
                vips.Add(databasePlayerId, vip);

                return true;
            }

            return false;
        }

        public void RemoveVip(int databasePlayerId)
        {
            vips.Remove(databasePlayerId);
        }

        public IEnumerable< KeyValuePair<int, Vip> > GetIndexed()
        {
            foreach (var vip in vips)
            {
                yield return vip;
            }
        }
    }
}