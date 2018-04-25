using OpenTibia.IO;

namespace OpenTibia
{
    public class TileOutgoingPacket : MapPacket
    {
        public TileOutgoingPacket(TibiaGameClient client, Position position) : base(client)
        {
            this.Position = position;
        }

        public Position Position { get; set; }
        
        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x69 );

            writer.Write(Position.X);

            writer.Write(Position.Y);

            writer.Write(Position.Z);

            MapDescription(writer, Position.X, Position.Y, Position.Z, 1, 1, Position.Z, 0);
        }
    }
}