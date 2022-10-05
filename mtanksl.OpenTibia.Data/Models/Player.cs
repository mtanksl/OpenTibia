using System.ComponentModel.DataAnnotations;

namespace OpenTibia.Data.Models
{
    public class Player
    {
        public int Id { get; set; }

        public int AccountId { get; set; }

        public int WorldId { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }
              
        public int CoordinateX { get; set; }

        public int CoordinateY { get; set; }

        public int CoordinateZ { get; set; }


        public Account Account { get; set; }

        public World World { get; set; }
    }
}