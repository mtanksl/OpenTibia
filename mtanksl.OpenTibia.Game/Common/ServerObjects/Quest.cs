using System.Collections.Generic;

namespace OpenTibia.Game
{
    public class Quest
    {
        public ushort Id { get; set; }

        public string Name { get; set; }

        private List<Mission> missions = new List<Mission>();

        public List<Mission> Missions
        {
            get
            {
                return missions;
            }
        }
    }
}