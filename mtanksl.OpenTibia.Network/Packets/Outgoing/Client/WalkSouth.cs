using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class WalkSouth : MapPacket
    {
        public WalkSouth(TibiaGameClient client, Position fromPosition) : base(client)
        {
            this.FromPosition = fromPosition;
        }

        public Position FromPosition { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x67 );

            if (FromPosition.Z <= 7)
            {
                MapDescription(writer, FromPosition.X - 8, FromPosition.Y + 8, FromPosition.Z, 18, 1, 7, -7);
            }
            else
            {
                MapDescription(writer, FromPosition.X - 8, FromPosition.Y + 8, FromPosition.Z, 18, 1, FromPosition.Z - 2, 4);
            }
        }
    }
}