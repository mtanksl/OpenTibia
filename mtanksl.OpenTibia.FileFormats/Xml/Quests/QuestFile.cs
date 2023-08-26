using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Quests
{
    public class QuestFile
    {
        public static QuestFile Load(string path)
        {
            QuestFile file = new QuestFile();

            file.quests = new List<Quest>();

            foreach (var questNode in XElement.Load(path).Elements("quest") )
            {
                file.quests.Add( Quest.Load(questNode) );
            }

            return file;
        }

        private List<Quest> quests;

        public List<Quest> Quests
        {
            get
            {
                return quests;
            }
        }
    }
}