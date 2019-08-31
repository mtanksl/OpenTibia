using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Network.Packets.Outgoing;

namespace OpenTibia.Game.Commands
{
    public class AttackCommand : Command
    {
        public AttackCommand(Player player, Creature target)
        {
            Player = player;

            Target = target;
        }

        public Player Player { get; set; }

        public Creature Target { get; set; }

        public override void Execute(Server server, Context context)
        {
            //Arrange

            if ( !Player.Tile.Position.IsInPlayerRange(Target.Tile.Position) )
            {
                //Act

                Player.AttackTarget = null;

                server.CancelQueueForExecution(Constants.PlayerAttackSchedulerEvent(Player) );

                Player.FollowTarget = null;

                server.CancelQueueForExecution(Constants.PlayerActionSchedulerEvent(Player) );

                //Notify

                context.Write(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),
                        
                                                        new StopAttackAndFollowOutgoingPacket(0) );
            }
            else
            {
                if ( Player.Tile.Position.IsNextTo(Target.Tile.Position) )
                {
                    //Act

                    //Notify

                    context.Write(Player.Client.Connection, new ShowMagicEffectOutgoingPacket(Target.Tile.Position, MagicEffectType.Puff) );

                    if (Target is Player observer)
                    {
                        context.Write(observer.Client.Connection, new SetFrameColorOutgoingPacket(Player.Id, FrameColor.Black),

                                                                  new ShowMagicEffectOutgoingPacket(Target.Tile.Position, MagicEffectType.Puff) );
                    }
                }

                server.QueueForExecution(Constants.PlayerAttackSchedulerEvent(Player), Constants.PlayerAttackEventDelay, this);
            }
        }
    }
}