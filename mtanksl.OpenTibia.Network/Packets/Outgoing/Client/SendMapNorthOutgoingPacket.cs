using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendMapNorthOutgoingPacket : SendMapOutgoingPacket
    {
        public SendMapNorthOutgoingPacket(IMapGetTile map, IClient client, Position position) : base(map, client)
        {
            this.Position = position;
        }

        public Position Position { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x65 );

            if (Position.Z <= 7)
            {
                GetMapDescription(writer, Position.X - 8, Position.Y - 6, Position.Z, 18, 1, 7, -7);
            }
            else
            {
                GetMapDescription(writer, Position.X - 8, Position.Y - 6, Position.Z, 18, 1, Position.Z - 2, 4);
            }
        }
    }
}