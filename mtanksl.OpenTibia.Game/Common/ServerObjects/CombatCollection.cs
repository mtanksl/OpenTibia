using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
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

        private Dictionary<uint /* attacker */, HashSet<uint /* target */> > offenses = new Dictionary<uint, HashSet<uint> >();

        public bool ContainsOffense(uint attacker, uint target)
        {
            HashSet<uint> targets;

            if (offenses.TryGetValue(attacker, out targets) )
            {
                return targets.Contains(target);
            }

            return false;
        }

        public void AddOffense(uint attacker, uint target)
        {
            HashSet<uint> targets;

            if ( !offenses.TryGetValue(attacker, out targets) )
            {
                targets = new HashSet<uint>();

                offenses.Add(attacker, targets);
            }

            targets.Add(target);
        }

        private Dictionary<uint /* target */, HashSet<uint /* attacker */> > defenses = new Dictionary<uint, HashSet<uint> >();

        public bool ContainsDefense(uint target, uint attacker)
        {
            HashSet<uint> attackers;

            if (defenses.TryGetValue(target, out attackers) )
            {
                return attackers.Contains(attacker);
            }

            return false;
        }

        public void AddDefense(uint target, uint attacker)
        {
            HashSet<uint> attackers;

            if ( !defenses.TryGetValue(target, out attackers) )
            {
                attackers = new HashSet<uint>();

                defenses.Add(target, attackers);
            }

            attackers.Add(attacker);
        }

        private Dictionary<uint /* attacker */, HashSet<uint /* target */> > yellowSkullOffenses = new Dictionary<uint, HashSet<uint> >();

        private bool YellowSkullContainsOffense(uint attacker, uint target)
        {
            HashSet<uint> targets;

            if (yellowSkullOffenses.TryGetValue(attacker, out targets) )
            {
                return targets.Contains(target);
            }

            return false;
        }

        public void YellowSkullAddOffense(uint attacker, uint target)
        {
            HashSet<uint> targets;

            if ( !yellowSkullOffenses.TryGetValue(attacker, out targets) )
            {
                targets = new HashSet<uint>();

                yellowSkullOffenses.Add(attacker, targets);
            }

            targets.Add(target);
        }

        private Dictionary<uint /* target */, HashSet<uint /* attacker */> > yellowSkullDefenses = new Dictionary<uint, HashSet<uint> >();

        public bool YellowSkullContainsDefense(uint target, uint attacker)
        {
            HashSet<uint> attackers;

            if (yellowSkullDefenses.TryGetValue(target, out attackers) )
            {
                return attackers.Contains(attacker);
            }

            return false;
        }

        public void YellowSkullAddDefense(uint target, uint attacker)
        {
            HashSet<uint> attackers;

            if ( !yellowSkullDefenses.TryGetValue(target, out attackers) )
            {
                attackers = new HashSet<uint>();

                yellowSkullDefenses.Add(target, attackers);
            }

            attackers.Add(attacker);
        }

        private Dictionary<uint /* attacker */, SkullIcon> skulls = new Dictionary<uint, SkullIcon>();

        public bool SkullContains(uint attacker, out SkullIcon skullIcon)
        {
            return skulls.TryGetValue(attacker, out skullIcon);
        }

        public void SkullAdd(uint attacker, SkullIcon skullIcon)
        {
            if (skullIcon != SkullIcon.White && skullIcon != SkullIcon.Red && skullIcon != SkullIcon.Black)
            {
                throw new ArgumentException("SkullIcon must be White, Red or Black.");
            }

            skulls[attacker] = skullIcon;
        }

        private Dictionary<uint /* attacker */, List<UnjustifiedKill> > unjustifiedKills = new Dictionary<uint, List<UnjustifiedKill> >();

        public void AddUnjustifiedKill(uint attacker, uint target)
        {
            List<UnjustifiedKill> kills;

            if ( !unjustifiedKills.TryGetValue(attacker, out kills) )
            {
                kills = new List<UnjustifiedKill>();

                unjustifiedKills.Add(attacker, kills);
            }

            kills.Add(new UnjustifiedKill()
            {
                TargetId = target,

                LastAttack = DateTime.UtcNow
            } );
        }

        public void CleanUp(Player player)
        {
            HashSet<uint> attackers;

            HashSet<uint> targets;

            if (offenses.TryGetValue(player.Id, out targets) )
            {
                offenses.Remove(player.Id);

                foreach (var target in targets)
                {
                    if (defenses.TryGetValue(target, out attackers) )
                    {
                        attackers.Remove(player.Id);
                    }
                }

                if ( !unjustifiedKills.ContainsKey(player.Id) )
                {
                    skulls.Remove(player.Id);
                }
            }

            if (defenses.TryGetValue(player.Id, out attackers) )
            {
                defenses.Remove(player.Id);

                foreach (var attacker in attackers)
                {
                    if (offenses.TryGetValue(attacker, out targets) )
                    {
                        targets.Remove(player.Id);
                    }
                }
            }

            if (yellowSkullOffenses.TryGetValue(player.Id, out targets) )
            {
                yellowSkullOffenses.Remove(player.Id);

                foreach (var target in targets)
                {
                    if (yellowSkullDefenses.TryGetValue(target, out attackers) )
                    {
                        attackers.Remove(player.Id);
                    }
                }
            }

            if (yellowSkullDefenses.TryGetValue(player.Id, out attackers) )
            {
                yellowSkullDefenses.Remove(player.Id);

                foreach (var attacker in attackers)
                {
                    if (yellowSkullOffenses.TryGetValue(attacker, out targets) )
                    {
                        targets.Remove(player.Id);
                    }
                }
            }

            foreach (var observer in Context.Current.Server.Map.GetObserversOfTypePlayer(player.Tile.Position) )
            {
                byte clientIndex;

                if (observer.Client.TryGetIndex(player, out clientIndex) )
                {
                    Context.Current.AddPacket(observer, new SetSkullIconOutgoingPacket(player.Id, observer.Client.GetSkullIcon(player) ) );
                }
            }
        }
    }
}