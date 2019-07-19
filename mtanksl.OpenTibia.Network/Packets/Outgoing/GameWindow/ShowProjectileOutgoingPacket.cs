using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class ShowProjectileOutgoingPacket : IOutgoingPacket
    {
        public ShowProjectileOutgoingPacket(Position fromPosition, Position toPosition, ProjectileType projectileType)
        {
            this.FromPosition = fromPosition;

            this.ToPosition = toPosition;

            this.ProjectileType = projectileType;
        }

        public Position FromPosition { get; set; }

        public Position ToPosition { get; set; }

        public ProjectileType ProjectileType { get; set; }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0x85 );

            writer.Write(FromPosition.X);

            writer.Write(FromPosition.Y);

            writer.Write(FromPosition.Z);

            writer.Write(ToPosition.X);

            writer.Write(ToPosition.Y);

            writer.Write(ToPosition.Z);
            
            writer.Write( (byte)ProjectileType );
        }
    }
}