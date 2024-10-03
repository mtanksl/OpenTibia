using OpenTibia.IO;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.FileFormats.Otbm
{
    public class HouseTile : Tile
    {
        private static readonly List<Item> tempItems = new List<Item>();

        public static HouseTile Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
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
                                tempItems.Add( Item.Load(stream, reader) );

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

        public uint HouseId { get; set; }
    }
}