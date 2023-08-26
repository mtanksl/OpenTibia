using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenQuestLineDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenQuestLineDialogOutgoingPacket(ushort questId, List<MissionDto> missions)
        {
            this.QuestId = questId;

            this.missions = missions;
        }

        public ushort QuestId { get; set; }

        private List<MissionDto> missions;

        public List<MissionDto> Missions
        {
            get
            {
                return missions ?? ( missions = new List<MissionDto>() );
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