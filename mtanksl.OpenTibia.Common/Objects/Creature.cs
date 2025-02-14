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

        public Light ConditionLight { get; set; }

        public virtual Light Light
        {
            get
            {
                if (ConditionLight != null)
                {
                    return ConditionLight;
                }

                return BaseLight;
            }
        }

        public Outfit BaseOutfit { get; set; }

        public Outfit ConditionOutfit { get; set; }

        public bool Swimming { get; set; }

        public bool ConditionStealth { get; set; }

        public virtual Outfit Outfit
        {
            get
            {
                if (Invisible)
                {
                    return Outfit.Invisible;
                }

                if (ConditionStealth)
                {
                    return Outfit.Invisible;
                }

                if (Swimming)
                {
                    return Outfit.Swimming;
                }

                if (ConditionOutfit != null)
                {
                    return ConditionOutfit;
                }

                return BaseOutfit;
            }
        }

        public ushort BaseSpeed { get; set; }

        public int? ConditionSpeed { get; set; }

        public virtual ushort Speed
        {
            get
            {
                int sum = BaseSpeed;

                if (ConditionSpeed != null)
                {
                    sum += ConditionSpeed.Value;
                }

                return (ushort)(sum);
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