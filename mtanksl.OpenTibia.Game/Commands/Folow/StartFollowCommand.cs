using OpenTibia.Common.Objects;

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

        public override void Execute(Context context)
        {
            //Arrange

            Creature creature = context.Server.Map.GetCreature(CreatureId);

            if (creature != null && creature != Player)
            {
                //Act

                Player.AttackTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                Player.FollowTarget = creature;

                new FollowCommand(Player, Player.FollowTarget).Execute(context);

                //Notify

                base.Execute(context);
            }
        }
    }
}