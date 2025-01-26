using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public class CombatCollection : ICombatCollection
    {
        private Dictionary<Creature /* target */, Dictionary<Creature /* attacker */, Hit> > hitss = new Dictionary<Creature, Dictionary<Creature, Hit> >();

        public void AddHitToTarget(Creature attacker, Creature target, int damage)
        {
            Dictionary<Creature, Hit> hits;

            if ( !hitss.TryGetValue(target, out hits) )
            {
                hits = new Dictionary<Creature, Hit>();

                hitss.Add(target, hits);
            }

            Hit hit;

            if ( !hits.TryGetValue(attacker, out hit) )
            {
                hit = new Hit();

                hits.Add(attacker, hit);
            }

            hit.Damage += damage;

            hit.LastAttack = DateTime.UtcNow;
        }

        public Dictionary<Creature, Hit> GetHitsByTargetAndRemove(Creature target)
        {
            Dictionary<Creature, Hit> hits;

            if (hitss.TryGetValue(target, out hits) )
            {
                hitss.Remove(target);
            }

            return hits;
        }

        private Dictionary<Player /* attacker */, HashSet<Player /* target */> > offenses = new Dictionary<Player, HashSet<Player> >();

        public bool ContainsOffense(Player attacker, Player target)
        {
            HashSet<Player> targets;

            if (offenses.TryGetValue(attacker, out targets) )
            {
                return targets.Contains(target);
            }

            return false;
        }

        public void AddOffense(Player attacker, Player target)
        {
            HashSet<Player> targets;

            if ( !offenses.TryGetValue(attacker, out targets) )
            {
                targets = new HashSet<Player>();

                offenses.Add(attacker, targets);
            }

            targets.Add(target);
        }

        private Dictionary<Player /* target */, HashSet<Player /* attacker */> > defenses = new Dictionary<Player, HashSet<Player> >();

        public bool ContainsDefense(Player target, Player attacker)
        {
            HashSet<Player> attackers;

            if (defenses.TryGetValue(target, out attackers) )
            {
                return attackers.Contains(attacker);
            }

            return false;
        }

        public void AddDefense(Player target, Player attacker)
        {
            HashSet<Player> attackers;

            if ( !defenses.TryGetValue(target, out attackers) )
            {
                attackers = new HashSet<Player>();

                defenses.Add(target, attackers);
            }

            attackers.Add(attacker);
        }

        private Dictionary<Player, SkullIcon> skulls = new Dictionary<Player, SkullIcon>();

        public bool SkullContains(Player attacker, out SkullIcon skullIcon)
        {
            return skulls.TryGetValue(attacker, out skullIcon);
        }

        public void SkullAdd(Player attacker, SkullIcon skullIcon)
        {
            if (skullIcon != SkullIcon.White && skullIcon != SkullIcon.Red && skullIcon != SkullIcon.Black)
            {
                throw new ArgumentException("SkullIcon must be White, Red or Black.");
            }

            skulls[attacker] = skullIcon;
        }

        private Dictionary<Player /* target */, HashSet<Player /* attacker */> > yellowSkulls = new Dictionary<Player, HashSet<Player> >();

        public bool YellowSkullContains(Player target, Player attacker)
        {
            HashSet<Player> attackers;

            if (yellowSkulls.TryGetValue(target, out attackers) )
            {
                return attackers.Contains(attacker);
            }

            return false;
        }

        public void YellowSkullAdd(Player target, Player attacker)
        {
            HashSet<Player> attackers;

            if ( !yellowSkulls.TryGetValue(target, out attackers) )
            {
                attackers = new HashSet<Player>();

                yellowSkulls.Add(target, attackers);
            }

            attackers.Add(attacker);
        }
    }
}