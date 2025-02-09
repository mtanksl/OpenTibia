using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System.Collections.Generic;
using System.Linq;

namespace OpenTibia.Game.Components
{
    public class MostDamageToAttackerTargetStrategy : ITargetStrategy
    {
        public static readonly MostDamageToAttackerTargetStrategy Instance = new MostDamageToAttackerTargetStrategy();

        private MostDamageToAttackerTargetStrategy()
        {
            
        }

        public Player GetTarget(int ticks, Creature attacker, Player[] players)
        {
            int maxDamage = 0;

            Player mostDamage = null;

            Dictionary<Creature, Hit> hits = Context.Current.Server.Combats.GetHitsByTarget(attacker);

            foreach (var hit in hits)
            {
                if (players.Contains(hit.Key) )
                {
                    if (hit.Value.Damage > maxDamage)
                    {
                        maxDamage = hit.Value.Damage;

                        mostDamage = (Player)hit.Key;
                    }
                }
            }

            return mostDamage;
        }
    }
}