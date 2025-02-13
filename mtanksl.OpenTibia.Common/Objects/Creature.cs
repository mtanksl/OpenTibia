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

            BaseLight = Light.None;

            BaseOutfit = Outfit.MaleCitizen;

            BaseSpeed = 220;

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
                return (Tile)Parent;
            }
        }
        
        public string Name { get; set; }

        public ushort Health { get; set; }

        public ushort MaxHealth { get; set; }

        public byte HealthPercentage
        {
            get
            {
                return (byte)Math.Max(0, Math.Min(100, Math.Ceiling(100.0 * Health / MaxHealth) ) );       
            }
        }

        public Direction Direction { get; set; }

        public Light BaseLight { get; set; }

        public Light Light
        {
            get
            {
                return BaseLight;
            }
        }

        public Outfit BaseOutfit { get; set; }

        public Outfit Outfit
        {
            get
            {
                return BaseOutfit;
            }
        }

        public ushort BaseSpeed { get; set; }

        public ushort Speed
        {
            get
            {
                return BaseSpeed;
            }
        }

        public bool Block { get; set; }

        public bool Invisible { get; set; }

        public SpecialCondition SpecialConditions { get; set; }

        public Tile Town { get; set; }

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