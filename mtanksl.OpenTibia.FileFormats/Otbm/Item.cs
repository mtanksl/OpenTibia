using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.FileFormats.Otbm
{
    public class Item
    {
        private static readonly List<Item> tempItems = new List<Item>();

        public static Item Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader, in OtbmVersion otbmVersion, Func<ushort, ItemMetadata> getItemMetadataByOpenTibiaId)
        {
            Item item = new Item();

            stream.Seek(Origin.Current, 1); // OtbmType.Item

            item.OpenTibiaId = reader.ReadUShort();

            if (otbmVersion == OtbmVersion.Version1)
            {
                ItemMetadata itemMetadata = getItemMetadataByOpenTibiaId(item.OpenTibiaId);

                if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) || itemMetadata.Flags.Is(ItemMetadataFlags.IsFluid) || itemMetadata.Flags.Is(ItemMetadataFlags.IsSplash) )
                {
                    item.Count = reader.ReadByte();
                }
            }

            while (true)
            {
                switch ( (OtbmAttribute)reader.ReadByte() )
                {
                    case OtbmAttribute.Count:

                        item.Count = reader.ReadByte();

                        break;

                    case OtbmAttribute.ActionId:

                        item.ActionId = reader.ReadUShort();

                        break;

                    case OtbmAttribute.UniqueId:

                        item.UniqueId = reader.ReadUShort();

                        break;

                    case OtbmAttribute.Text:

                        item.Text = reader.ReadString();

                        break;

                    case OtbmAttribute.WrittenDate:

                        item.WrittenDate = reader.ReadUInt();

                        break;

                    case OtbmAttribute.WrittenBy:

                        item.WrittenBy = reader.ReadString();

                        break;

                    case OtbmAttribute.SpecialDescription:

                        item.SpecialDescription = reader.ReadString();

                        break;

                    case OtbmAttribute.RuneCharges:

                        item.RuneCharges = reader.ReadByte();

                        break;

                    case OtbmAttribute.Charges:

                        item.Charges = reader.ReadUShort();

                        break;

                    case OtbmAttribute.Duration:

                        item.Duration = reader.ReadUInt();

                        break;

                    case OtbmAttribute.Decaying:

                        item.Decaying = reader.ReadByte();

                        break;

                    case OtbmAttribute.DepotId:

                        item.TownId = reader.ReadUShort();

                        break;

                    case OtbmAttribute.HouseDoorId:

                        item.DoorId = reader.ReadByte();

                        break;

                    case OtbmAttribute.SleeperId:

                        item.SleeperId = reader.ReadUInt();

                        break;

                    case OtbmAttribute.SleepStart:

                        item.SleepterStart = reader.ReadUInt();

                        break;

                    case OtbmAttribute.TeleportDestination:

                        item.TeleportPosition = new Position(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte() );

                        break;

                    case OtbmAttribute.KeyNumber:

                        item.KeyNumber = reader.ReadUShort();

                        break;

                    case OtbmAttribute.KeyHoleNumber:

                        item.KeyHoleNumber = reader.ReadUShort();

                        break;

                    case OtbmAttribute.DoorQuestNumber:

                        item.DoorQuestNumber = reader.ReadUShort();

                        break;

                    case OtbmAttribute.DoorQuestValue:

                        item.DoorQuestValue = reader.ReadUShort();

                        break;

                    case OtbmAttribute.DoorLevel:

                        item.DoorLevel = reader.ReadUShort();

                        break;

                    case OtbmAttribute.ChestQuestNumber:

                        item.ChestQuestNumber = reader.ReadUShort();

                        break;

                    default:

                        stream.Seek(Origin.Current, -1);

                        if ( stream.Child() )
                        {
                            while (true)
                            {
                                tempItems.Add( Item.Load(stream, reader, otbmVersion, getItemMetadataByOpenTibiaId) );

                                if ( !stream.Next() )
                                {
                                    break;
                                }
                            }

                            item.items = tempItems.ToList();

                            tempItems.Clear();
                        }

                        return item;
                }
            }
        }

        public static void Save(Item item, ByteArrayMemoryFileTreeStream stream, ByteArrayStreamWriter writer, in OtbmVersion otbmVersion, Func<ushort, ItemMetadata> getItemMetadataByOpenTibiaId)
        {
            writer.Write( (byte)OtbmType.Item);

            writer.Write( (ushort)item.OpenTibiaId);

            if (otbmVersion == OtbmVersion.Version1)
            {
                ItemMetadata itemMetadata = getItemMetadataByOpenTibiaId(item.OpenTibiaId);

                if (itemMetadata.Flags.Is(ItemMetadataFlags.Stackable) || itemMetadata.Flags.Is(ItemMetadataFlags.IsFluid) || itemMetadata.Flags.Is(ItemMetadataFlags.IsSplash) )
                {
                    writer.Write( (byte)item.Count);
                }
            }
            else
            {
                if (item.Count > 0) // Don't need to write 0 for stackable, fluid nor splash items
                {
                    writer.Write( (byte)OtbmAttribute.Count);

                    writer.Write( (byte)item.Count);
                }
            }

            if (item.ActionId > 0)
            {
                writer.Write( (byte)OtbmAttribute.ActionId);

                writer.Write( (ushort)item.ActionId);
            }

            if (item.UniqueId > 0)
            {
                writer.Write( (byte)OtbmAttribute.UniqueId);

                writer.Write( (ushort)item.UniqueId);
            }

            if (item.Text != null)
            {
                writer.Write( (byte)OtbmAttribute.Text);

                writer.Write( (string)item.Text);
            }

            if (item.WrittenDate > 0)
            {
                writer.Write( (byte)OtbmAttribute.WrittenDate);

                writer.Write( (uint)item.WrittenDate);
            }

            if (item.WrittenBy != null)
            {
                writer.Write( (byte)OtbmAttribute.WrittenBy);

                writer.Write( (string)item.WrittenBy);
            }

            if (item.SpecialDescription != null)
            {
                writer.Write( (byte)OtbmAttribute.SpecialDescription);

                writer.Write( (string)item.SpecialDescription);
            }

            if (item.RuneCharges > 0)
            {
                writer.Write( (byte)OtbmAttribute.RuneCharges);

                writer.Write( (byte)item.RuneCharges);
            }

            if (item.Charges > 0)
            {
                writer.Write( (byte)OtbmAttribute.Charges);

                writer.Write( (ushort)item.Charges);
            }

            if (item.Duration > 0)
            {
                writer.Write( (byte)OtbmAttribute.Duration);

                writer.Write( (uint)item.Duration);
            }

            if (item.Decaying > 0)
            {
                writer.Write( (byte)OtbmAttribute.Decaying);

                writer.Write( (byte)item.Decaying);
            }

            if (item.TownId > 0)
            {
                writer.Write( (byte)OtbmAttribute.DepotId);

                writer.Write( (ushort)item.TownId);
            }

            if (item.DoorId > 0)
            {
                writer.Write( (byte)OtbmAttribute.HouseDoorId);

                writer.Write( (byte)item.DoorId);
            }

            if (item.SleeperId > 0)
            {
                writer.Write( (byte)OtbmAttribute.SleeperId);

                writer.Write( (uint)item.SleeperId);
            }

            if (item.SleepterStart > 0)
            {
                writer.Write( (byte)OtbmAttribute.SleepStart);

                writer.Write( (uint)item.SleepterStart);
            }

            if (item.TeleportPosition != null)
            {
                writer.Write( (byte)OtbmAttribute.TeleportDestination);

                writer.Write( (ushort)item.TeleportPosition.X);

                writer.Write( (ushort)item.TeleportPosition.Y);

                writer.Write( (byte)item.TeleportPosition.Z);
            }

            /*
            if (item.KeyNumber > 0)
            {
                writer.Write( (byte)OtbmAttribute.KeyNumber);

                writer.Write( (ushort)item.KeyNumber);
            }

            if (item.KeyHoleNumber > 0)
            {
                writer.Write( (byte)OtbmAttribute.KeyHoleNumber);

                writer.Write( (ushort)item.KeyHoleNumber);
            }

            if (item.DoorQuestNumber > 0)
            {
                writer.Write( (byte)OtbmAttribute.DoorQuestNumber);

                writer.Write( (ushort)item.DoorQuestNumber);
            }

            if (item.DoorQuestValue > 0)
            {
                writer.Write( (byte)OtbmAttribute.DoorQuestValue);

                writer.Write( (ushort)item.DoorQuestValue);
            }

            if (item.DoorLevel > 0)
            {
                writer.Write( (byte)OtbmAttribute.DoorLevel);

                writer.Write( (ushort)item.DoorLevel);
            }

            if (item.ChestQuestNumber > 0)
            {
                writer.Write( (byte)OtbmAttribute.ChestQuestNumber);

                writer.Write( (ushort)item.ChestQuestNumber);
            }
            */

            if (item.items != null)
            {
                foreach (var item2 in item.items)
                {
                    stream.StartChild();

                    Item.Save(item2, stream, writer, otbmVersion, getItemMetadataByOpenTibiaId);

                    stream.EndChild();
                }
            }
        }

        public ushort OpenTibiaId { get; set; }

        public byte Count { get; set; }

        public ushort ActionId { get; set; }

        public ushort UniqueId { get; set; }

        public string Text { get; set; }

        public uint WrittenDate { get; set; }

        public string WrittenBy { get; set; }

        public string SpecialDescription { get; set; }

        public byte RuneCharges { get; set; }

        public ushort Charges { get; set; }

        public uint Duration { get; set; }

        public byte Decaying { get; set; }

        public ushort TownId { get; set; }

        public byte DoorId { get; set; }

        public uint SleeperId { get; set; }

        public uint SleepterStart { get; set; }

        public Position TeleportPosition { get; set; }

        public ushort KeyNumber { get; set; }

        public ushort KeyHoleNumber { get; set; }

        public ushort DoorQuestNumber { get; set; }

        public ushort DoorQuestValue { get; set; }

        public ushort DoorLevel { get; set; }

        public ushort ChestQuestNumber { get; set; }

        private List<Item> items;

        public List<Item> Items
        {
            get
            {
                return items;
            }
        }
    }
}