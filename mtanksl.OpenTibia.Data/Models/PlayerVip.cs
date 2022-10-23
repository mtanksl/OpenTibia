namespace OpenTibia.Data.Models
{
    public class PlayerVip
    {
        public int PlayerId { get; set; }

        public int SequenceId { get; set; }

        public int VipId { get; set; }


        public Player Player { get; set; }

        public Player Vip { get; set; }
    }
}