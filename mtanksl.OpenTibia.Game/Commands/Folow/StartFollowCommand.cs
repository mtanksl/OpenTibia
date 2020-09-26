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
            Creature creature = context.Server.GameObjects.GetGameObject<Creature>(CreatureId);

            if (creature != null && creature != Player)
            {
                Player.AttackTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                Player.FollowTarget = creature;

                new FollowCommand(Player, Player.FollowTarget).Execute(context);

                base.OnCompleted(context);
            }
        }
    }
}