using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbAccount
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public DateTime? PremiumUntil { get; set; }


        public ICollection<DbPlayer> Players { get; set; } = new List<DbPlayer>();
    }
}