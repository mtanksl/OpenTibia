using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class ItemMetadata
    {
        public ushort OpenTibiaId { get; set; }

        public ushort TibiaId { get; set; }

        public TopOrder TopOrder { get; set; }

        public ItemMetadataFlags Flags { get; set; }

        public ushort GroundSpeed { get; set; }

        public ushort MaxWriteChars { get; set; }

        public ushort MaxReadChars { get; set; }

        public Light Light { get; set; }
        
        public string Article { get; set; }

        public string Name { get; set; }

        public string Plural { get; set; }

        public string Description { get; set; }

        public string RuneSpellName { get; set; }

        public uint? Weight { get; set; }

        public byte? Armor { get; set; }

        public byte? Defense { get; set; }

        public byte? ExtraDefense { get; set; }

        public byte? Attack { get; set; }

        public FloorChange? FloorChange { get; set; }

        public Race? Race { get; set; }

        public byte? Capacity { get; set; }

        public WeaponType? WeaponType { get; set; }

        public AmmoType? AmmoType { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public byte? Range { get; set; }

        public byte? Charges { get; set; }

        public SlotType? SlotType { get; set; }

        public byte? BreakChance { get; set; }

        public AmmoAction? AmmoAction { get; set; }

        public byte? HitChance { get; set; }

        public byte? MaxHitChance { get; set; }

        public Dictionary<DamageType, double> DamageTakenFromElements { get; set; }

        public string GetDescription(byte type)
        {
            string description;

            if (Flags.Is(ItemMetadataFlags.Stackable) && type > 1)
            {
                if (Plural != null)
                {
                    description = type + " " + Plural;
                }
                else
                {
                    if (Name != null)
                    {
                        description = type + " " + Name;
                    }
                    else
                    {
                        description = "nothing special";
                    }
                }
            }
            else if ( (Flags.Is(ItemMetadataFlags.IsFluid) || Flags.Is(ItemMetadataFlags.IsSplash) ) && type > 0)
            {
                if (Name != null)
                {
                    if (Article != null)
                    {
                        description = Article + " " + Name + " of " + ( (FluidType)type ).GetDescription();
                    }
                    else
                    {
                        description = Name + " of " + ( (FluidType)type ).GetDescription();
                    }
                }
                else
                {
                    description = "nothing special";
                }
            }
            else
            {
                if (Name != null)
                {
                    if (Article != null)
                    {
                        description = Article + " " + Name;
                    }
                    else
                    {
                        description = Name;
                    }
                }
                else
                {
                    description = "nothing special";
                }
            }

            return description;
        }
    }
}