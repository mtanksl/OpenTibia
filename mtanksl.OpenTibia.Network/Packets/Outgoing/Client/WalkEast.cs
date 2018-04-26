using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class WalkEast : MapPacket
    {
        public WalkEast(TibiaGameClient client, Position fromPosition) : base(client)
        {
            this.FromPosition = fromPosition;
        }

        public Position FromPosition { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x66 );

            if (FromPosition.Z <= 7)
            {
                MapDescription(writer, FromPosition.X + 10, FromPosition.Y - 6, FromPosition.Z, 1, 14, 7, -7);
            }
            else
            {
                MapDescription(writer, FromPosition.X + 10, FromPosition.Y - 6, FromPosition.Z, 1, 14, FromPosition.Z - 2, 4);
            }
        }
    }
}