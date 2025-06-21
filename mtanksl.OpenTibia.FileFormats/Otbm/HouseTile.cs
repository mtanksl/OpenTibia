using OpenTibia.Common.Objects;
using OpenTibia.IO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.FileFormats.Otbm
{
    public class HouseTile : Tile
    {
        private static readonly List<Item> tempItems = new List<Item>();

        public static HouseTile Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader, in OtbmVersion otbmVersion, Func<ushort, ItemMetadata> getItemMetadataByOpenTibiaId)
        {
            HouseTile houseTile = new HouseTile();

            houseTile.OffsetX = reader.ReadByte();

            houseTile.OffsetY = reader.ReadByte();

            houseTile.HouseId = reader.ReadUInt();

            while (true)
            {
                switch ( (OtbmAttribute)reader.ReadByte() )
                {
                    case OtbmAttribute.Flags:

                        houseTile.Flags = (TileFlags)reader.ReadUInt();

                        break;

                    case OtbmAttribute.ItemId:

                        houseTile.OpenTibiaItemId = reader.ReadUShort();

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

                            houseTile.items = tempItems.ToList();

                            tempItems.Clear();
                        }

                        return houseTile;
                }
            }
        }

        public static void Save(HouseTile houseTile, ByteArrayMemoryFileTreeStream stream, ByteArrayStreamWriter writer, in OtbmVersion otbmVersion, Func<ushort, ItemMetadata> getItemMetadataByOpenTibiaId)
        {
            writer.Write( (byte)OtbmType.HouseTile);

            writer.Write( (byte)houseTile.OffsetX);

            writer.Write( (byte)houseTile.OffsetY);

            writer.Write( (uint)houseTile.HouseId);

            if (houseTile.Flags > 0)
            {
                writer.Write( (byte)OtbmAttribute.Flags);

                writer.Write( (uint)houseTile.Flags);
            }

            if (houseTile.OpenTibiaItemId > 0)
            {
                writer.Write( (byte)OtbmAttribute.ItemId);

                writer.Write( (ushort)houseTile.OpenTibiaItemId);
            }

            if (houseTile.items != null)
            {
                foreach (var item in houseTile.items)
                {
                    stream.StartChild();

                    Item.Save(item, stream, writer, otbmVersion, getItemMetadataByOpenTibiaId);

                    stream.EndChild();
                }
            }
        }

        public uint HouseId { get; set; }
    }
}