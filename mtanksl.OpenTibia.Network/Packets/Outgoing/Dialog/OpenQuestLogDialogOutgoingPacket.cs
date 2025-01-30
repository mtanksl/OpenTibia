using OpenTibia.IO;
using System.Collections.Generic;

namespace OpenTibia.Network.Packets.Outgoing
{
    public class OpenQuestLogDialogOutgoingPacket : IOutgoingPacket
    {
        public OpenQuestLogDialogOutgoingPacket(List<QuestDto> quests)
        {
            this.Quests = quests;
        }

        private List<QuestDto> quests;

        public List<QuestDto> Quests
        {
            get
            {
                return quests ?? ( quests = new List<QuestDto>() );
            }
            set
            {
                quests = value;
            }
        }
        
        public void Write(IByteArrayStreamWriter writer)
        {
            writer.Write( (byte)0xF0 );

            writer.Write( (ushort)Quests.Count );

            foreach (var quest in Quests)
            {
                writer.Write(quest.Id);

                writer.Write(quest.Name);

                writer.Write(quest.Completed);
            }
        }
    }
}