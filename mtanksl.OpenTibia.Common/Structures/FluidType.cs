namespace OpenTibia.Common.Structures
{
    public enum FluidType : byte
    {
        Empty = 0,

        Water = 1,

        Blood = 2,

        Beer = 3,

        Slime = 4,

        Lemonade = 5,

        Milk = 6,

        Manafluid = 7,

        Lifefluid = 10,

        Oil = 11,

        Urine = 13,

        CoconutMilk = 14,

        Wine = 15,

        Mud = 19,

        FruitJuice = 21,

        Lava = 26,

        Rum = 27,

        Swamp = 28
    }

    public static class FluidTypeExtensions
    {
        public static string GetDescription(this FluidType fluidType)
        {
            switch (fluidType)
            {
                case FluidType.Water:

                    return "water";

                case FluidType.Blood:

                    return "blood";

                case FluidType.Beer:

                    return "beer";

                case FluidType.Slime:

                    return "slime";

                case FluidType.Lemonade:

                    return "lemonade";

                case FluidType.Milk:

                    return "milk";

                case FluidType.Manafluid:

                    return "manafluid";

                case FluidType.Lifefluid:

                    return "lifefluid";

                case FluidType.Oil:

                    return "oil";

                case FluidType.Urine:

                    return "urine";

                case FluidType.CoconutMilk:

                    return "coconut milk";

                case FluidType.Wine:

                    return "wine";

                case FluidType.Mud:

                    return "mud";

                case FluidType.FruitJuice:

                    return "fruit juice";

                case FluidType.Lava:

                    return "lava";

                case FluidType.Rum:

                    return "rum";

                case FluidType.Swamp:

                    return "swamp";
            }

            return null;
        }
    }
}