using System.Collections.Generic;
using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Quests
{
    public class Quest
    {
        public static Quest Load(XElement questNode)
        {
            Quest quest = new Quest();

            quest.Id = (ushort)(uint)questNode.Attribute("id");

            quest.Name = (string)questNode.Attribute("name");

            quest.missions = new List<Mission>();

            foreach (var missionNode in questNode.Elements("mission") )
            {
                quest.missions.Add( Mission.Load(missionNode) );
            }

            return quest;
        }

        public ushort Id { get; set; }

        public string Name { get; set; }

        private List<Mission> missions;

        public List<Mission> Missions
        {
            get
            {
                return missions;
            }
        }
    }
}