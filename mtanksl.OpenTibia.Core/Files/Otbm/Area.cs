using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Otbm
{
    public class Area
    {
        public static Area Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Area area = new Area();

            area.Position = new Position(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte() );

            if ( stream.Child() )
            {
                area.tiles = new List<Tile>(1);

                while (true)
                {
                    Tile tile = null;

                    switch ( (OtbmType)reader.ReadByte() )
                    {
                        case OtbmType.Tile:

                            tile = Tile.Load(stream, reader);

                            break;

                        case OtbmType.HouseTile:

                            tile = HouseTile.Load(stream, reader);

                            break;
                    }
                    area.tiles.Add(tile);
                    
                    if ( !stream.Next() )
                    {
                        break; 
                    }
                }
            }
            return area;
        }

        public Position Position { get; set; }

        private List<Tile> tiles;

        public List<Tile> Tiles
        {
            get
            {
                return tiles;
            }
        }
    }
}