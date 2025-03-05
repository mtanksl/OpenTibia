using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Components;
using System.Collections.Generic;

namespace OpenTibia.Game.Commands
{
    public class ChallengeAttack : Attack
    {
        public override (int Damage, BlockType BlockType, HashSet<Item> RemoveCharges) Calculate(Creature attacker, Creature target)
        {
            return (0, BlockType.None, null);
        }

        public override Promise NoDamage(Creature attacker, Creature target, BlockType blockType)
        {
            if (target is Monster monster)
            {
                MonsterThinkBehaviour monsterThinkBehaviour = Context.Current.Server.GameObjectComponents.GetComponent<MonsterThinkBehaviour>(monster);

                if (monsterThinkBehaviour != null)
                {
                    monsterThinkBehaviour.Target = attacker;
                }
            }

            return Promise.Completed;
        }

        public override Promise Damage(Creature attacker, Creature target, int damage)
        {
            return Promise.Completed;
        }
    }
}