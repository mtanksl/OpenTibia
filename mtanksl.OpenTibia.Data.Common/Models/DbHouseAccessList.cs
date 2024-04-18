using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbHouseAccessList
    {
        public int HouseId { get; set; }

        public int ListId { get; set; }

        [Required]
        public string Text { get; set; }


        public DbHouse House { get; set; }
    }
}