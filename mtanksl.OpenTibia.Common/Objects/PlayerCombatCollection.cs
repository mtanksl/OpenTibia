using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Common.Objects
{
    public class PlayerCombatCollection
    {
        public class UnjustifiedKill
        {
            public int TargetId { get; set; }

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

        private List<UnjustifiedKill> unjustifiedKills = new List<UnjustifiedKill>();

        public int CountUnjustifiedKills
        {
            get
            {
                return unjustifiedKills.Count;
            }
        }

        private SkullIcon? skullIcon;

        public void AddUnjustifiedKill(int databasePlayerId, DateTime creationDate)
        {
            unjustifiedKills.Add(new UnjustifiedKill()
            {
                TargetId = databasePlayerId,

                CreationDate = creationDate
            } );

            skullIcon = null;
        }

        public IEnumerable<UnjustifiedKill> GetUnjustifiedKills()
        {
            return unjustifiedKills;
        }

        private SkullIcon GetUnjustifiedKillsSkullIcon()
        {
            SkullIcon GetSkullIcon()
            {
                DateTime now = DateTime.UtcNow;

                int count = unjustifiedKills.Where(k => k.CreationDate > now.AddDays(-1) ).Count();

                if (count >= 6)
                {
                    return SkullIcon.Black;
                }

                count = unjustifiedKills.Where(k => k.CreationDate > now.AddDays(-7) ).Count();

                if (count >= 10)
                {
                    return SkullIcon.Black;
                }

                count = unjustifiedKills.Where(k => k.CreationDate > now.AddDays(-30) ).Count();

                if (count >= 20)
                {
                    return SkullIcon.Black;
                }

                count = unjustifiedKills.Where(k => k.CreationDate > now.AddDays(-1) ).Count();

                if (count >= 3)
                {
                    return SkullIcon.Red;
                }

                count = unjustifiedKills.Where(k => k.CreationDate > now.AddDays(-7) ).Count();

                if (count >= 5)
                {
                    return SkullIcon.Red;
                }

                count = unjustifiedKills.Where(k => k.CreationDate > now.AddDays(-30) ).Count();

                if (count >= 10)
                {
                    return SkullIcon.Red;
                }

                count = unjustifiedKills.Where(k => k.CreationDate > now.AddMinutes(-15) ).Count();

                if (count >= 1)
                {
                    return SkullIcon.White;
                }

                return SkullIcon.None;
            }

            if (skullIcon == null)
            {
                skullIcon = GetSkullIcon();
            }

            return skullIcon.Value;
        }

        private HashSet<Player> attackedBy = new HashSet<Player>();

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

                    target.Combat.attackedBy.Add(this.player);
                }
                else
                {
                    attackedPlayerKiller.Add(target);

                    target.Combat.attackedBy.Add(this.player);
                }

                return true;
            }

            return false;
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

        public void Clear()
        {
            foreach (var attacker in attackedBy)
            {
                attacker.Combat.attackedInnocent.Remove(this.player);

                attacker.Combat.attackedPlayerKiller.Remove(this.player);
            }

            attackedInnocent.Clear();

            attackedPlayerKiller.Clear();
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