using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ICombatCollection
    {
        void AddHitToTarget(Creature attacker, Creature target, int damage);
        
        Dictionary<Creature, Hit> GetHitsByTargetAndRemove(Creature target);


        bool ContainsOffense(Player attacker, Player target);

        void AddOffense(Player attacker, Player target);

        bool ContainsDefense(Player target, Player attacker);

        void AddDefense(Player target, Player attacker);

        // bool YellowSkullContainsOffense(Player attacker, Player target);

        void YellowSkullAddOffense(Player attacker, Player target);

        bool YellowSkullContainsDefense(Player target, Player attacker);

        void YellowSkullAddDefense(Player target, Player attacker);

        bool SkullContains(Player attacker, out SkullIcon skullIcon);

        void SkullAdd(Player attacker, SkullIcon skullIcon);

        void AddUnjustifiedKill(Player attacker, Player target);

        void CleanUp(Player player);
    }
}