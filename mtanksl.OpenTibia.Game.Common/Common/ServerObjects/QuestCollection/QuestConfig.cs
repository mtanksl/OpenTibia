using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class QuestConfig
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        private List<MissionConfig> missions = new List<MissionConfig>();

        public List<MissionConfig> Missions
        {
            get
            {
                return missions;
            }
        }
    }
}