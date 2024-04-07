using System.Collections.Generic;

namespace OpenTibia.Data.Models
{
    public class DbHouse
    {
        public int Id { get; set; }

        public int? OwnerId { get; set; }


        public DbPlayer Owner { get; set; }

        public ICollection<DbHouseAccessList> HouseAccessLists { get; set; } = new List<DbHouseAccessList>();
    }
}