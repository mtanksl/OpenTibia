using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;

namespace OpenTibia.Game.Commands
{
    public class ParseStartFollowCommand : Command
    {
        public ParseStartFollowCommand(Player player, uint creatureId, uint nonce)
        {
            Player = player;

            CreatureId = creatureId;

            Nonce = nonce;
        }

        public Player Player { get; set; }

        public uint CreatureId { get; set; }

        public uint Nonce { get; set; }

        public override Promise Execute()
        {
            Creature target = Context.Server.GameObjects.GetCreature(CreatureId);
                
            if (target != null && target != Player)
            {
                PlayerAttackAndWalkBehaviour playerAttackAndFollowBehaviour = Context.Server.GameObjectComponents.GetComponent<PlayerAttackAndWalkBehaviour>(Player);

                if (playerAttackAndFollowBehaviour != null)
                {
                    playerAttackAndFollowBehaviour.Follow(target);
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}