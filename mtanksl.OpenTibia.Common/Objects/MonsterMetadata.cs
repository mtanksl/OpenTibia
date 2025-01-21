using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class MonsterMetadata
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public ushort Speed { get; set; }

        public uint Experience { get; set; }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public Outfit Outfit { get; set; }

        public ushort Corpse { get; set; }
        
        public string[] Sentences { get; set; }

        public LootItem[] Loot { get; set; }

        public Dictionary<DamageType, double> DamageTakenFromElements { get; set; }
    }
}