using System.Xml.Linq;

namespace OpenTibia.FileFormats.Xml.Quests
{
    public class Mission
    {
        public static Mission Load(XElement missionNode)
        {
            Mission mission = new Mission();

            mission.Name = (string)missionNode.Attribute("name");

            mission.Description = (string)missionNode.Attribute("description");

            mission.StorageKey = (int)missionNode.Attribute("storagekey");

            mission.StorageValue = (int)missionNode.Attribute("storagevalue");

            return mission;
        }

        public string Name { get; set; }

        public string Description { get; set; }

        public int StorageKey { get; set; }

        public int StorageValue { get; set; }
    }
}