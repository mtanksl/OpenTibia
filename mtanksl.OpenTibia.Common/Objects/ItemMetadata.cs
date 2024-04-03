using OpenTibia.Common.Structures;

namespace OpenTibia.Common.Objects
{
    public class ItemMetadata
    {
        public ushort OpenTibiaId { get; set; }

        public ushort TibiaId { get; set; }

        public TopOrder TopOrder { get; set; }

        public ItemMetadataFlags Flags { get; set; }

        public ushort Speed { get; set; }

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

        public byte? AttackStrength { get; set; }

        public byte? AttackVariation { get; set; }

        public FloorChange? FloorChange { get; set; }

        public byte? Capacity { get; set; }

        public WeaponType? WeaponType { get; set; }

        public AmmoType? AmmoType { get; set; }

        public ProjectileType? ProjectileType { get; set; }

        public MagicEffectType? MagicEffectType { get; set; }

        public byte? Range { get; set; }

        public byte? Charges { get; set; }

        public SlotType? SlotType { get; set; }
    }
}