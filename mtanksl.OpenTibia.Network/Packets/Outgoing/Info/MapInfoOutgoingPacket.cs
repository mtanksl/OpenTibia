using OpenTibia.IO;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Network.Packets.Incoming
{
    public class MapInfoOutgoingPacket : IOutgoingPacket
    {
        public MapInfoOutgoingPacket(string mapName, string mapAuthor, ushort mapWidth, ushort mapHeight)
        {
            MapName = mapName;

            MapAuthor = mapAuthor;

            MapWidth = mapWidth;

            MapHeight = mapHeight;
        }

        public string MapName { get; set; }

        public string MapAuthor { get; set; }

        public ushort MapWidth { get; set; }

        public ushort MapHeight { get; set; }

        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x30);

            writer.Write(MapName);

            writer.Write(MapAuthor);

            writer.Write(MapWidth);

            writer.Write(MapHeight);
        }
    }
}