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

        public Dictionary<Creature, Hit> GetHitsByTarget(Creature target) 
        {
            Dictionary<Creature, Hit> hits;

            hitss.TryGetValue(target, out hits);

            return hits;
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
    }
}