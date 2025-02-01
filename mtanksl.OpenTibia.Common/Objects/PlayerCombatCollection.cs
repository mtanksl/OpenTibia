using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class PlayerCombatCollection
    {
        public class kill
        {
            public int Id { get; set; }

            public int TargetId { get; set; }

            public bool Unjustified { get; set; }

            public DateTime CreationDate { get; set; }
        }

        public class Death
        {
            public int Id { get; set; }

            public int? AttackerId { get; set; }

            public string Name { get; set; }

            public int Level { get; set; }

            public bool Unjustified { get; set; }

            public DateTime CreationDate { get; set; }
        }

        public PlayerCombatCollection(Player player)
        {
            this.player = player;
        }

        private Player player;

        public Player Player
        {
            get
            {
                return player;
            }
        }

        private List<kill> kills = new List<kill>();

        public int CountKills
        {
            get
            {
                return kills.Count;
            }
        }

        public void AddKill(int id, int databasePlayerId, bool unjustified, DateTime creationDate)
        {
            kills.Add(new kill()
            {
                Id = id,

                TargetId = databasePlayerId,

                Unjustified  = unjustified,

                CreationDate = creationDate
            } );

            if (unjustified)
            {
                unjustifiedKillsSkullIcon = null;
            }
        }

        public IEnumerable<kill> GetKills()
        {
            return kills;
        }

        private List<Death> deaths = new List<Death>();

        public int CountDeaths
        {
            get
            {
                return deaths.Count;
            }
        }

        public void AddDeath(int id, int? databasePlayerId, string name, int level, bool unjustified, DateTime creationDate)
        {
            deaths.Add(new Death()
            {
                Id = id,

                AttackerId = databasePlayerId,

                Name = name,

                Level = level,

                Unjustified = unjustified,

                CreationDate = creationDate
            } );
        }

        public IEnumerable<Death> GetDeaths()
        {
            return deaths;
        }

        private HashSet<Player> attackedInnocent = new HashSet<Player>();

        private HashSet<Player> attackedPlayerKiller = new HashSet<Player>();

        public bool Attacked(Player target)
        {
            return attackedInnocent.Contains(target) || attackedPlayerKiller.Contains(target);
        }

        public bool CanAttack(Player target)
        {
            if ( !attackedInnocent.Contains(target) && !attackedPlayerKiller.Contains(target) )
            {
                if (target.Combat.GetSkullIcon(null) == SkullIcon.None)
                {
                    attackedInnocent.Add(target);
                }
                else
                {
                    attackedPlayerKiller.Add(target);
                }

                return true;
            }

            return false;
        }
          
        public void Clear()
        {
            attackedInnocent.Clear();

            attackedPlayerKiller.Clear();
        }

        private SkullIcon? unjustifiedKillsSkullIcon;

        private SkullIcon GetUnjustifiedKillsSkullIcon()
        {
            SkullIcon GetSkullIcon()
            {
                DateTime now = DateTime.UtcNow;

                if (kills.Count >= 6)
                {
                    // A black skull will last for 45 days

                    for (int i = 0; i < 45; i++)
                    {
                        // Six or more unmarked characters in one day (24 hours)

                        int count = kills.Where(k => k.Unjustified && k.CreationDate > now.AddDays(-i - 1) ).Count();

                        if (count >= 6)
                        {
                            return SkullIcon.Black;
                        }

                        // Ten or more unmarked characters in one week (7 days)

                        count = kills.Where(k => k.Unjustified && k.CreationDate > now.AddDays(-i - 7) ).Count();

                        if (count >= 10)
                        {
                            return SkullIcon.Black;
                        }

                        // Twenty or more unmarked characters in one month (30 days)

                        count = kills.Where(k => k.Unjustified && k.CreationDate > now.AddDays(-i - 30) ).Count();

                        if (count >= 20)
                        {
                            return SkullIcon.Black;
                        }
                    }
                }

                if (kills.Count >= 3)
                {
                    // A red skull will last for 30 days

                    for (int i = 0; i < 30; i++)
                    {
                        // Three or more unmarked characters in one day (24 hours)

                        int count = kills.Where(k => k.Unjustified && k.CreationDate > now.AddDays(-i - 1) ).Count();

                        if (count >= 3)
                        {
                            return SkullIcon.Red;
                        }

                        // Five or more unmarked characters in one week (7 days)
                        
                        count = kills.Where(k => k.Unjustified && k.CreationDate > now.AddDays(-i - 7) ).Count();

                        if (count >= 5)
                        {
                            return SkullIcon.Red;
                        }

                        // Ten or more unmarked characters in one month (30 days)

                        count = kills.Where(k => k.Unjustified && k.CreationDate > now.AddDays(-i - 30) ).Count();

                        if (count >= 10)
                        {
                            return SkullIcon.Red;
                        }
                    }
                }

                if (kills.Count >= 1)
                {
                    // A white skull will last for 15 minutes

                    int count = kills.Where(k => k.Unjustified && k.CreationDate > now.AddMinutes(-15) ).Count();

                    if (count >= 1)
                    {
                        return SkullIcon.White;
                    }
                }

                return SkullIcon.None;
            }

            if (unjustifiedKillsSkullIcon == null)
            {
                unjustifiedKillsSkullIcon = GetSkullIcon();
            }

            return unjustifiedKillsSkullIcon.Value;
        }

        private SkullIcon GetOffenseSkullIcon(Player target)
        {
            if (attackedInnocent.Count > 0)
            {
                return SkullIcon.White;
            }

            if (target != null)
            {
                if (attackedPlayerKiller.Contains(target) )
                {
                    return SkullIcon.Yellow;
                }
            }

            return SkullIcon.None;
        }

        public SkullIcon GetSkullIcon(Player target)
        {
            SkullIcon skullIcon = GetUnjustifiedKillsSkullIcon();

            if (skullIcon != SkullIcon.None)
            {
                return skullIcon;
            }

            skullIcon = GetOffenseSkullIcon(target);

            if (skullIcon != SkullIcon.None)
            {
                return skullIcon;
            }

            return SkullIcon.None;
        }
    }
}