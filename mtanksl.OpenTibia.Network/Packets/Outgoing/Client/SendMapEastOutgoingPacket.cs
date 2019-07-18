using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendMapEastOutgoingPacket : SendMapOutgoingPacket
    {
        public SendMapEastOutgoingPacket(IMap map, IClient client, Position fromPosition) : base(map, client)
        {
            this.FromPosition = fromPosition;
        }

        public Position FromPosition { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x66 );

            if (FromPosition.Z <= 7)
            {
                Write(writer, FromPosition.X + 10, FromPosition.Y - 6, FromPosition.Z, 1, 14, 7, -7);
            }
            else
            {
                Write(writer, FromPosition.X + 10, FromPosition.Y - 6, FromPosition.Z, 1, 14, FromPosition.Z - 2, 4);
            }
        }
    }
}