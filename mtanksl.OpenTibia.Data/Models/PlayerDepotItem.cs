namespace OpenTibia.Data.Models
{
    public class PlayerDepotItem
    {
        public int PlayerId { get; set; }

        public int SequenceId { get; set; }

        public int ParentId { get; set; }

        public int OpenTibiaId { get; set; }

        public int Count { get; set; }


        public Player Player { get; set; }
    }
}