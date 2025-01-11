using OpenTibia.Common.Structures;
using OpenTibia.IO;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.FileFormats.Otbm
{
    public class Area
    {
        private static readonly List<Tile> tempTiles = new List<Tile>();

        public static Area Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Area area = new Area();

            area.Position = new Position(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte() );

            if ( stream.Child() )
            {
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

                    tempTiles.Add(tile);
                    
                    if ( !stream.Next() )
                    {
                        break; 
                    }
                }

                area.tiles = tempTiles.ToList();

                tempTiles.Clear();
            }

            return area;
        }

        public static void Save(Area area, ByteArrayMemoryFileTreeStream stream, ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)OtbmType.Area);

            writer.Write( (ushort)area.Position.X);

            writer.Write( (ushort)area.Position.Y);

            writer.Write( (byte)area.Position.Z);

            if (area.tiles != null)
            {
                foreach (var tile in area.tiles)
                {
                    stream.StartChild();

                    if (tile is HouseTile houseTile)
                    {
                        HouseTile.Save(houseTile, stream, writer);
                    }
                    else
                    {
                        Tile.Save(tile, stream, writer);
                    }

                    stream.EndChild();
                }
            }
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