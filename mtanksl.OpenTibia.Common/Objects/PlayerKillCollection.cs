using System;
using System.Collections.Generic;

namespace OpenTibia.Common.Objects
{
    public class PlayerKillCollection
    {
        public class UnjustifiedKill
        {
            public int TargetId { get; set; }

            public DateTime CreationDate { get; set; }
        }

        private List<UnjustifiedKill> unjustifiedKills = new List<UnjustifiedKill>();

        public int Count
        {
            get
            {
                return unjustifiedKills.Count;
            }
        }

        public void AddUnjustifiedKill(int databasePlayerId, DateTime creationDate)
        {
            unjustifiedKills.Add(new UnjustifiedKill()
            {
                TargetId = databasePlayerId,

                CreationDate = creationDate
            } );
        }

        public IEnumerable<UnjustifiedKill> GetKills()
        {
            return unjustifiedKills;
        }
    }
}