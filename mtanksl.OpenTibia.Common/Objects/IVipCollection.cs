using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public interface IVipCollection
    {
        Vip AddVip(string name);

        void RemoveVip(uint creatureId);

        IEnumerable<Vip> GetVips();
    }
}