using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendTilesOutgoingPacket : SendMapOutgoingPacket
    {
        public SendTilesOutgoingPacket(IMapGetTile map, IClient client, Position position) : base(map, client)
        {
            this.Position = position;
        }

        public Position Position { get; set; }

        public override void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x64 );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            if (Position.Z <= 7)
            {
                GetMapDescription(writer, Position.X - 8, Position.Y - 6, Position.Z, 18, 14, 7, -7);
            }
            else
            {
                GetMapDescription(writer, Position.X - 8, Position.Y - 6, Position.Z, 18, 14, Position.Z - 2, 4);
            }
        }
    }
}