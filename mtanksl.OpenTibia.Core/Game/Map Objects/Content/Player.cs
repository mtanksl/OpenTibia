namespace OpenTibia
{
    public class Player : Creature
    {
        public Player() : base()
        {
            Experience = 0;

            Level = 100;

            LevelPercent = 0;

            Mana = 50;

            MaxMana = 50;

            Soul = 100;

            Capacity = 10000;

            Stamina = 42 * 60;

            CanReportBugs = true;

            Skills = new Skills();

            Slots = new Slots();
            

            Speed = (ushort)(2 * (Level - 1) + 220);
        }

        public TibiaGameClient Client { get; set; }

        public uint Experience { get; set; }

        public ushort Level { get; set; }

        public byte LevelPercent { get; set; }

        public ushort Mana { get; set; }

        public ushort MaxMana { get; set; }

        public byte Soul { get; set; }

        public uint Capacity { get; set; }

        public ushort Stamina { get; set; }

        public bool CanReportBugs { get; set; }

        public Skills Skills { get; set; }

        public Slots Slots { get; set; }        
    }
}