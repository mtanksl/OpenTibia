namespace OpenTibia.Common.Objects
{
    public class Player : Creature
    {
        public Player()
        {
            Inventory = new Inventory(this);

            Skills = new Skills(this)
            {
                Fist = 10,

                Club = 10,

                Sword = 10,

                Axe = 10,

                Distance = 10,

                Shield = 10,

                Fish = 10
            };

            Experience = 0;

            Level = 1;

            LevelPercent = 0;

            Mana = 50;

            MaxMana = 50;

            Soul = 100;

            Capacity = 10000;

            Stamina = 42 * 60;

            CanReportBugs = true;
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

        public Inventory Inventory { get; }

        public Skills Skills { get; set; }

        public uint Experience { get; set; }

        public ushort Level { get; set; }

        public byte LevelPercent { get; set; }

        public ushort Mana { get; set; }

        public ushort MaxMana { get; set; }

        public byte Soul { get; set; }

        public uint Capacity { get; set; }

        public ushort Stamina { get; set; }

        public bool CanReportBugs { get; set; }
    }
}