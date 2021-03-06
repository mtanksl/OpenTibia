﻿using OpenTibia.Common.Objects;

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

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Creature creature = server.Map.GetCreature(CreatureId);

            if (creature != null && creature != Player)
            {
                //Act

                Player.AttackTarget = null;

                server.CancelQueueForExecution(Constants.PlayerAttackSchedulerEvent(Player) );

                Player.FollowTarget = creature;

                new FollowCommand(Player, Player.FollowTarget).Execute(server, context);

                //Notify

                base.Execute(server, context);
            }
        }
    }
}