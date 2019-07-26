using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendMapDownOutgoingPacket : SendMapOutgoingPacket
    {
        private IMap map;

        private IClient client;
        
        public SendMapDownOutgoingPacket(IMap map, IClient client, Position position) : base(map, client)
        {
            this.map = map;

            this.client = client;

            this.Position = position;
        }

        public Position Position { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xBF );

            if (Position.Z == 7)
            {
                Write(writer, Position.X - 8, Position.Y - 6, Position.Z, 18, 14, 8, 2);
            }
            else if (Position.Z > 7)
            {
                Write(writer, Position.X - 8, Position.Y - 6, Position.Z, 18, 14, Position.Z + 3, 0);
            }
        }
    }
}