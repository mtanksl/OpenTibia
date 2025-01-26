using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Collections.Generic;
using System.Linq;

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

        private Dictionary<Player /* attacker */, HashSet<Player /* target */> > yellowSkullOffenses = new Dictionary<Player, HashSet<Player> >();

        private bool YellowSkullContainsOffense(Player attacker, Player target)
        {
            HashSet<Player> targets;

            if (yellowSkullOffenses.TryGetValue(attacker, out targets) )
            {
                return targets.Contains(target);
            }

            return false;
        }

        public void YellowSkullAddOffense(Player attacker, Player target)
        {
            HashSet<Player> targets;

            if ( !yellowSkullOffenses.TryGetValue(attacker, out targets) )
            {
                targets = new HashSet<Player>();

                yellowSkullOffenses.Add(attacker, targets);
            }

            targets.Add(target);
        }

        private Dictionary<Player /* target */, HashSet<Player /* attacker */> > yellowSkullDefenses = new Dictionary<Player, HashSet<Player> >();

        public bool YellowSkullContainsDefense(Player target, Player attacker)
        {
            HashSet<Player> attackers;

            if (yellowSkullDefenses.TryGetValue(target, out attackers) )
            {
                return attackers.Contains(attacker);
            }

            return false;
        }

        public void YellowSkullAddDefense(Player target, Player attacker)
        {
            HashSet<Player> attackers;

            if ( !yellowSkullDefenses.TryGetValue(target, out attackers) )
            {
                attackers = new HashSet<Player>();

                yellowSkullDefenses.Add(target, attackers);
            }

            attackers.Add(attacker);
        }

        private Dictionary<Player /* attacker */, SkullIcon> skulls = new Dictionary<Player, SkullIcon>();

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

        private Dictionary<Player /* attacker */, List<UnjustifiedKill> > unjustifiedKills = new Dictionary<Player, List<UnjustifiedKill> >();

        public void AddUnjustifiedKill(Player attacker, Player target)
        {
            List<UnjustifiedKill> kills;

            if ( !unjustifiedKills.TryGetValue(attacker, out kills) )
            {
                kills = new List<UnjustifiedKill>();

                unjustifiedKills.Add(attacker, kills);
            }

            kills.Add(new UnjustifiedKill()
            {
                TargetId = target.Id,

                LastAttack = DateTime.UtcNow
            } );
        }

        public void CleanUp(Player player)
        {
            HashSet<Player> attackers;

            HashSet<Player> targets;

            if (offenses.TryGetValue(player, out targets) )
            {
                offenses.Remove(player);

                foreach (var target in targets)
                {
                    if (defenses.TryGetValue(target, out attackers) )
                    {
                        attackers.Remove(player);
                    }
                }

                if ( !unjustifiedKills.ContainsKey(player) )
                {
                    skulls.Remove(player);
                }
            }

            if (defenses.TryGetValue(player, out attackers) )
            {
                defenses.Remove(player);

                foreach (var attacker in attackers)
                {
                    if (offenses.TryGetValue(attacker, out targets) )
                    {
                        targets.Remove(player);
                    }
                }
            }

            if (yellowSkullOffenses.TryGetValue(player, out targets) )
            {
                yellowSkullOffenses.Remove(player);

                foreach (var target in targets)
                {
                    if (yellowSkullDefenses.TryGetValue(target, out attackers) )
                    {
                        attackers.Remove(player);
                    }
                }
            }

            if (yellowSkullDefenses.TryGetValue(player, out attackers) )
            {
                yellowSkullDefenses.Remove(player);

                foreach (var attacker in attackers)
                {
                    if (yellowSkullOffenses.TryGetValue(attacker, out targets) )
                    {
                        targets.Remove(player);
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