using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using System.Collections.Generic;

namespace OpenTibia.Game.Common.ServerObjects
{
    public interface ICombatCollection
    {
        void AddHitToTarget(Creature attacker, Creature target, int damage);
        
        Dictionary<Creature, Hit> GetHitsByTargetAndRemove(Creature target);


        bool ContainsOffense(uint attacker, uint target);

        void AddOffense(uint attacker, uint target);

        bool ContainsDefense(uint target, uint attacker);

        void AddDefense(uint target, uint attacker);

        // bool YellowSkullContainsOffense(uint attacker, uint target);

        void YellowSkullAddOffense(uint attacker, uint target);

        bool YellowSkullContainsDefense(uint target, uint attacker);

        void YellowSkullAddDefense(uint target, uint attacker);

        bool SkullContains(uint attacker, out SkullIcon skullIcon);

        void SkullAdd(uint attacker, SkullIcon skullIcon);

        void CleanUp(Player player);
    }
}