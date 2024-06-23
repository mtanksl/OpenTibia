namespace OpenTibia.Data.Models
{
    public class DbPlayerBless
    {
        public int PlayerId { get; set; }

        public string Name { get; set; }


        public DbPlayer Player { get; set; }
    }
}