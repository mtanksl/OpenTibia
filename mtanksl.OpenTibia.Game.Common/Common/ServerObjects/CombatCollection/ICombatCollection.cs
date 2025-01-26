using OpenTibia.Common.Objects;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ICombatCollection
    {
        void AddHitToTarget(Creature attacker, Creature target, int damage);
        
        Dictionary<Creature, Hit> GetHitsByTargetAndRemove(Creature target);


        public bool WhiteSkullContains(Player attacker);

        public bool WhiteSkullContains(Player attacker, Player target);

        public void WhiteSkullAdd(Player attacker, Player target);


        public bool YellowSkullContains(Player attacker, Player target);

        public void YellowSkullAdd(Player attacker, Player target);
    }
}