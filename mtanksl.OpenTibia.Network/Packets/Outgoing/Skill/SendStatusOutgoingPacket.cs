using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendStatusOutgoingPacket : IOutgoingPacket
    {
        public SendStatusOutgoingPacket(ushort health, ushort maxHealth, uint capacity, uint experience, ushort level, byte levelPercent, ushort mana, ushort maxMana, byte magicLevel, byte magicLevelPercent, byte soul, ushort stamina)
        {
            this.Health = health;

            this.MaxHealth = maxHealth;

            this.Capacity = capacity;

            this.Experience = experience;

            this.Level = level;

            this.LevelPercent = levelPercent;

            this.Mana = mana;

            this.MaxMana = maxMana;

            this.MagicLevel = magicLevel;

            this.MagicLevelPercent = magicLevelPercent;

            this.Soul = soul;

            this.Stamina = stamina;
        }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public uint Capacity { get; set; }

        public uint Experience { get; set; }

        public ushort Level { get; set; }

        public byte LevelPercent { get; set; }

        public ushort Mana { get; set; }

        public ushort MaxMana { get; set; }

        public byte MagicLevel { get; set; }

        public byte MagicLevelPercent { get; set; }

        public byte Soul { get; set; }

        public ushort Stamina { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xA0 );

            writer.Write(Health);

            writer.Write(MaxHealth);

            writer.Write(Capacity);

            writer.Write(Experience);

            writer.Write(Level);

            writer.Write(LevelPercent);

            writer.Write(Mana);

            writer.Write(MaxMana);

            writer.Write(MagicLevel);

            writer.Write(MagicLevelPercent);

            writer.Write(Soul);

            writer.Write(Stamina);
        }
    }
}