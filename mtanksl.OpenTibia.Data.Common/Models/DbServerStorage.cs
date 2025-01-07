using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbServerStorage
    {
        [Required]
        [StringLength(255)]
        public string Key { get; set; }

        [Required]
        [StringLength(255)]
        public string Value { get; set; }
    }
}