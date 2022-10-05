using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class World
    {
        public int Id { get; set; }

        [Required]
        [StringLength(255)] 
        public string Name { get; set; }

        [Required]
        [StringLength(255)] 
        public string Ip { get; set; }

        public int Port { get; set; }
    }
}