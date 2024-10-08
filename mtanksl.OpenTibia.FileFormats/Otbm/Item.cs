﻿using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.FileFormats.Otbm
{
    public class Item
    {
        private static readonly List<Item> tempItems = new List<Item>();

        public static Item Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Item item = new Item();

            stream.Seek(Origin.Current, 1);

            item.OpenTibiaId = reader.ReadUShort();

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

                    case OtbmAttribute.ContainerItems:

                        item.ContainerItems = reader.ReadUInt();

                        break;

                    default:

                        stream.Seek(Origin.Current, -1);

                        if ( stream.Child() )
                        {
                            while (true)
                            {
                                tempItems.Add( Item.Load(stream, reader) );

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

        public uint ContainerItems { get; set; }

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