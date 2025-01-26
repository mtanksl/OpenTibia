using OpenTibia.Common.Objects;
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

        private Dictionary<Player /* attacker */, HashSet<Player /* target */> > whiteSkulls = new Dictionary<Player, HashSet<Player> >();

        public bool WhiteSkullContains(Player attacker)
        {
            return whiteSkulls.ContainsKey(attacker);
        }

        public bool WhiteSkullContains(Player attacker, Player target)
        {
            HashSet<Player> targets;

            if (whiteSkulls.TryGetValue(attacker, out targets) )
            {
                return targets.Contains(target);
            }

            return false;
        }

        public void WhiteSkullAdd(Player attacker, Player target)
        {
            HashSet<Player> targets;

            if ( !whiteSkulls.TryGetValue(attacker, out targets) )
            {
                targets = new HashSet<Player>();

                whiteSkulls.Add(attacker, targets);
            }

            targets.Add(target);
        }

        private Dictionary<Player /* target */, HashSet<Player /* attacker */> > yellowSkulls = new Dictionary<Player, HashSet<Player> >();

        public bool YellowSkullContains(Player attacker, Player target)
        {
            HashSet<Player> attackers;

            if ( yellowSkulls.TryGetValue(target, out attackers) )
            {
                return attackers.Contains(attacker);
            }

            return false;
        }

        public void YellowSkullAdd(Player attacker, Player target)
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