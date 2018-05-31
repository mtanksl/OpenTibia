using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendMapWest : SendMap
    {
        public SendMapWest(IMap map, IClient client, Position fromPosition) : base(map, client)
        {
            this.FromPosition = fromPosition;
        }

        public Position FromPosition { get; set; }

        public override void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x68 );

            if (FromPosition.Z <= 7)
            {
                MapDescription(writer, FromPosition.X - 9, FromPosition.Y - 6, FromPosition.Z, 1, 14, 7, -7);
            }
            else
            {
                MapDescription(writer, FromPosition.X - 9, FromPosition.Y - 6, FromPosition.Z, 1, 14, FromPosition.Z - 2, 4);
            }
        }
    }
}