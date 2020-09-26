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

        public override void Execute(Context context)
        {
            if ( !Player.Tile.Position.IsInBattleRange(Target.Tile.Position) )
            {
                Player.AttackTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                Player.FollowTarget = null;

                context.Server.CancelQueueForExecution(Constants.CreatureAttackSchedulerEvent(Player) );

                context.AddPacket(Player.Client.Connection, new ShowWindowTextOutgoingPacket(TextColor.WhiteBottomGameWindow, Constants.TargetLost),
                        
                                                            new StopAttackAndFollowOutgoingPacket(0) );
            }
            else
            {
                if ( Player.Tile.Position.IsNextTo(Target.Tile.Position) )
                {
                    context.AddPacket(Player.Client.Connection, new ShowMagicEffectOutgoingPacket(Target.Tile.Position, MagicEffectType.Puff) );

                    if (Target is Player observer)
                    {
                        context.AddPacket(observer.Client.Connection, new SetFrameColorOutgoingPacket(Player.Id, FrameColor.Black),

                                                                  new ShowMagicEffectOutgoingPacket(Target.Tile.Position, MagicEffectType.Puff) );
                    }
                }

                context.Server.QueueForExecution(Constants.CreatureAttackSchedulerEvent(Player), Constants.CreatureAttackEventDelay, this);
            }
        }
    }
}