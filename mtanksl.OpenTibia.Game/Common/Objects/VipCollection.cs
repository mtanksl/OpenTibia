using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class VipCollection : IVipCollection
    {
        public VipCollection(IClient client)
        {
            this.client = client;
        }

        private IClient client;

        private IClient Client
        {
            get
            {
                return client;
            }
        }

        private uint uniqueId = 0;

        private uint GenerateId()
        {
            uniqueId++;

            return uniqueId;
        }

        private List<Vip> vips = new List<Vip>();

        public Vip AddVip(string name)
        {
            Vip vip = new Vip()
            {
                Id = GenerateId(),

                Name = name
            };

            vips.Add(vip);

            return vip;
        }

        public void RemoveVip(uint id)
        {
            for (int i = 0; i < vips.Count; i++)
            {
                if (vips[i].Id == id)
                {
                    vips.RemoveAt(i);

                    break;
                }
            }
        }

        public IEnumerable<Vip> GetVips()
        {
            return vips;
        }
    }
}