namespace OpenTibia.Data.Models
{
    public class DbPlayerVip
    {
        public int PlayerId { get; set; }

        public int VipId { get; set; }

        public string Description { get; set; }

        public int IconId { get; set; }

        public bool NotifyLogin { get; set; }


        public DbPlayer Player { get; set; }

        public DbPlayer Vip { get; set; }
    }
}