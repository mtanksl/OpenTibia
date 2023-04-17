using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class Account
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public int PremiumDays { get; set; }


        public ICollection<Player> Players { get; set; } = new List<Player>();
    }
}