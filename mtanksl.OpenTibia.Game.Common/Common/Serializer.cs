using OpenTibia.Common.Objects;
using OpenTibia.FileFormats.Otbm;
using OpenTibia.IO;
using System;
using System.Linq;
using Item = OpenTibia.Common.Objects.Item;

namespace OpenTibia.Game.Common
{
    public static class Serializer
    {
        private static byte[] temp;

        private static ByteArrayArrayStream tempStream;

        private static ByteArrayStreamWriter tempWriter;

        private static ByteArrayStreamReader tempReader;

        static Serializer()
        {
            temp = new byte[65535];

            tempStream = new ByteArrayArrayStream(temp);

            tempWriter = new ByteArrayStreamWriter(tempStream);

            tempReader = new ByteArrayStreamReader(tempStream);
        }

        public static byte[] SerializeItemAttributes(Item item)
        {
            tempStream.Seek(Origin.Begin, 0);

            if (item.ActionId > 0)
            {
                tempWriter.Write( (byte)OtbmAttribute.ActionId);

                tempWriter.Write( (ushort)item.ActionId);
            }

            if (item.UniqueId > 0)
            {
                tempWriter.Write( (byte)OtbmAttribute.UniqueId);

                tempWriter.Write( (ushort)item.UniqueId);
            }

            if (item is ReadableItem readableItem)
            {
                if (readableItem.Text != null)
                {
                    tempWriter.Write( (byte)OtbmAttribute.Text);

                    tempWriter.Write( (string)readableItem.Text);
                }

                if (readableItem.WrittenDate > 0)
                {
                    tempWriter.Write( (byte)OtbmAttribute.WrittenDate);

                    tempWriter.Write( (uint)readableItem.WrittenDate);
                }

                if (readableItem.WrittenBy != null)
                {
                    tempWriter.Write( (byte)OtbmAttribute.WrittenBy);

                    tempWriter.Write( (string)readableItem.WrittenBy);
                }
            }

            if (item.Charges > 0)
            {
                tempWriter.Write( (byte)OtbmAttribute.Charges);

                tempWriter.Write( (ushort)item.Charges);
            }

            if (item.DurationInMilliseconds > 0)
            {
                tempWriter.Write( (byte)OtbmAttribute.Duration);

                tempWriter.Write( (uint)item.DurationInMilliseconds);
            }

            if (tempStream.Position > 0)
            {
                return temp.Take(tempStream.Position).ToArray();
            }

            return null;
        }

        public static void DeserializeItemAttributes(Item item, byte[] attributes)
        {
            if (attributes != null)
            {
                tempStream.Seek(Origin.Begin, 0);

                tempStream.Write(attributes, 0, attributes.Length);

                tempStream.Seek(Origin.Begin, 0);

                while (tempStream.Position < attributes.Length)
                {
                    switch ( (OtbmAttribute)tempReader.ReadByte() )
                    {
                        case OtbmAttribute.ActionId:

                            item.ActionId = tempReader.ReadUShort();

                            break;

                        case OtbmAttribute.UniqueId:

                            item.UniqueId = tempReader.ReadUShort();

                            break;

                        case OtbmAttribute.Text:

                            ( (ReadableItem)item ).Text = tempReader.ReadString();

                            break;

                        case OtbmAttribute.WrittenDate:

                            ( (ReadableItem)item ).WrittenDate = tempReader.ReadUInt();

                            break;

                        case OtbmAttribute.WrittenBy:

                            ( (ReadableItem)item ).WrittenBy = tempReader.ReadString();

                            break;

                        case OtbmAttribute.Charges:

                            item.Charges = (int)tempReader.ReadUShort();

                            break;
                                                         
                        case OtbmAttribute.Duration:

                            item.DurationInMilliseconds = (int)tempReader.ReadUInt();

                            break;

                        default:

                            throw new NotImplementedException();
                    }
                }
            }
        }
    }
}