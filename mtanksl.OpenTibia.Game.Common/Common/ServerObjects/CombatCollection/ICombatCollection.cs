using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ICombatCollection
    {
        void AddHitToTarget(Creature attacker, Creature target, int damage);
        
        Dictionary<Creature, Hit> GetHitsByTarget(Creature target);

        Dictionary<Creature, Hit> GetHitsByTargetAndRemove(Creature target);
    }
}