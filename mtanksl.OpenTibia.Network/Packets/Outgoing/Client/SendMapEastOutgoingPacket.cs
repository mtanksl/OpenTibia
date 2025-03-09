using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendMapEastOutgoingPacket : SendMapOutgoingPacket
    {
        public SendMapEastOutgoingPacket(IMapGetTile map, IClient client, Position position) : base(map, client)
        {
            this.Position = position;
        }

        public Position Position { get; set; }

        public override void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x66 );

            if (Position.Z <= 7)
            {
                GetMapDescription(writer, features, Position.X + 9, Position.Y - 6, Position.Z, 1, 14, 7, -7);
            }
            else
            {
                GetMapDescription(writer, features, Position.X + 9, Position.Y - 6, Position.Z, 1, 14, Position.Z - 2, 4);
            }
        }
    }
}