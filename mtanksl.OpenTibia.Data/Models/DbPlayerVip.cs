namespace OpenTibia.Data.Models
{
    public class DbPlayerVip
    {
        public int PlayerId { get; set; }

        public int SequenceId { get; set; }

        public int VipId { get; set; }


        public DbPlayer Player { get; set; }

        public DbPlayer Vip { get; set; }
    }
}