using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.IO;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class SendInfoOutgoingPacket : IOutgoingPacket
    {
        public SendInfoOutgoingPacket(uint creatureId, ushort serverBeat, double creatureSpeedA, double creatureSpeedB, double creatureSpeedC, bool canReportBugs)
        {
            this.CreatureId = creatureId;

            this.ServerBeat = serverBeat;

            this.CreatureSpeedA = creatureSpeedA;

            this.CreatureSpeedB = creatureSpeedB;

            this.CreatureSpeedC = creatureSpeedC;

            this.CanReportBugs = canReportBugs;
        }

        public uint CreatureId { get; set; }

        public ushort ServerBeat { get; set; }

        public double CreatureSpeedA { get; set; }

        public double CreatureSpeedB { get; set; }

        public double CreatureSpeedC { get; set; }

        public bool CanReportBugs { get; set; }
        
        public void Write(IByteArrayStreamWriter writer, IHasFeatureFlag features)
        {
            if ( !features.HasFeatureFlag(FeatureFlag.LoginPending) )
            {
                writer.Write( (byte)0x0A );
            }
            else
            {
                writer.Write( (byte)0x17 );
            }

            writer.Write(CreatureId);

            writer.Write(ServerBeat);

            if (features.HasFeatureFlag(FeatureFlag.NewSpeedLaw) )
            {
                writer.Write(CreatureSpeedA, 3); 

                writer.Write(CreatureSpeedB, 3); 

                writer.Write(CreatureSpeedC, 3); 
            }

            writer.Write(CanReportBugs);

            if (features.HasFeatureFlag(FeatureFlag.PVPFrame) )
            {
                writer.Write( (byte)0x00 ); //TODO: FeatureFlag.PVPFrame, can change pvp frame option
            }

            if (features.HasFeatureFlag(FeatureFlag.ExpertMode) )
            {
                writer.Write( (byte)0x00 ); //TODO: FeatureFlag.ExpertMode, expert mode button
            }

            if (features.HasFeatureFlag(FeatureFlag.IngameStore) )
            {
                writer.Write( (string)null); //TODO: FeatureFlag.IngameStore

                writer.Write( (ushort)25 );
            }
        }
    }
}