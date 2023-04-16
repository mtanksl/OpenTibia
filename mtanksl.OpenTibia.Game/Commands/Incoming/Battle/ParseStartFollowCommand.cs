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
            Creature creature = Context.Server.GameObjects.GetCreature(CreatureId);
                
            if (creature != null && creature != Player)
            {
                PlayerAttackAndFollowBehaviour playerAttackAndFollowThinkBehaviour = Context.Server.Components.GetComponent<PlayerAttackAndFollowBehaviour>(Player);

                if (playerAttackAndFollowThinkBehaviour != null)
                {
                    playerAttackAndFollowThinkBehaviour.Follow(creature);
                }

                return Promise.Completed;
            }

            return Promise.Break;
        }
    }
}