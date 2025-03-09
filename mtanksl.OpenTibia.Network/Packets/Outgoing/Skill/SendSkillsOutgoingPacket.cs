using OpenTibia.Common.Objects;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendSkillsOutgoingPacket : IOutgoingPacket
    {
        public SendSkillsOutgoingPacket(byte fist, byte fistPercent, byte club, byte clubPercent, byte sword, byte swordPercent, byte axe, byte axePercent, byte distance, byte distancePercent, byte shield, byte shieldPercent, byte fish, byte fishPercent)
        {
            this.Fist = fist;
            
            this.FistPercent = fistPercent;
            
            this.Club = club;
            
            this.ClubPercent = clubPercent;
            
            this.Sword = sword;
            
            this.SwordPercent = swordPercent;
            
            this.Axe = axe;
            
            this.AxePercent = axePercent;
            
            this.Distance = distance;
            
            this.DistancePercent = distancePercent;
            
            this.Shield = shield;
            
            this.ShieldPercent  = shieldPercent;
            
            this.Fish = fish;

            this.FishPercent = fishPercent;
        }

        public byte Fist { get; set; }

        public byte FistPercent { get; set; }

        public byte Club { get; set; }

        public byte ClubPercent { get; set; }

        public byte Sword { get; set; }

        public byte SwordPercent { get; set; }

        public byte Axe { get; set; }

        public byte AxePercent { get; set; }

        public byte Distance { get; set; }

        public byte DistancePercent { get; set; }

        public byte Shield { get; set; }

        public byte ShieldPercent { get; set; }

        public byte Fish { get; set; }

        public byte FishPercent { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            writer.Write( (byte)0xA1 );

            writer.Write(Fist);

            writer.Write(FistPercent);

            writer.Write(Club);

            writer.Write(ClubPercent);

            writer.Write(Sword);

            writer.Write(SwordPercent);

            writer.Write(Axe);

            writer.Write(AxePercent);

            writer.Write(Distance);

            writer.Write(DistancePercent);

            writer.Write(Shield);

            writer.Write(ShieldPercent);

            writer.Write(Fish);

            writer.Write(FishPercent);
        }
    }
}