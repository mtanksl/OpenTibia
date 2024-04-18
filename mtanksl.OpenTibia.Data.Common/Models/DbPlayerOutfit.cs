namespace OpenTibia.Data.Models
{
    public class DbPlayerOutfit
    {
        public int PlayerId { get; set; }

        public int OutfitId { get; set; }

        public int OutfitAddon { get; set; }


        public DbPlayer Player { get; set; }
    }
}