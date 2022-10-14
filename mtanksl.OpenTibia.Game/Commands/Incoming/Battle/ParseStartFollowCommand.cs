using OpenTibia.Common.Objects;
using OpenTibia.Game.Components;
using System;

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

        public override Promise Execute(Context context)
        {
            return Promise.Run(resolve =>
            {
                Creature creature = context.Server.GameObjects.GetCreature(CreatureId);
                
                if (creature != null && creature != Player)
                {
                    AttackAndFollowBehaviour component = Player.GetComponent<AttackAndFollowBehaviour>();

                    component.Follow(creature);
                }

                resolve(context);
            } );
        }
    }
}