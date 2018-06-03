using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendMapNorth : SendMap
    {
        public SendMapNorth(IMap map, IClient client, Position fromPosition) : base(map, client)
        {
            this.FromPosition = fromPosition;
        }

        public Position FromPosition { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x65 );

            if (FromPosition.Z <= 7)
            {
                Write(writer, FromPosition.X - 8, FromPosition.Y - 7, FromPosition.Z, 18, 1, 7, -7);
            }
            else
            {
                Write(writer, FromPosition.X - 8, FromPosition.Y - 7, FromPosition.Z, 18, 1, FromPosition.Z - 2, 4);
            }
        }
    }
}