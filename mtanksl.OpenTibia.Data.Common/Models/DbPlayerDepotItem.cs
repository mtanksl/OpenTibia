namespace OpenTibia.Data.Models
{
    public class DbPlayerDepotItem
    {
        public int PlayerId { get; set; }

        public int SequenceId { get; set; }

        public int ParentId { get; set; }

        public int OpenTibiaId { get; set; }

        public int Count { get; set; }

        public byte[] Attributes { get; set; }


        public DbPlayer Player { get; set; }
    }
}