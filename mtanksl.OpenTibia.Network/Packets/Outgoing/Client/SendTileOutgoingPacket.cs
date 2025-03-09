using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendTileOutgoingPacket : SendMapOutgoingPacket
    {
        public SendTileOutgoingPacket(IMapGetTile map, IClient client, Position position) : base(map, client)
        {
            this.Position = position;
        }

        public Position Position { get; set; }
        
        public override void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0x69 );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            GetMapDescription(writer, features, Position.X, Position.Y, Position.Z, 1, 1, Position.Z, 0);
        }
    }
}