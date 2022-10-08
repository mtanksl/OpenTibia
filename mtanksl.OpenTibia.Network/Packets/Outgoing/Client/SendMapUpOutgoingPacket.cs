using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendMapUpOutgoingPacket : SendMapOutgoingPacket
    {
        private IMap map;

        private IClient client;

        public SendMapUpOutgoingPacket(IMap map, IClient client, Position position) : base(map, client)
        {
            this.map = map;

            this.client = client;

            this.Position = position;
        }

        public Position Position { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xBE );

            if (Position.Z == 8)
            {
                GetMapDescription(writer, Position.X - 8, Position.Y - 6, Position.Z, 18, 14, 5, -5);
            }
            else if (Position.Z > 8)
            {
                GetMapDescription(writer, Position.X - 8, Position.Y - 6, Position.Z, 18, 14, Position.Z - 3, 0);
            }
        }
    }
}