using System;
using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbPlayerDeath
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public int? AttackerId { get; set; }

        [StringLength(255)]
        public string Name { get; set; }

        public int Level { get; set; }

        public bool Unjustified { get; set; }

        public DateTime CreationDate { get; set; }


        public DbPlayer Player { get; set; }

        public DbPlayer Attacker { get; set; }
    }
}