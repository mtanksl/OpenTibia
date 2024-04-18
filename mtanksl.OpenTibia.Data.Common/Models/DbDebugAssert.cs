using System;
using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbDebugAssert
    {
        public int Id { get; set; }

        public int PlayerId { get; set; }

        [Required]
        [StringLength(255)]
        public string AssertLine { get; set; }

        [Required]
        [StringLength(255)]
        public string ReportDate { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [StringLength(255)]
        public string Comment { get; set; }

        public DateTime CreationDate { get; set; }


        public DbPlayer Player { get; set; }
    }
}