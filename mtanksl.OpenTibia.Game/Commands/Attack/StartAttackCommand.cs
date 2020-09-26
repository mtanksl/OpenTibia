using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class StartAttackCommand : Command
    {
        public StartAttackCommand(Player player, uint creatureId, uint nonce)
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
                if (creature is Npc)
                {
                    //Act

                    Player.AttackTarget = null;

                    context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                    Player.FollowTarget = null;

                    context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                    //Notify

                    context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackThisCreature),

                                                            new StopAttackAndFollowOutgoingPacket(Nonce) );                   
                }
                else
                {
                    //Act

                    Player.AttackTarget = creature;

                    new AttackCommand(Player, Player.AttackTarget).Execute(context);

                    if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                    {
                        Player.FollowTarget = null;

                        context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );
                    }
                    else
                    {
                        Player.FollowTarget = creature;

                        new FollowCommand(Player, Player.FollowTarget).Execute(context);
                    }

                    //Notify

                    base.Execute(context);
                }
            }
        }
    }
}