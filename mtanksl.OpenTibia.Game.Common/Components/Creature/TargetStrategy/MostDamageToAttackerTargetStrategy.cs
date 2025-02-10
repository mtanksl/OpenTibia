using OpenTibia.Common.Objects;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using System.Collections.Generic;

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

            Dictionary<Creature, Hit> hits = Context.Current.Server.Combats.GetHitsByTarget(attacker); // Get everyone that attacked the attacker

            if (hits != null)
            {
                foreach (var player in players)
                {
                    Hit hit;

                    if (hits.TryGetValue(player, out hit) ) // Find the player in the list
                    {
                        if (hit.Damage > maxDamage)
                        {
                            maxDamage = hit.Damage;

                            mostDamage = player;
                        }
                    }
                }
            }

            return mostDamage;
        }
    }
}