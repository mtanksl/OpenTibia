namespace OpenTibia.Data.Models
{
    public class DbPlayerStorage
    {
        public int PlayerId { get; set; }

        public int Key { get; set; }

        public int Value { get; set; }


        public DbPlayer Player { get; set; }
    }
}