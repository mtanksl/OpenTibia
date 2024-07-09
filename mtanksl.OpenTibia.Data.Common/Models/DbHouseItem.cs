namespace OpenTibia.Data.Models
{
    public class DbHouseItem
    {
        public int HouseId { get; set; }

        public long SequenceId { get; set; }

        public long ParentId { get; set; }

        public int OpenTibiaId { get; set; }

        public int Count { get; set; }


        public DbHouse House { get; set; }
    }
}