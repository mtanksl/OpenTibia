using OpenTibia.FileFormats.Xml.Quests;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game
{
    public class QuestCollection
    {
        private QuestFile questFile;

        public QuestCollection(QuestFile questFile)
        {
            this.questFile = questFile;
        }

        public Quest GetQuestById(ushort id)
        {
            return questFile.Quests.Where(q => q.Id == id).FirstOrDefault();
        }

        public IEnumerable<Quest> GetQuests()
        {
            return questFile.Quests;
        }
    }
}