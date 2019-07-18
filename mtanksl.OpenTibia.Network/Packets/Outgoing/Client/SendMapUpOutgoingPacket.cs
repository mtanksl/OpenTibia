using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendMapUpOutgoingPacket : SendMapOutgoingPacket
    {
        private IMap map;

        private IClient client;

        public SendMapUpOutgoingPacket(IMap map, IClient client, Position fromPosition) : base(map, client)
        {
            this.map = map;

            this.client = client;

            this.FromPosition = fromPosition;
        }

        public Position FromPosition { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xBE );

            if (FromPosition.Z == 8)
            {
                Write(writer, FromPosition.X - 8, FromPosition.Y - 6, FromPosition.Z, 18, 14, 5, -5);
            }
            else if (FromPosition.Z > 8)
            {
                Write(writer, FromPosition.X - 8, FromPosition.Y - 6, FromPosition.Z, 18, 14, FromPosition.Z - 3, 0);
            }

            new SendMapWestOutgoingPacket(map, client, FromPosition.Offset(0, 0, -1) ).Write(writer);

            new SendMapNorthOutgoingPacket(map, client, FromPosition.Offset(0, 0, -1) ).Write(writer);
        }
    }
}