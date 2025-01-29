using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbPlayerBless
    {
        public int PlayerId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }


        public DbPlayer Player { get; set; }
    }
}