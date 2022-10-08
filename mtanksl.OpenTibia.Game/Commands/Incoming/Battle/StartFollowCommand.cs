using OpenTibia.Common.Objects;
using System;

namespace OpenTibia.Game.Commands
{
    public class StartFollowCommand : Command
    {
        public StartFollowCommand(Player player, uint creatureId, uint nonce)
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
                Creature creature = context.Server.GameObjects.GetGameObject<Creature>(CreatureId);

                if (creature != null && creature != Player)
                {
                    Player.AttackTarget = null;

                    Player.FollowTarget = creature;
                }

                resolve(context);
            } );
        }
    }
}