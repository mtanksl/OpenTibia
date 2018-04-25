using System.Collections.Generic;

namespace OpenTibia.Data
{
    public class Account
    {
        public string Name { get; set; }

        public string Password { get; set; }

        public int PremiumDays { get; set; }

        public virtual ICollection<Player> Players { get; set; }
    }
}