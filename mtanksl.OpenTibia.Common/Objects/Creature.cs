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

            Light = Light.None;

            BaseOutfit = Outfit = Outfit.MaleCitizen;

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

        public Outfit BaseOutfit { get; set; }

        public Outfit Outfit { get; set; }

        public ushort BaseSpeed { get; set; }

        public ushort Speed { get; set; }

        public SkullIcon SkullIcon { get; set; }

        public PartyIcon PartyIcon { get; set; }

        public WarIcon WarIcon { get; set; }

        public bool Block { get; set; }

        public bool Invisible { get; set; }

        public SpecialCondition SpecialConditions { get; set; }

        public Tile Spawn { get; set; }

        public bool HasSpecialCondition(SpecialCondition specialCondition)
        {
            return (SpecialConditions & specialCondition) == specialCondition;
        }

        public void AddSpecialCondition(SpecialCondition specialCondition)
        {
            SpecialConditions |= specialCondition;
        }

        public void RemoveSpecialCondition(SpecialCondition specialCondition)
        {
            SpecialConditions &= ~specialCondition;
        }

        public override string ToString()
        {
            return "Id: " + Id + " Name: " + Name;
        }
    }
}