using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Net;

namespace OpenTibia.IO
{
    public static class ByteArrayStreamWriterExtensions
    {       
        public static void Write(this IByteArrayStreamWriter writer, Outfit outfit)
        {
            writer.Write(outfit.Id);

            if (outfit.Id == 0)
            {
                writer.Write(outfit.TibiaId);
            }
            else
            {
                writer.Write(outfit.Head);

                writer.Write(outfit.Body);

                writer.Write(outfit.Legs);

                writer.Write(outfit.Feet);

                writer.Write( (byte)outfit.Addon );
            }
        }

        public static void Write(this IByteArrayStreamWriter writer, Light light)
        {
            writer.Write(light.Level);

            writer.Write(light.Color);
        }

        private static FluidColor[] FluidColors = new FluidColor[]
        {
            FluidColor.Empty,

            FluidColor.Blue,

            FluidColor.Red,

            FluidColor.Brown1,

            FluidColor.Green,

            FluidColor.Yellow,

            FluidColor.White,

            FluidColor.Purple
        };

        public static void Write(this IByteArrayStreamWriter writer, Item item)
        {
            writer.Write(item.Metadata.TibiaId);

            switch (item)
            {
                case StackableItem stackable:

                    writer.Write( (byte)stackable.Count);

                    break;

                case FluidItem fluidItem:

                    writer.Write( (byte)FluidColors[ (int)fluidItem.FluidType % FluidColors.Length ] );

                    break;

                case SplashItem splashItem:

                    writer.Write( (byte)FluidColors[ (int)splashItem.FluidType % FluidColors.Length ] );

                    break;
            }
        }

        /// <summary>
        /// Known creature.
        /// </summary>
        public static void Write(this IByteArrayStreamWriter writer, Creature creature, SkullIcon skullIcon, PartyIcon partyIcon)
        {
             writer.Write( (ushort)0x62 );

             writer.Write(creature.Id);

             writer.Write(creature.HealthPercentage);

             writer.Write( (byte)creature.Direction );

             writer.Write(creature.ClientOutfit);

             writer.Write(creature.ClientLight);

             writer.Write(creature.ClientSpeed);

             writer.Write( (byte)skullIcon);

             writer.Write( (byte)partyIcon);

             writer.Write(creature.Block);
        }

        /// <summary>
        /// Unknown creature.
        /// </summary>
        public static void Write(this IByteArrayStreamWriter writer, uint removeId, Creature creature, SkullIcon skullIcon, PartyIcon partyIcon, WarIcon warIcon)
        {
            writer.Write( (ushort)0x61 );

            writer.Write(removeId);

            writer.Write(creature.Id);

            writer.Write(creature.Name);

            writer.Write(creature.HealthPercentage);

            writer.Write( (byte)creature.Direction );

            writer.Write(creature.ClientOutfit);

            writer.Write(creature.ClientLight);

            writer.Write(creature.ClientSpeed);

            writer.Write( (byte)skullIcon);

            writer.Write( (byte)partyIcon);

            writer.Write( (byte)warIcon);

            writer.Write(creature.Block);
        }

        public static void Write(this IByteArrayStreamWriter writer, IPAddress ipAddress)
        {
            writer.Write( ipAddress.GetAddressBytes() );
        }
    }
}