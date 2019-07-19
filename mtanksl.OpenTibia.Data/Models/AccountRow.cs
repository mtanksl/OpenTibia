using System.Collections.Generic;

namespace OpenTibia.Data
{
    public class AccountRow
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public int PremiumDays { get; set; }

        public virtual ICollection<PlayerRow> Players { get; set; }
    }
}