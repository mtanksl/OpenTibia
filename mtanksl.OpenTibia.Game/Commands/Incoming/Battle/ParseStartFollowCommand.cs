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
            return Promise.Run( (resolve, reject) =>
            {
                Creature creature = Context.Server.GameObjects.GetCreature(CreatureId);
                
                if (creature != null && creature != Player)
                {
                    AttackAndFollowBehaviour component = Context.Server.Components.GetComponent<AttackAndFollowBehaviour>(Player);

                    component.Follow(creature);
                }

                resolve();
            } );
        }
    }
}