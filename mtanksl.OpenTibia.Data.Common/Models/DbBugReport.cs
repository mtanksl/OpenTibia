using System;
using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbBugReport
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        public int PositionX { get; set; }

        public int PositionY { get; set; }

        public int PositionZ { get; set; }

        [Required]
        [StringLength(255)]
        public string Message { get; set; }

        public DateTime CreationDate { get; set; }


        public DbPlayer Player { get; set; }
    }
}