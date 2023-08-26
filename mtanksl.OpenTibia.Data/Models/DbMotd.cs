using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class DbMotd
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string Message { get; set; }
    }
}