using OpenTibia.IO;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.FileFormats.Otbm
{
    public class Tile
    {
        private static readonly List<Item> tempItems = new List<Item>();

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

                        tile.OpenTibiaItemId = reader.ReadUShort();

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

                            tile.items = tempItems.ToList();

                            tempItems.Clear();
                        }

                        return tile;
                }
            }
        }

        public byte OffsetX { get; set; }

        public byte OffsetY { get; set; }

        public TileFlags Flags { get; set; }

        public ushort OpenTibiaItemId { get; set; }

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