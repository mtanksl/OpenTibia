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

        public override void Execute(Server server, Context context)
        {
            //Arrange

            Creature creature = server.Map.GetCreature(CreatureId);

            if (creature != null && creature != Player)
            {
                if (creature is Npc)
                {
                    //Act

                    Player.AttackTarget = null;

                    server.CancelQueueForExecution(Constants.PlayerAttackSchedulerEvent(Player) );

                    Player.FollowTarget = null;

                    server.CancelQueueForExecution(Constants.PlayerActionSchedulerEvent(Player) );

                    //Notify

                    context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.YouMayNotAttackThisCreature),

                                                            new StopAttackAndFollowOutgoingPacket(Nonce) );                   
                }
                else
                {
                    //Act

                    Player.AttackTarget = creature;

                    new AttackCommand(Player, Player.AttackTarget).Execute(server, context);

                    if (Player.Client.ChaseMode == ChaseMode.StandWhileFighting)
                    {
                        Player.FollowTarget = null;

                        server.CancelQueueForExecution(Constants.PlayerActionSchedulerEvent(Player) );
                    }
                    else
                    {
                        Player.FollowTarget = creature;

                        new FollowCommand(Player, Player.FollowTarget).Execute(server, context);
                    }

                    //Notify

                    base.Execute(server, context);
                }
            }
        }
    }
}