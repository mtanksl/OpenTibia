using System;

namespace OpenTibia.Data.Models
{
    public class DbPlayerKill
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public int TargetId { get; set; }

        public bool Unjustified { get; set; }

        public DateTime CreationDate { get; set; }


        public DbPlayer Player { get; set; }

        public DbPlayer Target { get; set; }
    }
}