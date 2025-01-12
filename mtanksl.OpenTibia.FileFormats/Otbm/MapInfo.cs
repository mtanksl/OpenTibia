using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.FileFormats.Otbm
{
    public class MapInfo
    {
        public static MapInfo Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            MapInfo mapInfo = new MapInfo();

            stream.Seek(Origin.Current, 1); // OtbmType.MapInfo

            while (true)
            {
                switch ( (OtbmAttribute)reader.ReadByte() )
                {
                    case OtbmAttribute.Description:

                        mapInfo.descriptions.Add( reader.ReadString() );

                        break;

                    case OtbmAttribute.SpawnFile:

                        mapInfo.SpawnFile = reader.ReadString();

                        break;

                    case OtbmAttribute.HouseFile:

                        mapInfo.HouseFile = reader.ReadString();

                        break;

                    default:

                        stream.Seek(Origin.Current, -1);

                        return mapInfo;
                }
            }
        }

        public static void Save(MapInfo mapInfo, ByteArrayMemoryFileTreeStream stream, ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)OtbmType.MapInfo);

            foreach (var description in mapInfo.descriptions)
            {
                writer.Write( (byte)OtbmAttribute.Description);

                writer.Write( (string)description);
            }

            writer.Write( (byte)OtbmAttribute.SpawnFile);

            writer.Write( (string)mapInfo.SpawnFile);

            writer.Write( (byte)OtbmAttribute.HouseFile);

            writer.Write( (string)mapInfo.HouseFile);
        }

        private List<string> descriptions = new List<string>();

        public List<string> Descriptions
        {
            get
            {
                return descriptions;
            }
        }

        public string SpawnFile { get; set; }

        public string HouseFile { get; set; }
    }
}