using OpenTibia.Common.Structures;
using System;

namespace OpenTibia.Common.Objects
{
    public abstract class Creature : GameObject, IContent
    {
        public Creature()
        {
            MaxHealth = Health = 150;

            Direction = Direction.South;

            Light = new Light(0, 0);

            Outfit = new Outfit(266, 0, 0, 0, 0, Addon.None);

            BaseSpeed = Speed = 220;

            SkullIcon = SkullIcon.None;

            PartyIcon = PartyIcon.None;

            WarIcon = WarIcon.None;

            Block = true;
        }

        public TopOrder TopOrder
        {
            get
            {
                return TopOrder.Creature;
            }
        }

        public IContainer Parent { get; set; }

        public Tile Tile
        {
            get
            {
                return Parent as Tile;
            }
        }
        
        public string Name { get; set; }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public byte HealthPercentage
        {
            get
            {
                return (byte)Math.Ceiling(100.0 * Health / MaxHealth);
            }
        }

        public Direction Direction { get; set; }

        public Light Light { get; set; }

        public Outfit Outfit { get; set; }

        public ushort BaseSpeed { get; set; }

        public ushort Speed { get; set; }

        public SkullIcon SkullIcon { get; set; }

        public PartyIcon PartyIcon { get; set; }

        public WarIcon WarIcon { get; set; }

        public bool Block { get; set; }
    }
}