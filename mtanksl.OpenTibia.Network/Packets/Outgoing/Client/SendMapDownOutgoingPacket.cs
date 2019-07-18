using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendMapDownOutgoingPacket : SendMapOutgoingPacket
    {
        private IMap map;

        private IClient client;
        
        public SendMapDownOutgoingPacket(IMap map, IClient client, Position fromPosition) : base(map, client)
        {
            this.map = map;

            this.client = client;

            this.FromPosition = fromPosition;
        }

        public Position FromPosition { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xBF );

            if (FromPosition.Z == 7)
            {
                Write(writer, FromPosition.X - 8, FromPosition.Y - 6, FromPosition.Z, 18, 14, 8, 2);
            }
            else if (FromPosition.Z > 7)
            {
                Write(writer, FromPosition.X - 8, FromPosition.Y - 6, FromPosition.Z, 18, 14, FromPosition.Z + 3, 0);
            }

            new SendMapEastOutgoingPacket(map, client, FromPosition.Offset(0, 0, 1) ).Write(writer);

            new SendMapSouthOutgoingPacket(map, client, FromPosition.Offset(0, 0, 1) ).Write(writer);
        }
    }
}