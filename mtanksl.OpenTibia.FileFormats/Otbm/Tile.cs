using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.FileFormats.Otbm
{
    public class Tile
    {
        public static Tile Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Tile tile = new Tile();

            tile.OffsetX = reader.ReadByte();

            tile.OffsetY = reader.ReadByte();

            while (true)
            {
                switch ( (OtbmAttribute)reader.ReadByte() )
                {
                    case OtbmAttribute.Flags:

                        tile.Flags = (TileFlags)reader.ReadUInt();

                        break;

                    case OtbmAttribute.ItemId:

                        tile.ItemId = reader.ReadUShort();

                        break;

                    default:

                        stream.Seek(Origin.Current, -1);
                        
                        if ( stream.Child() )
                        {
                            tile.items = new List<Item>();

                            while (true)
                            {
                                tile.items.Add( Item.Load(stream, reader) );

                                if ( !stream.Next() )
                                {
                                    break;
                                }
                            }
                        }

                        return tile;
                }
            }
        }

        public byte OffsetX { get; set; }

        public byte OffsetY { get; set; }

        public TileFlags Flags { get; set; }

        public ushort ItemId { get; set; }

        protected List<Item> items;

        public List<Item> Items
        {
            get
            {
                return items;
            }
        }
    }
}