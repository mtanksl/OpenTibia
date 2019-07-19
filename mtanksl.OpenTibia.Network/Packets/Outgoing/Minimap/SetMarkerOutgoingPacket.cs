using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SetMarkerOutgoingPacket : IOutgoingPacket
    {
        public SetMarkerOutgoingPacket(Position position, Marker marker, string description)
        {
            this.Position = position;

            this.Marker = marker;

            this.Description = description;
        }

        public Position Position { get; set; }

        public Marker Marker { get; set; }

        public string Description { get; set; }

        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xDD );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            writer.Write( (byte)Marker );

            writer.Write(Description);
        }
    }
}