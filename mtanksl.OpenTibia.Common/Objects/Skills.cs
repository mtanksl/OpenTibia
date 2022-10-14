namespace OpenTibia.Common.Objects
{
    public class Skills
    {
        public Skills(Player player)
        {
            this.player = player;
        }

        private Player player;

        public Player Player
        {
            get
            {
                return player;
            }
        }

        public byte MagicLevel { get; set; }

        public byte MagicLevelPercent { get; set; }

        public byte Fist { get; set; }

        public byte FistPercent { get; set; }

        public byte Club { get; set; }

        public byte ClubPercent { get; set; }

        public byte Sword { get; set; }

        public byte SwordPercent { get; set; }

        public byte Axe { get; set; }

        public byte AxePercent { get; set; }

        public byte Distance { get; set; }

        public byte DistancePercent { get; set; }

        public byte Shield { get; set; }

        public byte ShieldPercent { get; set; }

        public byte Fish { get; set; }

        public byte FishPercent { get; set; }
    }
}