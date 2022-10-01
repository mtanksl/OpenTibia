using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenQuestLineDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenQuestLineDialogOutgoingPacket(ushort questId, List<Mission> missions)
        {
            this.QuestId = questId;

            this.missions = missions;
        }

        public ushort QuestId { get; set; }

        private List<Mission> missions;

        public List<Mission> Missions
        {
            get
            {
                return missions ?? ( missions = new List<Mission>() );
            }
            set
            {
                missions = value;
            }
        }
        
        public void Write(ByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xF1 );

            writer.Write(QuestId);

            writer.Write( (byte)Missions.Count );

            foreach (var mission in Missions)
            {
                writer.Write(mission.Name);

                writer.Write(mission.Description);
            }
        }
    }
}