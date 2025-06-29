using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendStatusOutgoingPacket : IOutgoingPacket
    {
        public SendStatusOutgoingPacket(ushort health, ushort maxHealth, uint capacity, uint maxCapacity, ulong experience, ushort level, byte levelPercent, ushort mana, ushort maxMana, ushort magicLevel, ushort baseMagicLevel, byte magicLevelPercent, byte soul, ushort stamina, ushort baseSpeed)
        {
            this.Health = health;

            this.MaxHealth = maxHealth;

            this.Capacity = capacity;

            this.MaxCapacity = maxCapacity;

            this.Experience = experience;

            this.Level = level;

            this.LevelPercent = levelPercent;

            this.Mana = mana;

            this.MaxMana = maxMana;

            this.MagicLevel = magicLevel;

            this.BaseMagicLevel = baseMagicLevel;

            this.MagicLevelPercent = magicLevelPercent;

            this.Soul = soul;

            this.Stamina = stamina;

            this.BaseSpeed = baseSpeed;
        }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public uint Capacity { get; set; }

        public uint MaxCapacity { get; set; }

        public ulong Experience { get; set; }

        public ushort Level { get; set; }

        public byte LevelPercent { get; set; }

        public ushort Mana { get; set; }

        public ushort MaxMana { get; set; }

        public ushort MagicLevel { get; set; }

        public ushort BaseMagicLevel { get; set; }

        public byte MagicLevelPercent { get; set; }

        public byte Soul { get; set; }

        public ushort Stamina { get; set; }

        public ushort BaseSpeed { get; set; }

        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xA0 );

            writer.Write(Health);

            writer.Write(MaxHealth);

            if ( !features.HasFeatureFlag(FeatureFlag.PlayerCapacityUInt32) )
            {
                writer.Write( (ushort)(Capacity / 100) );
            }
            else
            {
                writer.Write(Capacity);
            }

            if (features.HasFeatureFlag(FeatureFlag.PlayerTotalCapacity) )
            {
                writer.Write(MaxCapacity);
            }

            if ( !features.HasFeatureFlag(FeatureFlag.PlayerExperienceUInt64) )
            {
                writer.Write( (uint)Math.Min(int.MaxValue, Experience) );
            }
            else
            {
                writer.Write(Experience);
            }

            writer.Write(Level);

            writer.Write(LevelPercent);

            if (features.HasFeatureFlag(FeatureFlag.ExperienceBonus) )
            {
                if ( !features.HasFeatureFlag(FeatureFlag.MultipleExperienceBonus) )
                {
                    writer.Write( (double)0, 2); // Experience bonus
                }
                else
                {
                    writer.Write( (ushort)100); // Base xp gain

                    writer.Write( (ushort)0); // Voucher addend

                    writer.Write( (ushort)0); // Grinding addend

                    writer.Write( (ushort)0); // Store boost addend

                    writer.Write( (ushort)100); // Hunting boost factor
                }
            }

            writer.Write(Mana);

            writer.Write(MaxMana);

            writer.Write( (byte)MagicLevel);

            if (features.HasFeatureFlag(FeatureFlag.PlayerSkillsBase) )
            {
                writer.Write( (byte)BaseMagicLevel);
            }

            writer.Write(MagicLevelPercent);

            writer.Write(Soul);

            if (features.HasFeatureFlag(FeatureFlag.PlayerStamina) )
            {
                writer.Write(Stamina);
            }

            if (features.HasFeatureFlag(FeatureFlag.PlayerSkillsBase) )
            {
                if ( !features.HasFeatureFlag(FeatureFlag.NewSpeedLaw) )
                {
                    writer.Write(BaseSpeed);
                }
                else
                {
                    writer.Write( (ushort)(BaseSpeed / 2) );
                }
            }

            if (features.HasFeatureFlag(FeatureFlag.PlayerRegenerationTime) )
            {
                writer.Write( (ushort)0 ); //TODO: FeatureFlag.PlayerRegenerationTime
            }

            if (features.HasFeatureFlag(FeatureFlag.OfflineTrainingTime) )
            {
                writer.Write( (ushort)0 ); //TODO: FeatureFlag.OfflineTrainingTime
            }

            if (features.HasFeatureFlag(FeatureFlag.MultipleExperienceBonus) )
            {
                writer.Write( (ushort)0 ); //TODO: FeatureFlag.MultipleExperienceBonus, remaining store xp boost seconds

                writer.Write( (byte)0 ); //TODO: FeatureFlag.MultipleExperienceBonus, can buy more xp boosts
            }
        }
    }
}