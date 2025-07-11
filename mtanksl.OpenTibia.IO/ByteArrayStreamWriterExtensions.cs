﻿using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Net;

namespace OpenTibia.IO
{
    public static class ByteArrayStreamWriterExtensions
    {       
        public static void Write(this IByteArrayStreamWriter writer, IHasFeatureFlag features, Outfit outfit)
        {
            if ( !features.HasFeatureFlag(FeatureFlag.LookTypeUInt16) )
            {
                writer.Write( (byte)outfit.Id);
            }
            else
            {
                writer.Write(outfit.Id);
            }

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

                if (features.HasFeatureFlag(FeatureFlag.PlayerAddons) )
                {
                    writer.Write( (byte)outfit.Addon );
                }                
            }

            if (features.HasFeatureFlag(FeatureFlag.PlayerMounts) )
            {
                writer.Write(outfit.Mount);
            }
        }

        public static void Write(this IByteArrayStreamWriter writer, Light light)
        {
            writer.Write(light.Level);

            writer.Write(light.Color);
        }

        public static void Write(this IByteArrayStreamWriter writer, IHasFeatureFlag features, Item item)
        {
            writer.Write(item.Metadata.TibiaId);

            if (features.HasFeatureFlag(FeatureFlag.ThingMarks) )
            {
                writer.Write( (byte)0xFF); //TODO: FeatureFlag.ThingMarks, 0xFF = Unmarked
            }

            switch (item)
            {
                case StackableItem stackable:

                    writer.Write( (byte)stackable.Count);

                    break;

                case FluidItem fluidItem:

                    writer.Write(features.GetByteForFluidType(fluidItem.FluidType) );

                    break;

                case SplashItem splashItem:

                    writer.Write(features.GetByteForFluidType(splashItem.FluidType) );

                    break;
            }

            if (features.HasFeatureFlag(FeatureFlag.ItemAnimationPhase) )
            {
                if (item.Metadata.Flags.Is(ItemMetadataFlags.IsAnimated) )
                {
                    writer.Write( (byte)0xFE); //TODO: FeatureFlag.IsAnimated, 0x00 = Automatic Phase, 0xFE = Random Phase, 0xFF = Async Phase
                }
            }
        }

        /// <summary>
        /// Known creature.
        /// </summary>
        public static void Write(this IByteArrayStreamWriter writer, IHasFeatureFlag features, Creature creature, SkullIcon skullIcon, PartyIcon partyIcon)
        {
             writer.Write( (ushort)0x62 );

             writer.Write(creature.Id);

             writer.Write(creature.HealthPercentage);

             writer.Write( (byte)creature.Direction );

             writer.Write(features, creature.ClientOutfit);

             writer.Write(creature.ClientLight);

            if ( !features.HasFeatureFlag(FeatureFlag.NewSpeedLaw) )
            {
                writer.Write(creature.ClientSpeed);
            }
            else
            {
                writer.Write( (ushort)(creature.ClientSpeed / 2) );
            }

            writer.Write( (byte)skullIcon);

            writer.Write( (byte)partyIcon);

            if (features.HasFeatureFlag(FeatureFlag.ThingMarks) )
            {
                byte creatureType = (byte)(creature is Player ? 0x00 : creature is Monster ? 0x01 : creature is Npc ? 0x02 : 0x00);

                writer.Write(creatureType); //TODO: FeatureFlag.ThingMarks, 0x03 = Own Summon, 0x04 = Other Summon
            }

            if (features.HasFeatureFlag(FeatureFlag.CreatureIcons) )
            {
                writer.Write( (byte)SpeechBubble.None); //TODO: FeatureFlag.CreatureIcons, speech bubble
            }

            if (features.HasFeatureFlag(FeatureFlag.ThingMarks) )
            {
                writer.Write( (byte)0xFF); //TODO: FeatureFlag.ThingMarks, 0xFF = Unmarked

                writer.Write( (ushort)0); //TODO: FeatureFlag.ThingMarks, helpers
            }

            if (features.HasFeatureFlag(FeatureFlag.CreatureBlock) )
            {
                writer.Write(creature.Block);
            }
        }

        /// <summary>
        /// Unknown creature.
        /// </summary>
        public static void Write(this IByteArrayStreamWriter writer, IHasFeatureFlag features, uint removeId, Creature creature, SkullIcon skullIcon, PartyIcon partyIcon, WarIcon warIcon)
        {
            writer.Write( (ushort)0x61 );

            writer.Write(removeId);

            writer.Write(creature.Id);

            byte creatureType = (byte)(creature is Player ? 0x00 : creature is Monster ? 0x01 : creature is Npc ? 0x02 : 0x00);

            if (features.HasFeatureFlag(FeatureFlag.CreatureType) )
            {
                writer.Write(creatureType);
            }

            writer.Write(creature.Name);

            writer.Write(creature.HealthPercentage);

            writer.Write( (byte)creature.Direction );

            writer.Write(features, creature.ClientOutfit);

            writer.Write(creature.ClientLight);

            if ( !features.HasFeatureFlag(FeatureFlag.NewSpeedLaw) )
            {
                writer.Write(creature.ClientSpeed);
            }
            else
            {
                writer.Write( (ushort)(creature.ClientSpeed / 2) );
            }

            writer.Write( (byte)skullIcon);

            writer.Write( (byte)partyIcon);

            if (features.HasFeatureFlag(FeatureFlag.CreatureWarIcon) )
            {
                writer.Write( (byte)warIcon);
            }

            if (features.HasFeatureFlag(FeatureFlag.ThingMarks) )
            {
                writer.Write(creatureType); //TODO: FeatureFlag.ThingMarks, 0x03 = Own Summon, 0x04 = Other Summon
            }

            if (features.HasFeatureFlag(FeatureFlag.CreatureIcons) )
            {
                writer.Write( (byte)SpeechBubble.None); //TODO: FeatureFlag.CreatureIcons, speech bubble
            }

            if (features.HasFeatureFlag(FeatureFlag.ThingMarks) )
            {
                writer.Write( (byte)0xFF); //TODO: FeatureFlag.ThingMarks, 0xFF = Unmarked

                writer.Write( (ushort)0); //TODO: FeatureFlag.ThingMarks, helpers
            }

            if (features.HasFeatureFlag(FeatureFlag.CreatureBlock) )
            {
                writer.Write(creature.Block);
            }
        }

        public static void Write(this IByteArrayStreamWriter writer, IPAddress ipAddress)
        {
            writer.Write( ipAddress.GetAddressBytes() );
        }
    }
}