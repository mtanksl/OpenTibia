using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class Player : Creature
    {
        public Player()
        {
            Inventory = new Inventory(this);

            Lockers = new Safe(this);

            Achievements = new PlayerAchievementsCollection();

            Blesses = new PlayerBlessCollection();

            Combat = new PlayerCombatCollection(this);

            Outfits = new PlayerOutfitCollection();

            Spells = new PlayerSpellCollection();

            Storages = new PlayerStorageCollection();

            Vips = new PlayerVipCollection();

            Skills = new Skills(this);

            Experience = 0;

            Level = 1;

            LevelPercent = 0;

            MaxMana = Mana = 55;

            Soul = 100;

            MaxCapacity = Capacity = 400 * 100;

            Stamina = 42 * 60;
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

        public int DatabasePlayerId { get; set; }

        public int DatabaseAccountId { get; set; }

        public Inventory Inventory { get; }

        public Safe Lockers { get; }

        public PlayerAchievementsCollection Achievements { get; }

        public PlayerBlessCollection Blesses { get; }

        public PlayerCombatCollection Combat { get; set; }

        public PlayerOutfitCollection Outfits { get; }

        public PlayerSpellCollection Spells { get; }

        public PlayerStorageCollection Storages { get; }

        public PlayerVipCollection Vips { get; }

        public Skills Skills { get; set; }

        public ulong Experience { get; set; }

        public ushort Level { get; set; }

        public byte LevelPercent { get; set; }

        public ushort Mana { get; set; }

        public ushort MaxMana { get; set; }

        public byte Soul { get; set; }

        public uint Capacity { get; set; }

        public uint MaxCapacity { get; set; }

        public ushort Stamina { get; set; }

        public Gender Gender { get; set; }

        public Vocation Vocation { get; set; }

        public Rank Rank { get; set; }

        public bool Premium { get; set; }

        public ulong BankAccount { get; set; }

        public override Light Light
        {
            get 
            {
                if (ConditionLight != null)
                {
                    return ConditionLight;
                }

                //TODO: Item light from inventory

                return BaseLight;
            }
        }

        public override ushort Speed
        {
            get
            {
                int sum = BaseSpeed;

                if (ConditionSpeed != null)
                {
                    sum += ConditionSpeed.Value;
                }

                //TODO: Item speed from inventory

                return (ushort)(sum);
            }
        }
    }
}