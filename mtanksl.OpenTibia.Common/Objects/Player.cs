namespace OpenTibia.Common.Objects
{
    public class Player : Creature
    {
        public Player()
        {
            Experience = 0;

            Level = 1;

            LevelPercent = 0;

            Mana = 50;

            MaxMana = 50;

            Soul = 100;

            Capacity = 10000;

            Stamina = 42 * 60;

            CanReportBugs = true;

            Inventory = new Inventory(this);
        }

        private IClient client;

        public IClient Client
        {
            get
            {
                return client;
            }
            set
            {
                if (value != client)
                {
                    var current = client;

                                  client = value;

                    if (value == null)
                    {
                        current.Player = null;
                    }
                    else
                    {
                        client.Player = this;
                    }
                }
            }
        }

        public uint Experience { get; set; }

        public ushort Level { get; set; }

        public byte LevelPercent { get; set; }

        public ushort Mana { get; set; }

        public ushort MaxMana { get; set; }

        public byte Soul { get; set; }

        public uint Capacity { get; set; }

        public ushort Stamina { get; set; }

        public bool CanReportBugs { get; set; }

        public Inventory Inventory { get; set; }
    }
}