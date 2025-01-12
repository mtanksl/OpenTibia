using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.FileFormats.Otbm
{
    public class Waypoint
    {
        public static Waypoint Load(ByteArrayFileTreeStream stream, ByteArrayStreamReader reader)
        {
            Waypoint waypoint = new Waypoint();

            stream.Seek(Origin.Current, 1); // OtbmType.Waypoint

            waypoint.Name = reader.ReadString();

            waypoint.Position = new Position(reader.ReadUShort(), reader.ReadUShort(), reader.ReadByte() );

            return waypoint;
        }

        public static void Save(Waypoint waypoint, ByteArrayMemoryFileTreeStream stream, ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)OtbmType.Waypoint);

            writer.Write( (string)waypoint.Name);

            writer.Write( (ushort)waypoint.Position.X);

            writer.Write( (ushort)waypoint.Position.Y);

            writer.Write( (byte)waypoint.Position.Z);
        }

        public string Name { get; set; }

        public Position Position { get; set; }
    }
}