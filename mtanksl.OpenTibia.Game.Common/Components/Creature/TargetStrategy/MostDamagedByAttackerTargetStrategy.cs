using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System.Collections.Generic;

namespace OpenTibia.Game.Components
{
    public class MostDamagedByAttackerTargetStrategy : ITargetStrategy
    {
        public static readonly MostDamagedByAttackerTargetStrategy Instance = new MostDamagedByAttackerTargetStrategy();

        private MostDamagedByAttackerTargetStrategy()
        {
            
        }

        public Player GetTarget(int ticks, Creature attacker, Player[] players)
        {
            int maxDamage = 0;

            Player mostDamaged = null;

            foreach (var player in players)
            {
                Dictionary<Creature, Hit> hits = Context.Current.Server.Combats.GetHitsByTarget(player);

                if (hits != null)
                {
                    Hit hit;

                    if (hits.TryGetValue(attacker, out hit) )
                    {
                        if (hit.Damage > maxDamage)
                        {
                            maxDamage = hit.Damage;

                            mostDamaged = player;
                        }
                    }
                }
            }

            return mostDamaged;
        }
    }
}